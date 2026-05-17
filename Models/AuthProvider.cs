using System.ComponentModel.DataAnnotations.Schema;
namespace Portafolio.Models;

public enum Providers
{
    Google,
    Azure
}

public class AuthProvider
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public Providers Provider { get; set; }
    public string ProviderUserId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    [ForeignKey("UserId")]
    public AppUser AppUser { get; set; } = null!;
}