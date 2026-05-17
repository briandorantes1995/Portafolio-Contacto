using System.ComponentModel.DataAnnotations.Schema;
namespace Portafolio.Models;

public class JobHistory
{
    public int Id { get; set; }
    public int JobId { get; set; }
    public int ChangerId { get; set; }
    public Statuses OldStatus { get; set; }
    public Statuses NewStatus { get; set; }
    public SubStatuses? OldSubStatus { get; set; }
    public SubStatuses? NewSubStatus { get; set; }
    public DateTime CreatedAt { get; set; }
    [ForeignKey("JobId")]
    public Job Job { get; set; } = null!;
    [ForeignKey("ChangerId")]
    public AppUser Changer { get; set; } = null!;
}