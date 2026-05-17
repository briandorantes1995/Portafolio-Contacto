using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Portafolio.Data;
using Portafolio.Models;
using Microsoft.AspNetCore.Authorization;
namespace Portafolio.Pages.UsersPages;

[Authorize(Roles = "Admin")]
public class CreateModel : PageModel
{
    private readonly UserDb _context;

    public CreateModel(UserDb context)
    {
        _context = context;
    }

    [BindProperty]
    public AppUser NewUser { get; set; } = new();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        NewUser.CreatedAt = DateTime.UtcNow;
        NewUser.UpdatedAt = DateTime.UtcNow;

        _context.AppUsers.Add(NewUser);

        await _context.SaveChangesAsync();

        return RedirectToPage("/UsersPages/Details", new { id = NewUser.Id });
    }
}