using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Portafolio.Data;
using Portafolio.Services.Storage;

namespace Portafolio.Pages.Blog;

[Authorize(Roles = "Admin")]
public class EditModel : PageModel
{
    private readonly UserDb _context;
    private readonly IBlobService _blobService;

    public EditModel(UserDb context, IBlobService blobService)
    {
        _context = context;
        _blobService = blobService;
    }

    [BindProperty]
    public CreateModel.BlogPostInput Input { get; set; } = new();

    [BindProperty]
    public IFormFile? CoverImage { get; set; }

    public string? CurrentCoverImageUrl { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var post = await _context.BlogPosts.FirstOrDefaultAsync(b => b.Id == id);
        if (post == null)
            return NotFound();

        Input = new CreateModel.BlogPostInput
        {
            Title = post.Title,
            Summary = post.Summary,
            Content = post.Content,
            PublishedAt = post.PublishedAt
        };
        CurrentCoverImageUrl = post.CoverImageUrl;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var post = await _context.BlogPosts.FirstOrDefaultAsync(b => b.Id == id);
        if (post == null)
            return NotFound();

        if (CoverImage != null && !CoverImage.ContentType.StartsWith("image/"))
            ModelState.AddModelError(nameof(CoverImage), "La portada debe ser una imagen");

        if (!ModelState.IsValid)
        {
            CurrentCoverImageUrl = post.CoverImageUrl;
            return Page();
        }

        post.Title = Input.Title;
        post.Summary = Input.Summary;
        post.Content = Input.Content;
        post.PublishedAt = CreateModel.ToUtc(Input.PublishedAt);
        post.UpdatedAt = DateTime.UtcNow;

        if (CoverImage != null)
        {
            post.CoverImageUrl = await _blobService.UploadFileAsync(CoverImage, "blogpost");
        }

        await _context.SaveChangesAsync();

        return RedirectToPage("/Blog/Details", new { id = post.Id });
    }
}
