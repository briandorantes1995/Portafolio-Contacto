using System.ComponentModel.DataAnnotations.Schema;
namespace Portafolio.Models;

public class JobFile
{
    public int Id { get; set; }
    public int JobId { get; set; }
    public int UploaderId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    [ForeignKey("JobId")]
    public Job Job { get; set; } = null!;
    [ForeignKey("UploaderId")]
    public AppUser Uploader { get; set; } = null!;
}