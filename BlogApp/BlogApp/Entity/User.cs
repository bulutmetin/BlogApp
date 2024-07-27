using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Entity
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? Image { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        public List<Article> Articles { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
