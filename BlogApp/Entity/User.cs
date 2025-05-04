namespace BlogApp.Entity
{
    public class User
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? Image{ get; set; }
        public List<Post> Posts = new List<Post>();
        public List<Comment> Comments = new List<Comment>();
    }
}
