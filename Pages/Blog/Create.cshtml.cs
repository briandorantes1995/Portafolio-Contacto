using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Portafolio.Data;
using Portafolio.Models;
using Portafolio.Services.Storage;

namespace Portafolio.Pages.Blog;

[Authorize(Roles = "Admin")]
public class CreateModel : PageModel
{
    private readonly UserDb _context;
    private readonly IBlobService _blobService;

    public CreateModel(UserDb context,IBlobService blobService)
    {
        _context = context;
        _blobService = blobService;
    }

    [BindProperty]
    public BlogPostInput Input { get; set; } = new();
    
    [BindProperty]
    public IFormFile? CoverImage { get; set; }

    public void OnGet()
    {
        Input.PublishedAt = DateTime.UtcNow.Date;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (CoverImage != null && !CoverImage.ContentType.StartsWith("image/"))
            ModelState.AddModelError(nameof(CoverImage), "La portada debe ser una imagen");

        if (!ModelState.IsValid)
            return Page();

        var now = DateTime.UtcNow;
        var post = new BlogPost
        {
            AuthorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value),
            Title = Input.Title,
            Summary = Input.Summary,
            Content = Input.Content,
            PublishedAt = ToUtc(Input.PublishedAt),
            CreatedAt = now,
            UpdatedAt = now
        };
        
        if (CoverImage != null)
        {
            post.CoverImageUrl = await _blobService.UploadFileAsync(CoverImage, "blogpost");
        }

        _context.BlogPosts.Add(post);
        await _context.SaveChangesAsync();

        return RedirectToPage("/Blog/Details", new { id = post.Id });
    }

    public class BlogPostInput
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Summary { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime? PublishedAt { get; set; }
    }

    // El input de fecha llega sin zona horaria; Postgres exige UTC
    public static DateTime? ToUtc(DateTime? date) =>
        date.HasValue ? DateTime.SpecifyKind(date.Value.Date, DateTimeKind.Utc) : null;
}
