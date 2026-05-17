using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Portafolio.Services.Auth;
using Portafolio.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace Portafolio.Pages.Auth;

public class Login : PageModel
{
    private readonly IPasswordService _passwordService;
    private readonly UserDb _context;
    
    public Login(UserDb context, IPasswordService passwordService)
    {
        _context = context;
        _passwordService = passwordService;
    }
    
    [BindProperty]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    public string Password { get; set; } = string.Empty;
    
    public void OnGet()
    {
    }
    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.Email == Email);

        if (user == null || user.PasswordHash == null)
        {
            ModelState.AddModelError("", "Credenciales invalidas");

            return Page();
        }
        
        
        var validPassword = _passwordService.VerifyPassword(user.PasswordHash, Password);

        if (!validPassword)
        {
            ModelState.AddModelError("", "Credenciales invalidas");

            return Page();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim("AvatarUrl", user.AvatarUrl ?? "")
        };

        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme
        );

        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal
        );

        return RedirectToPage("/Index");
    }
}