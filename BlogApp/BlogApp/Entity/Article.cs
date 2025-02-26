﻿using Azure;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Entity
{
    public class Article
    {
        [Key]
        public int ArticleId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Description { get; set; }
        public string? Url { get; set; }
        public string? Image { get; set; }
        public DateTime PublishedOn { get; set; }
        public bool IsActive { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public List<Tag> Tags { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
