using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Portafolio.Data;
using Portafolio.Models;

namespace Portafolio.Pages.ContactMessages;

public class CreateModel : PageModel
{
    private readonly UserDb _context;

    public CreateModel(UserDb context)
    {
        _context = context;
    }

    [BindProperty]
    public ContactMessage ContactMessage { get; set; } = new();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        ContactMessage.CreatedAt = DateTime.UtcNow;

        _context.ContactMessages.Add(ContactMessage);

        await _context.SaveChangesAsync();

        return RedirectToPage("/Index");
    }
}