using Microsoft.AspNetCore.Mvc.RazorPages;
using Portafolio.Data;
using Portafolio.Models;
using Microsoft.AspNetCore.Authorization;

namespace Portafolio.Pages.UsersPages;
[Authorize(Roles = "Admin")]
public class IndexModel : PageModel
{
    private readonly UserDb _context;

    public IndexModel(UserDb context)
    {
        _context = context;
    }

    public List<AppUser> AppUsers { get; set; } = new();

    public void OnGet()
    {
        AppUsers = _context.AppUsers.ToList();
    }
}