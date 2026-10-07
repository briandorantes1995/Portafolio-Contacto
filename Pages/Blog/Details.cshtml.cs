using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Portafolio.Data;
using Portafolio.Models;

namespace Portafolio.Pages.Blog;

[AllowAnonymous]
public class Details : PageModel
{
    private readonly UserDb _context;

    public Details(UserDb context)
    {
        _context = context;
    }

    public BlogPost Post { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var post = await _context.BlogPosts.Include(b => b.Author).FirstOrDefaultAsync(b => b.Id == id);

        if (post == null || (!post.IsPublished && !User.IsInRole("Admin")))
            return NotFound();

        Post = post;
        return Page();
    }
}
