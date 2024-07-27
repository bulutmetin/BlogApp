using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Entity
{
    public enum TagColors
    {
        primary, secondary, warning, danger, info, success
    }
    public class Tag
    {
        [Key]
        public int TagId { get; set; }
        public string? Text { get; set; }
        public string? Url { get; set; }
        public TagColors? Colors { get; set; }
        public List<Article> Articles { get; set; }
    }
}
