using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlogApp.Controllers
{
    public class PostsController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        public PostsController(IPostRepository postRepository, ICommentRepository commentRepository)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
        }
        public async Task<IActionResult> Index(string tag)
        {
            var claims = User.Claims;
            var posts = _postRepository.Posts;

            if (!string.IsNullOrEmpty(tag))
            {
                return View(await posts.Where(p => p.Tags.Any(t => t.Url == tag))
                    .Include(p => p.Tags)
                    .ToListAsync());
            }
            return View( await posts.Include(p => p.Tags).ToListAsync());
        }

        public async Task<IActionResult> Details(string? url) {
            return View(
                await
                _postRepository.Posts
                .Include(p => p.Tags)
                .Include(p => p.Comments)
                .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(p => p.Url == url)
                );
        }

        public IActionResult AddComment(int PostId,string UserName, string Text)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var avatar= User.FindFirstValue(ClaimTypes.UserData);

            var comment = new Comment
            {
                PostId = PostId,
                Text = Text,
                PublishedOn = DateTime.UtcNow,
                UserId = int.Parse(userId ?? "")
            };

            _commentRepository.CreateComment(comment);

            return RedirectToAction("Details", new { url = _postRepository.Posts.FirstOrDefault(p => p.PostId == PostId)!.Url });
        }
    }
}
