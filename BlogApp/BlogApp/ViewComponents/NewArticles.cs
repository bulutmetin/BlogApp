using BlogApp.Data.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.ViewComponents
{
    public class NewArticles:ViewComponent
    {
        private IArticleRepository _articleRepository;
        public NewArticles(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _articleRepository.Articles.OrderBy(p => p.PublishedOn).Take(5).ToListAsync());
        }
    }
}
