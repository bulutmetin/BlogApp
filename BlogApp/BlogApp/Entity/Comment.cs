using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Entity
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        public string? Text { get; set; }
        public DateTime PublishedOn { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; } = null!;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
