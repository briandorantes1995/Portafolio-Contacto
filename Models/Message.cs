using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Portafolio.Models;

public class Message
{
    public int Id { get; set; }
    public int JobId { get; set; }
    public int SenderId { get; set; }
    [Required]
    public string Content { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
    [ForeignKey("JobId")]
    public Job Job { get; set; } = null!;
    [ForeignKey("SenderId")]
    public AppUser Sender { get; set; } = null!;
}