using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Portafolio.Data;
using Portafolio.Models;
using Portafolio.Services.Mails;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;

namespace Portafolio.Pages.ContactMessages;

[Authorize(Roles = "Admin")]
public class IndexModel : PageModel
{
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;
    private readonly UserDb _context;

    public IndexModel(UserDb context,IEmailService emailService,IConfiguration configuration)
    {
        _context = context;
        _emailService = emailService;
        _configuration = configuration;
    }

    public List<ContactMessage> ContactMessages { get; set; } = new();
    
    public void OnGet()
    {
        ContactMessages = _context.ContactMessages.ToList();
    }
    
    public async Task<IActionResult> OnPostReviewAsync(int id)
    {
        var message = await _context.ContactMessages.FirstOrDefaultAsync(m => m.Id == id);

        if (message == null)
            return NotFound();

        message.Reviewed = true;
        
        await _context.SaveChangesAsync();
        await _emailService.SendAsync(
            message.Email,
            "Solicitud recibida",
            "<h1>Tu solicitud está siendo revisada</h1>" +
            "<p>Estate Atento si necesitamos mas informacion<p/>"
        );

        return RedirectToPage("/ContactMessages/Index");
    }
    
    
    public async Task<IActionResult> OnPostAcceptAsync(int id)
    {
        var message = await _context.ContactMessages.FirstOrDefaultAsync(m => m.Id == id);

        if (message == null)
            return NotFound();
 
        message.Reviewed = true;
        message.Accepted = true;

        var newUser = await _context.AppUsers.FirstOrDefaultAsync(u => u.Email == message.Email);
        if (newUser == null)
        {
             newUser = new AppUser
            {
                Name = message.Name,
                Email = message.Email,
                CompanyName = message.CompanyName
            };   
            _context.AppUsers.Add(newUser);
            await _context.SaveChangesAsync();
        }
        
        var newToken = await _context.InviteTokens.FirstOrDefaultAsync(t => t.UserId == newUser.Id);
        if (newToken  == null)
        {
            newToken = new InviteToken
            {
                UserId = newUser.Id,
                Token =  Convert.ToHexString(RandomNumberGenerator.GetBytes(32)),
                ExpiresAt = DateTime.UtcNow.AddDays(7)

            };
            _context.InviteTokens.Add(newToken);
            await _context.SaveChangesAsync();
        }
        var newJob = await _context.Jobs.FirstOrDefaultAsync(j => j.UserId == newUser.Id && j.Description == message.Message);
        if (newJob == null)
        {
             newJob = new Job
            {
                UserId = newUser.Id,
                Title = message.CompanyName,
                Description = message.Message,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,

            };
            _context.Jobs.Add(newJob);
            await _context.SaveChangesAsync();
        }
        var baseUrl = _configuration["App:BaseUrl"];
        var onboardingLink = $"{baseUrl}/Auth/Onboarding/{newToken.Token}";
        await _context.SaveChangesAsync();
        await _emailService.SendAsync(
            message.Email,
            "Solicitud Aceptada",
            "<h1>Tu solicitud ha sido Aceptada</h1>"+
                 "<p>Pronto Recibiras mas Informacion<p/>"+
                 $"<a href='{onboardingLink}'>" +
                 "Continua tu onboarding aqui para dar seguimiento" +
                 "</a>"
        );
        

        return RedirectToPage("/ContactMessages/Index");
    }
}