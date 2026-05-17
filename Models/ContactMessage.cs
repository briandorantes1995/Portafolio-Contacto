using System.ComponentModel.DataAnnotations;

namespace Portafolio.Models;

public class ContactMessage
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public string? CompanyName { get; set; }

    [Required]
    public string Message { get; set; } = string.Empty;

    public decimal? Budget { get; set; }

    public bool Reviewed { get; set; }

    public bool Accepted { get; set; }

    public DateTime CreatedAt { get; set; }
}