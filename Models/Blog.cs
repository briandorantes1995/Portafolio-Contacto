using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Portafolio.Models;

public class BlogPost
{
    public int Id { get; set; }
    public int AuthorId { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Summary { get; set; }

    [Required]
    public string Content { get; set; } = string.Empty;

    public string? CoverImageUrl { get; set; }

    // null = borrador; fecha futura = programado
    public DateTime? PublishedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    [ForeignKey("AuthorId")]
    public AppUser Author { get; set; } = null!;

    [NotMapped]
    public bool IsPublished => PublishedAt != null && PublishedAt <= DateTime.UtcNow;
}
