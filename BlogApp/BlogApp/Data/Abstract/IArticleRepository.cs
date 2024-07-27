using BlogApp.Entity;

namespace BlogApp.Data.Abstract
{
    public interface IArticleRepository
    {
        IQueryable<Article> Articles { get; }

        void CreateArticle(Article article);
        void EditArticle(Article article);
        void EditArticle(Article article, int[] tagIds);

        void DeleteArticle(int articleId);
    }
}
