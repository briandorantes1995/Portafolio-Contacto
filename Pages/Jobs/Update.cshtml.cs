using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Portafolio.Data;
using Portafolio.DTOs;
using Portafolio.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Portafolio.Pages.Jobs;

[Authorize(Roles = "Admin")]
public class Update : PageModel
{
    
    private readonly UserDb _context;

    public Update(UserDb context)
    {
        _context = context;
    }
    
    [BindProperty]
    public UpdateJobInput Input { get; set; } = new();
    
    public async Task<IActionResult> OnGetAsync(int id)
    {
        var job = await _context.Jobs.FirstOrDefaultAsync(j => j.Id == id);

        if (job == null)
            return NotFound();
        Input = new UpdateJobInput
        {
            Title = job.Title,
            Budget = job.Budget,
            Description = job.Description,
            Status = job.Status,
            SubStatus = job.SubStatus,
            StartDate = job.StartDate,
            EndDate = job.EndDate
        };
        
        return Page();
    }
    
    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (!ModelState.IsValid)
            return Page();

        var job = await _context.Jobs.FirstOrDefaultAsync(j => j.Id == id);
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (job == null)
            return NotFound();

        if (job.SubStatus != Input.SubStatus ||  job.Status != Input.Status)
        {
            var history = new JobHistory
            {
                JobId = job.Id,
                ChangerId = int.Parse(currentUserId),
                OldStatus =  job.Status,
                NewStatus =  Input.Status,
                OldSubStatus =  job.SubStatus,
                NewSubStatus =  Input.SubStatus,
                CreatedAt = DateTime.UtcNow,
            };
                _context.JobHistories.Add(history);
            await _context.SaveChangesAsync();
        }
        
        job.Title = Input.Title;
        job.Budget= Input.Budget;
        job.Description = Input.Description;
        job.Status=  Input.Status;
        job.SubStatus = Input.SubStatus;
        job.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return RedirectToPage("/Jobs/Details", new { id = job.Id });
    }
}