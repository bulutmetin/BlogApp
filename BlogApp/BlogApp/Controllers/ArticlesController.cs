using BlogApp.Data.Abstract;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlogApp.Controllers
{
    public class ArticlesController : Controller
    {
        private IArticleRepository _articleRepository;
        private ICommentRepository _commentRepository;
        private ITagRepository _tagRepository;

        public ArticlesController(IArticleRepository articleRepository, ICommentRepository commentRepository, ITagRepository tagRepository)
        {
            _articleRepository = articleRepository;
            _commentRepository = commentRepository;
            _tagRepository = tagRepository;
        }
        public async Task<IActionResult> Index(string tag, string searchTag)
        {
            var articles = _articleRepository.Articles.Where(i => i.IsActive);

            if (!string.IsNullOrEmpty(tag))
            {
                articles = articles.Where(x => x.Tags.Any(t => t.Url == tag));
            }

            ViewBag.Tags = new SelectList(_tagRepository.Tags, "TagId", "Text");

            return View(new ArticleViewModel
            {
                Articles = await articles.ToListAsync(),
            });
        }

        public async Task<IActionResult> Details(string url)
        {
            return View(await _articleRepository.Articles.Include(x => x.User).Include(x => x.Tags).Include(x => x.Comments)
                .ThenInclude(x => x.User).FirstOrDefaultAsync(p => p.Url == url));
        }

        public IActionResult Search(string searchString)
        {
            var articles = _articleRepository.Articles;


            if (!String.IsNullOrEmpty(searchString))
            {
                ViewBag.SearchString = searchString;

                articles = articles.Where(p => p.Title.ToLower().Contains(searchString));
            }
            else
            {
                return RedirectToAction("Index");
            }

            return View(articles.ToList());
        }

        [HttpPost]
        [Authorize]
        public JsonResult AddComment(int ArticleId, string Text)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var username = User.FindFirstValue(ClaimTypes.Name);
            var avatar = User.FindFirstValue(ClaimTypes.UserData);

            var entity = new Comment
            {
                ArticleId = ArticleId,
                Text = Text,
                PublishedOn = DateTime.Now,
                UserId = int.Parse(userId ?? "")
            };

            _commentRepository.CreateComment(entity);

            return Json(new
            {
                username,
                Text,
                entity.PublishedOn,
                avatar
            });
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(ArticleCreateViewModel model, IFormFile imageFile)
        {

            const int maxFileSize = 2 * 1024 * 1024;
            var allowenExtensions = new[] { ".jpg", ".png", ".jpeg" };
            if (imageFile != null)
            {

                if (imageFile.Length > maxFileSize)
                {
                    ModelState.AddModelError("", "Dosya boyutu 2 MB den küçük olmalıdır.");
                }
                var extensions = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
                if (!allowenExtensions.Contains(extensions))
                {
                    ModelState.AddModelError("", "Geçerli bir resim seçiniz!");
                }
                else
                {
                    var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extensions}");
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);
                    try
                    {
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            imageFile.CopyTo(stream);
                        }
                        model.Image = randomFileName;
                    }
                    catch
                    {
                        ModelState.AddModelError("", "Dosya yüklenirken bir hata oluştu.");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Bir dosya seçiniz.");
            }


            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _articleRepository.CreateArticle(
                    new Article
                    {
                        Title = model.Title,
                        Content = model.Content,
                        Description = model.Description,
                        Url = model.Url,
                        UserId = int.Parse(userId ?? ""),
                        Image = model.Image,
                        IsActive = false,

                    }
                );
                return RedirectToAction("Index");
            }
            return View(model);


        }

        [Authorize]
        public async Task<IActionResult> List()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "");
            var role = User.FindFirstValue(ClaimTypes.Role);

            var articles = _articleRepository.Articles;

            if (string.IsNullOrEmpty(role))
            {
                articles = articles.Where(i => i.UserId == userId);
            }

            return View(await articles.ToListAsync());
        }

        [Authorize]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var article = _articleRepository.Articles.Include(x => x.Tags).FirstOrDefault(i => i.ArticleId == id);

            if (article == null)
            {
                return NotFound();
            }

            ViewBag.Tags = _tagRepository.Tags.ToList();
            return View(
                new ArticleCreateViewModel
                {
                    ArticleId = article.ArticleId,
                    Title = article.Title,
                    Description = article.Description,
                    Content = article.Content,
                    Url = article.Url,
                    IsActive = article.IsActive,
                    Tags = article.Tags
                }

                );
        }
     
        [HttpPost]
        [Authorize]

        public IActionResult Edit(ArticleCreateViewModel model, int[] tagIds)
        {
            if (ModelState.IsValid)
            {
                var entityUpdate = new Article
                {
                    ArticleId = model.ArticleId,
                    Title = model.Title,
                    Description = model.Description,
                    Content = model.Content,
                    Url = model.Url,

                };
                if (User.FindFirstValue(ClaimTypes.Role) == "admin")
                {
                    entityUpdate.IsActive = model.IsActive;
                }
                _articleRepository.EditArticle(entityUpdate, tagIds);
                return RedirectToAction("List");
            }
            return View(model);
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            var article = _articleRepository.Articles.FirstOrDefault(a => a.ArticleId == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _articleRepository.DeleteArticle(id);
            return RedirectToAction("List");
        }

    }
}
