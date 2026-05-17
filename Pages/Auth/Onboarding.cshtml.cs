using Microsoft.AspNetCore.Mvc.RazorPages;
using Portafolio.Models;
using Portafolio.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portafolio.Services.Auth;

namespace Portafolio.Pages.Auth;

public class Onboarding : PageModel
{
    
    private readonly UserDb _context;
    private readonly IPasswordService _passwordService;

    public Onboarding(UserDb context, IPasswordService passwordService)
    {
        _context = context;
        _passwordService = passwordService;
    }
    
    public InviteToken InviteToken { get; set; } = null!;
    
    [BindProperty]
    public AppUser UsuarioOnboarding { get; set; } = null!;
    
    [BindProperty]
    public string  Password { get; set; } = string.Empty;
    [BindProperty]
    public string ConfirmPassword { get; set; } = string.Empty;
    

    public async Task<IActionResult> OnGetAsync(string token)
    {
        var tokenusuario = await _context.InviteTokens.FirstOrDefaultAsync(t => t.Token == token);
        if (tokenusuario == null || tokenusuario.ExpiresAt < DateTime.UtcNow)
            return NotFound();
        var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.Id == tokenusuario.UserId);
        if (user == null)
            return NotFound();
        

        UsuarioOnboarding = user;
        InviteToken = tokenusuario;

        return Page();
    }
    
    public async Task<IActionResult> OnPostAsync(string token)
    {
        var tokenusuario = await _context.InviteTokens.FirstOrDefaultAsync(t => t.Token == token);
        if (tokenusuario == null || tokenusuario.ExpiresAt < DateTime.UtcNow)
            return NotFound();
        ModelState.Remove("UsuarioOnboarding.Email");

        ModelState.Remove("UsuarioOnboarding.Role");

        ModelState.Remove("UsuarioOnboarding.PasswordHash");
        if (!ModelState.IsValid)
            return Page();
        var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.Id == tokenusuario.UserId);
        
        if (user == null)
            return NotFound();
        
        if (Password != ConfirmPassword)
        {
            ModelState.AddModelError("", "Las contraseñas no coinciden");
            return Page();
        }

        user.Name = UsuarioOnboarding.Name;
        user.CompanyName = UsuarioOnboarding.CompanyName;
        user.PhoneNumber = UsuarioOnboarding.PhoneNumber;
        user.OnboardingComplete = true;
        user.UpdatedAt = DateTime.UtcNow;
        user.PasswordHash = _passwordService.HashPassword(Password);

        await _context.SaveChangesAsync();

        return RedirectToPage("/Index");
    }
}