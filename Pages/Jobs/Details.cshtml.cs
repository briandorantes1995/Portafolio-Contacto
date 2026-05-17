using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Portafolio.Data;
using Portafolio.Models;
using System.Security.Claims;
using Portafolio.Services.Storage;
namespace Portafolio.Pages.Jobs;

[Authorize]
public class Details : PageModel
{
    private readonly UserDb _context;
    private readonly IBlobService _blobService;

    public Details(UserDb context, IBlobService blobService)
    {
        _context = context;
        _blobService = blobService;
    }
    
    public Job JobDetails{ get; set; } = null!;
    public List<JobHistory>  History{ get; set; } = null!;
    public List<JobFile>  Files{ get; set; } = null!;
    public List<Message>  Messages{ get; set; } = null!;

    [BindProperty]
    public string NewMessage { get; set; } = string.Empty;
    
    [BindProperty]
    public IFormFile Upload { get; set; } = null!;
    
    public async Task<IActionResult> OnGetAsync(int id)
    {
        var job = await _context.Jobs.FirstOrDefaultAsync(j => j.Id == id);
        if (job == null)
            return NotFound();
        
        var messages = await _context.Messages.Include(m => m.Sender).Where(m => m.JobId == id).OrderBy(m => m.CreatedAt).ToListAsync();
        var history = await _context.JobHistories.Include(h => h.Changer).Where(h => h.JobId == job.Id).ToListAsync();
        var file =await _context.JobFiles.Include(f=>f.Uploader).Where(f => f.JobId == job.Id).OrderBy(f=>f.UpdatedAt).ToListAsync<JobFile>();
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        

        if (!User.IsInRole("Admin") && currentUserId != job.UserId.ToString())
            return Forbid();

        JobDetails = job;
        History = history;
        Files = file;
        Messages = messages;
        
        return Page();
    }

    public async Task<IActionResult> OnPostSendMessageAsync(int id)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var message = new Message
        {
            JobId = id,
            SenderId = int.Parse(currentUserId),
            Content = NewMessage,
            CreatedAt = DateTime.UtcNow
        };
        _context.Messages.Add(message);
        await _context.SaveChangesAsync();
        return RedirectToPage(new { id });
    }
    
    public async Task<IActionResult> OnPostUploadFileAsync(int id)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var fileUrl = await _blobService.UploadFileAsync(Upload, "jobfiles");

        var file = new JobFile
        {
            JobId = id,
            UploaderId = int.Parse(currentUserId),
            FileName = Upload.FileName,
            FileSize = Upload.Length,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            FileUrl = fileUrl
        };

        _context.JobFiles.Add(file);

        await _context.SaveChangesAsync();

        return RedirectToPage(new { id });
    }
    
    public async Task<IActionResult> OnGetDownloadAsync(int fileId)
    {
        var file = await _context.JobFiles.Include(f => f.Job).FirstOrDefaultAsync(f => f.Id == fileId);

        if (file == null)
            return NotFound();

        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!User.IsInRole("Admin") && currentUserId != file.Job.UserId.ToString())
        {
            return Forbid();
        }

        var blobName = Path.GetFileName(file.FileUrl);

        var downloadUrl = await _blobService.GenerateDownloadUrlAsync(blobName, "jobfiles");

        return Redirect(downloadUrl);
    }
}