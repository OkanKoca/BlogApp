﻿using BlogApp.Entity;

namespace BlogApp.Models
{
    public class PostsAndTagsViewModel
    {
        public List<Post> Posts { get; set; } = new List<Post>();
        public List<Tag> Tags { get; set; } = new List<Tag>();
    }
}
