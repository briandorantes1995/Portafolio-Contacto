using System.ComponentModel.DataAnnotations.Schema;
namespace Portafolio.Models;

public class InviteToken
{
    public int Id { get; set; }
  
    public int UserId { get; set; }

    public string Token { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }

    [ForeignKey("UserId")]
    public AppUser AppUser { get; set; } = null!;
}