using BlogApp.Data.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.ViewComponents
{
    public class RecentPostsViewComponent : ViewComponent
    {
        private readonly IPostRepository _postRepository;

        public RecentPostsViewComponent(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(
                    await 
                    _postRepository
                    .Posts
                    .OrderByDescending(p=> p.PublishedOn)
                    .Take(5)
                    .ToListAsync()
                );
        }
    }
}
