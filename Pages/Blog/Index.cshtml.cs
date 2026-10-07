using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Portafolio.Data;
using Portafolio.Models;

namespace Portafolio.Pages.Blog;

[AllowAnonymous]
public class Index : PageModel
{
    private readonly UserDb _context;

    public Index(UserDb context)
    {
        _context = context;
    }

    public List<BlogPost> Posts { get; set; } = new();

    public async Task OnGetAsync()
    {
        var query = _context.BlogPosts.Include(b => b.Author).AsQueryable();

        // Los usuarios solo ven posts cuya fecha de publicacion ya llego; el admin ve todo
        if (!User.IsInRole("Admin"))
        {
            var now = DateTime.UtcNow;
            query = query.Where(b => b.PublishedAt != null && b.PublishedAt <= now);
        }

        Posts = await query.OrderByDescending(b => b.PublishedAt ?? b.CreatedAt).ToListAsync();
    }
}
