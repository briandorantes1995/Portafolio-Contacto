using Portafolio.Models;
namespace Portafolio.DTOs;
using System.ComponentModel.DataAnnotations;

public class UpdateUserInput
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public string? CompanyName { get; set; }
    
    public string? PhoneNumber { get; set; }
    
    public Roles Role { get; set; } = Roles.User;
    
    public string? AvatarUrl { get; set; }
    
    public ContactPreferences ContactPreference { get; set; } = ContactPreferences.Email;
}
