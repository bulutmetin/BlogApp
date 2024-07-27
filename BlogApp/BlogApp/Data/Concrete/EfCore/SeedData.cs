using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Concrete.EfCore
{
    public static class SeedData
    {
        public static void FillTheTestDatas(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetService<BlogContext>();

            if (context != null)
            {
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }

                if (!context.Tags.Any())
                {
                    context.Tags.AddRange(
                        new Tag { Text = "Yapay Zeka", Url = "yapay-zeka", Colors = TagColors.primary },
                        new Tag { Text = "Teknoloji", Url = "teknoloji", Colors = TagColors.danger },
                        new Tag { Text = "Bilim", Url = "bilim", Colors = TagColors.info },
                        new Tag { Text = "Kültür ve Saanat", Url = "kültür-ve-sanat", Colors = TagColors.success },
                        new Tag { Text = "Ekonomi", Url = "ekonomi", Colors = TagColors.secondary },
                        new Tag { Text = "Sağlık", Url = "saglik", Colors = TagColors.danger },
                        new Tag { Text = "Sosyal Medya", Url = "sosyal-medya", Colors = TagColors.info },
                        new Tag { Text = "Egitim", Url = "egitim-ve-ogrenme", Colors = TagColors.secondary },
                        new Tag { Text = "Yazilim", Url = "yazilim", Colors = TagColors.secondary });

                    context.SaveChanges();
                }
                if (!context.Users.Any())
                {
                    context.Users.AddRange(
                        new User { Username = "metinbulut", Image = "metin.jpg", Email = "info@metinbulut.com", Password = "123456" },
                        new User { Username = "toprakbulut", Image = "avatar.jpg", Email = "info@toprakbulut.com", Password = "12345" }

                    );
                    context.SaveChanges();
                }

                if (!context.Articles.Any())
                {
                    context.Articles.AddRange(
                        new Article
                        {
                            Title = "Yapay Zeka ve Makine Öğrenmesi",
                            Content = "Yapay zeka (YZ) ve makine öğrenmesi (MÖ) günümüz teknolojisinin temel taşları haline geldi. Bu makalede, YZ ve MÖ'nün temel kavramları, uygulama alanları ve gelecekteki potansiyel etkileri ele alınacaktır. Ayrıca, bu teknolojilerin endüstri, sağlık ve günlük yaşam üzerindeki etkileri tartışılacaktır.",
                            Description = "Yapay Zeka ve Makine Öğrenmesi Nedir",
                            Url = "yapay-zeka-ve-makine-ogrenmesi",
                            IsActive = true,
                            PublishedOn = DateTime.Now.AddDays(-15),
                            Tags = context.Tags.Take(3).ToList(),
                            Image = "yapayzeka.jpg",
                            UserId = 1,
                            Comments = new List<Comment>()
                            {
                                new Comment{Text="Basarılı bir yazı olmuş",PublishedOn= new DateTime(),UserId=1},
                                new Comment{Text="Faydalı bir makale ",PublishedOn= new DateTime(),UserId=2}
                            }
                        },
                        new Article
                        {
                            Title = "Unity ile oyun geliştirme",
                            Content = "Unity ile oyunlar geliştirebilirsiniz",
                            Description = "Unity editörünü tanımlayalım",
                            Url = "unity-ile-oyun-geliştirme",
                            IsActive = true,
                            PublishedOn = DateTime.Now.AddDays(-10),
                            Tags = context.Tags.Take(3).ToList(),
                            Image = "2.png",
                            UserId = 2
                        },
                        new Article
                        {
                            Title = "Full Stack Developer olmak",
                            Content = "Full Stack Developer olmak güzeldir",
                            Description = "full stack developer nasıl olunur",
                            Url = "full-stack-developer",
                            IsActive = true,
                            PublishedOn = DateTime.Now.AddDays(-5),
                            Tags = context.Tags.Take(4).ToList(),
                            Image = "1.png",
                            UserId = 1
                        }
                    );
                    context.SaveChanges();
                }
            }


        }
    }
}
