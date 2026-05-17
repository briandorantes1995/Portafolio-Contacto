using Microsoft.AspNetCore.Mvc.RazorPages;
using Portafolio.Data;
using Portafolio.Models;
using Portafolio.Services.Mails;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Portafolio.Pages.Jobs;

[Authorize]
public class Index : PageModel
{
    private readonly UserDb _context;

    public Index(UserDb context,IEmailService emailService,IConfiguration configuration)
    {
        _context = context;
    }

    public List<Job> Jobs { get; set; } = new();
    
    public void OnGet()
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (User.IsInRole("Admin"))
        {
            Jobs = _context.Jobs.ToList();
        }
        else
        {
            Jobs = _context.Jobs.Where(j => j.UserId == int.Parse(currentUserId!)).ToList();
        }
        
    }
}