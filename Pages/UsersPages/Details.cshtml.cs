using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Portafolio.Data;
using Portafolio.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
namespace Portafolio.Pages.UsersPages;

[Authorize]
public class DetailsModel : PageModel
{
    private readonly UserDb _context;

    public DetailsModel(UserDb context)
    {
        _context = context;
    }

    public AppUser UserDetail { get; set; } = null!;
    

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
            return NotFound();
        
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!User.IsInRole("Admin") && currentUserId != user.Id.ToString())
            return Forbid();

        UserDetail = user;

        return Page();
    }
    
    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
            return NotFound();
        
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!User.IsInRole("Admin") && currentUserId != user.Id.ToString())
            return Forbid();

        _context.AppUsers.Remove(user);

        await _context.SaveChangesAsync();

        return RedirectToPage("/UsersPages/Index");
    }
}