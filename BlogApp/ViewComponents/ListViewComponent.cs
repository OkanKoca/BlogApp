using BlogApp.Data.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.ViewComponents
{
    public class ListViewComponent : ViewComponent
    {
        private readonly IPostRepository _postRepository;
        public ListViewComponent(IPostRepository postRepository) { 
            _postRepository = postRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            return View(await _postRepository.Posts.FirstOrDefaultAsync(p => p.PostId == id));
        }
    }
}
