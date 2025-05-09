using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;

namespace BlogApp.Data.Concrete.EfCore
{
    public static class SeedData
    {
        public static void CreateTestDatas(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<BlogContext>();

            if(context != null)
            {
                if(context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }

                if (!context.Tags.Any())
                {
                    context.Tags.AddRange(
                        new Tag { Text = "C#" , Url = "csharp"},
                        new Tag { Text = "ASP.NET Core", Url = "aspnet-core"},
                        new Tag { Text = "Entity Framework Core", Url = "entity-framework-core"},
                        new Tag { Text = "JavaScript", Url = "javascript"},
                        new Tag { Text = "HTML", Url="html"},
                        new Tag { Text = "CSS", Url="css"}
                    );
                    context.SaveChanges();
                }

                if(!context.Users.Any())
                {
                    context.Users.AddRange(
                        new User { UserName = "okankoca", Email="okankoca@gmail.com",Password="123456", Image = "p1.jpg" },
                        new User { UserName = "ahmetkaya",Email="ahmetkaya@gmail.com",Password="1234567" , Image = "p1.jpg"},
                        new User { UserName = "ayseozdemir",Email= "ayseozdemir@gmail.com",Password= "12345678", Image = "p2.jpg"}
                );
                    context.SaveChanges();
                }

                if (!context.Posts.Any())
                {
                    var allTags = context.Tags.OrderBy(t => t.TagId).ToList();

                    foreach(var tag in allTags) {
                        Console.WriteLine(tag.Text);
                    }
                    context.Posts.AddRange(
                        new Post
                        {
                            Title = "C# 10 Features",
                            Content = "C# 10 introduces several new features, including global using directives and file-scoped namespaces.",
                            Description = "C# 10 Features",
                            Url = "csharp-10-features",
                            Image = "img1.jpg",
                            PublishedOn = DateTime.UtcNow.AddDays(-29),
                            IsActive = true,
                            UserId = 1,
                            Tags = allTags.Take(4).ToList(),
                            Comments = new List<Comment> { new Comment { Text = "Great article!", PublishedOn= DateTime.UtcNow.AddDays(-5) ,UserId = 1 },
                                new Comment { Text = "Very informative.", PublishedOn= DateTime.UtcNow.AddDays(-2) ,UserId = 2 },
                                new Comment { Text = "I learned a lot.", PublishedOn= DateTime.UtcNow.AddDays(-1) ,UserId = 3 }
                            }
                        },
                        new Post
                        {
                            Title = "ASP.NET Core MVC",
                            Content = "ASP.NET Core MVC is a framework for building web applications using the Model-View-Controller pattern.",
                            Description = "ASP.NET Core MVC",
                            Url = "aspnet-core-mvc",
                            Image = "img2.jpg",
                            PublishedOn = DateTime.UtcNow.AddDays(-50),
                            IsActive = true,
                            UserId = 2,
                            Tags = allTags.Take(2).ToList()
                        },
                        new Post
                        {
                            Title = "Entity Framework Core",
                            Content = "Entity Framework Core is an ORM that enables .NET developers to work with databases using .NET objects.",
                            Description = "Entity Framework Core",
                            Url = "entity-framework-core",
                            Image = "img3.jpg",
                            PublishedOn = DateTime.UtcNow.AddDays(-15),
                            IsActive = true,
                            UserId = 3,
                            Tags = allTags.Take(2).ToList()
                        },
                        new Post
                        {
                            Title = "Docker for .NET Developers",
                            Content = "Docker simplifies the deployment process by containerizing .NET applications, making them portable and consistent across environments.",
                            Description = "Docker for .NET Developers",
                            Url = "docker-for-dotnet-developers",
                            Image = "img4.jpg",
                            PublishedOn = DateTime.UtcNow.AddDays(-35),
                            IsActive = true,
                            UserId = 1,
                            Tags = allTags.Skip(2).Take(3).ToList()
                        },
                        new Post
                        {
                            Title = "Minimal APIs in .NET 6",
                            Content = "Minimal APIs offer a lightweight way to build HTTP APIs with less ceremony and improved performance in .NET 6.",
                            Description = "Minimal APIs in .NET 6",
                            Url = "minimal-apis-dotnet-6",
                            Image = "img5.jpg",
                            PublishedOn = DateTime.UtcNow.AddDays(-10),
                            IsActive = true,
                            UserId = 2,
                            Tags = allTags.Skip(1).Take(4).ToList()
                        }
                    );

                    context.SaveChanges();
                }

            }
        }
    }
}
