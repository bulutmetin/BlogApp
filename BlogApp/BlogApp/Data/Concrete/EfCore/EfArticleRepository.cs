using BlogApp.Data.Abstract;
using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Concrete.EfCore
{
    public class EfArticleRepository:IArticleRepository
    {
        private BlogContext _context;

        public EfArticleRepository(BlogContext context)
        {
            _context = context;
        }
        public IQueryable<Article> Articles => _context.Articles;

        public void CreateArticle(Article article)
        {
            _context.Articles.Add(article);
            _context.SaveChanges();
        }

        public void EditArticle(Article article)
        {
            var entity = _context.Articles.FirstOrDefault(i => i.ArticleId == article.ArticleId);
            if (entity != null)
            {
                entity.Title = article.Title;
                entity.Description = article.Description;
                entity.Content = article.Content;
                entity.Url = article.Url;
                entity.IsActive = article.IsActive;

                _context.SaveChanges();
            }
        }

        public void EditArticle(Article article, int[] tagIds)
        {
            var entity = _context.Articles.Include(i => i.Tags).FirstOrDefault(i => i.ArticleId == article.ArticleId);
            if (entity != null)
            {
                entity.Title = article.Title;
                entity.Description = article.Description;
                entity.Content = article.Content;
                entity.Url = article.Url;
                entity.IsActive = article.IsActive;

                entity.Tags = _context.Tags.Where(tag => tagIds.Contains(tag.TagId)).ToList();

                _context.SaveChanges();
            }
        }

        public void DeleteArticle(int articleId)
        {
            var entity = _context.Articles.FirstOrDefault(i => i.ArticleId == articleId);
            if (entity != null)
            {
                _context.Articles.Remove(entity);
                _context.SaveChanges();
            }
        }
    }
}
