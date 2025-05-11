using BlogApp.Entity;

namespace BlogApp.Data.Abstract
{
    public interface IPostRepository
    {
        IQueryable<Post> Posts { get; }
        void CreatePost(Post post);
        void UpdatePost(Post post);
        void UpdatePost(Post post, int[] tagIds);
        public void DeletePost(Post post);
    }
}
