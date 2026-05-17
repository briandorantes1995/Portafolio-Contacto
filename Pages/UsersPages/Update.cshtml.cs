using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Portafolio.Services.Storage;
using Portafolio.Data;
using Portafolio.Models;

using System.Security.Claims;

namespace Portafolio.Pages.UsersPages;
[Authorize]
public class EditModel : PageModel
{
    private readonly UserDb _context;
    private readonly IBlobService _blobService;

    public EditModel(UserDb context, IBlobService blobService)
    {
        _context = context;
        _blobService = blobService;
    }

    [BindProperty]
    public AppUser EditUser { get; set; } = null!;
    
    [BindProperty]
    public IFormFile AvatarUpload { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
            return NotFound();
        
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!User.IsInRole("Admin") && currentUserId != user.Id.ToString())
            return Forbid();
      

        EditUser = user;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (!ModelState.IsValid)
            return Page();

        var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
            return NotFound();
        
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!User.IsInRole("Admin") && currentUserId != user.Id.ToString())
            return Forbid();

        user.Name = EditUser.Name;
        user.CompanyName = EditUser.CompanyName;
        user.PhoneNumber = EditUser.PhoneNumber;
        user.Email = EditUser.Email;
        user.ContactPreference = EditUser.ContactPreference;
        user.UpdatedAt = DateTime.UtcNow;
        
        if (User.IsInRole("Admin"))
        {
            user.Role = EditUser.Role;
        }
        
        if (AvatarUpload != null)
        {
            var avatarUrl = await _blobService.UploadFileAsync(AvatarUpload, "avatars");
            user.AvatarUrl = avatarUrl;
        }

        await _context.SaveChangesAsync();

        return RedirectToPage("/UsersPages/Details", new { id = user.Id });
    }
}