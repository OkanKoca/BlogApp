using BlogApp.Entity;

namespace BlogApp.Models
{
    public class PostCreateViewModel
    {
        public int PostId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Description { get; set; }
        public string? Url { get; set; }
        public bool IsActive { get; set; }
    }
}
