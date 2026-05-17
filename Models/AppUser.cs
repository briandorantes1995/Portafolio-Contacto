using System.ComponentModel.DataAnnotations;

namespace Portafolio.Models;

public enum Roles
{
    User,
    Admin
}

public enum ContactPreferences
{
    Email,
    Sms,
    Both
}

public class AppUser
{
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; } = string.Empty;
    
    public string? CompanyName { get; set; }
    
    public string? PhoneNumber { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    public string? PasswordHash { get; set; } = null ;

    public bool OnboardingComplete { get; set; } = false;
    
    public Roles Role { get; set; } = Roles.User;
    
    public string? AvatarUrl { get; set; }
    
    public ContactPreferences ContactPreference { get; set; } = ContactPreferences.Email;
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    
    public ICollection<Job> Jobs { get; set; } = new List<Job>();
    public ICollection<AuthProvider> AuthProviders { get; set; } = new List<AuthProvider>();
    public ICollection<JobFile> JobFiles { get; set; } = new List<JobFile>();
    public ICollection<JobHistory> JobHistories { get; set; } = new List<JobHistory>();
    public ICollection<InviteToken> InviteTokens { get; set; } = new List<InviteToken>();
}