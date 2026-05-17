using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Portafolio.Models;

public enum Statuses
{
    Created,
    InProgress,
    Finished,
    Cancelled
}     

public enum SubStatuses
{
    Onboarding,
    WaitingForClient,
    Review,
    Blocked,
    PaymentPending
}


public class Job
{
    public int Id { get; set; }
    public int UserId { get; set; }
    
    [Required]
    [StringLength(150)]
    public string Title { get; set; } = string.Empty;
    [Required]
    public string Description { get; set; } = string.Empty;
    [Required]
    public Statuses Status{ get; set; } = Statuses.Created;
    [Required]
    public SubStatuses SubStatus{ get; set; } = SubStatuses.Onboarding;
    public decimal? Budget { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    [ForeignKey("UserId")]
    public AppUser AppUser { get; set; } = null!;
    public ICollection<Message> Messages { get; set; } = new List<Message>();
    public ICollection<JobFile> JobFiles { get; set; } = new List<JobFile>();
    public ICollection<JobHistory> JobHistories { get; set; } = new List<JobHistory>();
    
}