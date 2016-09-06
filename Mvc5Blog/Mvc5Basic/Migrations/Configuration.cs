namespace Mvc5Basic.Migrations
{
    using Mvc5Basic.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Mvc5Basic.DAL.BlogDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Mvc5Basic.DAL.BlogDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            var categories = new List<Category> { 
                new Category{Name="C Sharp"},
                new Category{Name="Asp.Net"},
                new Category{Name="JavaScript"}
            };
            categories.ForEach(s => context.Categories.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();


            var posts = new List<Post>{
                new Post{
                    Name="Post 1",
                    categories= new List<Category>{
                        categories.Single(s=> s.Name == "C Sharp"),
                        categories.Single(s=> s.Name == "Asp.Net")
                    }
                },
                new Post{
                    Name="Post 2"
                    ,
                    categories = new List<Category>{
                        categories.Single(s=> s.Name =="JavaScript")
                    }
                },
            };
            posts.ForEach(s => context.Posts.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();
        }
    }
}
