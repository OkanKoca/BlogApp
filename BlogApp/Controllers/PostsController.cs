using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlogApp.Controllers
{
    public class PostsController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly ITagRepository _tagRepository;
        public PostsController(IPostRepository postRepository, ICommentRepository commentRepository,
            ITagRepository tagRepository)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _tagRepository = tagRepository;
        }
        public async Task<IActionResult> Index(string? tag)
        {
            var claims = User.Claims;
            var posts = _postRepository.Posts;

            if (!string.IsNullOrEmpty(tag))
            {
                return View(await posts.Where(p => p.Tags.Any(t => t.Url == tag))
                    .Include(p => p.Tags).Where(p=> p.IsActive)
                    .ToListAsync());
            }
            return View( await posts.Include(p => p.Tags).Where(p=> p.IsActive).ToListAsync()); 
        }

        public async Task<IActionResult> Details(string url) {
            if (!string.IsNullOrEmpty(url))
            {
                return View(
                await
                _postRepository.Posts
                .Include(p => p.User)
                .Include(p => p.Tags)
                .Include(p => p.Comments)
                .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(p => p.Url == url)
                );
            }
            
            return View("Index");
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

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public IActionResult Create(PostCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var post = new Post
                {
                    Title = model.Title,
                    Content = model.Content,
                    Description = model.Description,
                    Url = model.Url,
                    Image = "anon.jpg",
                    PublishedOn = DateTime.UtcNow,
                    UserId = int.Parse(userId ?? ""),
                    IsActive = false
                };
                _postRepository.CreatePost(post);
                return RedirectToAction("Index");
            }
            return View();
        }
        public async Task<IActionResult> List()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "");
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            if (string.IsNullOrEmpty(userRole))
            {
                return View(await _postRepository.Posts.Include(p => p.Tags)
                    .Where(p => p.UserId == userId)
                    .ToListAsync());
            }
            return View(await _postRepository.Posts.Include(p => p.Tags).ToListAsync());
        }

        public IActionResult Edit(int? id, int[] tagIds)
        {
            if(id == null)
            {
                return NotFound();
            }

            var post = _postRepository.Posts.Include(p=> p.Tags).FirstOrDefault(p => p.PostId == id);

            if(post == null)
            {
                return NotFound();
            }

            ViewBag.Tags = _tagRepository.Tags.ToList();

            return View(new PostCreateViewModel
            {
                PostId = post.PostId,
                Content = post.Content,
                Description = post.Description,
                Title = post.Title,
                Url = post.Url,
                IsActive = post.IsActive,
                Tags = post.Tags
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(PostCreateViewModel model, int[] tagIds)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                {
                    return NotFound();
                }

                var postToUpdate = new Post
                {
                    PostId = model.PostId,
                    Content = model.Content,
                    Description = model.Description,
                    Title = model.Title,
                    Url = model.Url,
                    Tags = model.Tags
                }; 

                if (User.FindFirstValue(ClaimTypes.Role) == "Admin")
                {
                    postToUpdate.IsActive = model.IsActive;
                }

                if (postToUpdate == null)
                {
                    return NotFound();
                }

                _postRepository.UpdatePost(postToUpdate, tagIds);
                ViewBag.Tags = _tagRepository.Tags.ToList();
                return RedirectToAction("List");
            }

            return View(model);
            
        }
        [HttpGet]
        [HttpPost]  
        public IActionResult Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var post = _postRepository.Posts.FirstOrDefault(p => p.PostId == id);

            if (post == null) { 
                return NotFound();
            }

            _postRepository.DeletePost(post);
            return RedirectToAction("List");
        } 
    }
}
