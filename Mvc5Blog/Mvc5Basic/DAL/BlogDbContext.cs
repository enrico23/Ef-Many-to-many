using Mvc5Basic.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Mvc5Basic.DAL
{
    public class BlogDbContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }

        public BlogDbContext()
        {
            //base.Configuration.ProxyCreationEnabled = false;
            //Database.SetInitializer<BlogDbContext>(null);

        }

        protected override void OnModelCreating(DbModelBuilder dbModelBuilder)
        {
            dbModelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            dbModelBuilder.Entity<Post>()
                .HasMany<Category>(s => s.categories)
                .WithMany(c => c.posts)
                .Map(cs =>
                {
                    cs.MapLeftKey("PostId");
                    cs.MapRightKey("CategoryId");
                    cs.ToTable("PostCategory");
                });

        }



    }
}