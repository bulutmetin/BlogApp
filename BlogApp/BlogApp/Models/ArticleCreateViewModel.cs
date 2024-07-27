using BlogApp.Entity;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class ArticleCreateViewModel
    {
        public int ArticleId { get; set; }
        [Required]
        [Display(Name = "Başlık")]
        public string? Title { get; set; }

        [Required]
        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        [Display(Name = "Image")]
        public string? Image { get; set; }

        [Required]
        [Display(Name = "İçerik")]
        public string? Content { get; set; }

        [Required]
        [Display(Name = "Url")]
        public string? Url { get; set; }

        public bool IsActive { get; set; }
        public List<Tag> Tags { get; set; } = new();
    }
}
