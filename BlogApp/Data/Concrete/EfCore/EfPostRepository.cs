using BlogApp.Data.Abstract;
using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Concrete.EfCore
{
    public class EfPostRepository : IPostRepository
    {
        private readonly BlogContext _context;

        public EfPostRepository(BlogContext context)
        {
            _context = context;
        }
        public IQueryable<Post> Posts => _context.Posts;

        public void CreatePost(Post post)
        {
            _context.Posts.Add(post);
            _context.SaveChanges();
        }
        public void UpdatePost(Post post)
        {
            var postToUpdate = _context.Posts.FirstOrDefault(p=> p.PostId == post.PostId);

            if (postToUpdate != null) {
                postToUpdate.Title = post.Title;
                postToUpdate.Content = post.Content;
                postToUpdate.Description = post.Description;
                postToUpdate.Url = post.Url;
                postToUpdate.IsActive = post.IsActive;

                _context.SaveChanges();
            }
        }
        public void UpdatePost(Post post, int[] tagIds)
        {
            var postToUpdate = _context.Posts.Include(p=> p.Tags).FirstOrDefault(p => p.PostId == post.PostId);

            if (postToUpdate != null)
            {
                postToUpdate.Title = post.Title;
                postToUpdate.Content = post.Content;
                postToUpdate.Description = post.Description;
                postToUpdate.Url = post.Url;
                postToUpdate.IsActive = post.IsActive;

                postToUpdate.Tags = _context.Tags.Where(p => tagIds.Contains(p.TagId)).ToList();

                _context.SaveChanges();
            }

        }

        public void DeletePost(Post post)
        {
            var postToDelete = _context.Posts.FirstOrDefault(p => p.PostId == post.PostId);

            if(postToDelete != null)
            {
                _context.Posts.Remove(postToDelete);
                _context.SaveChanges();
            }
        }

    }
}
