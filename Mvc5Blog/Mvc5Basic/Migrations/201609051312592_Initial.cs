namespace Mvc5Basic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.Post",
                c => new
                    {
                        PostId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.PostId);
            
            CreateTable(
                "dbo.PostCategory",
                c => new
                    {
                        PostId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PostId, t.CategoryId })
                .ForeignKey("dbo.Post", t => t.PostId, cascadeDelete: true)
                .ForeignKey("dbo.Category", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.PostId)
                .Index(t => t.CategoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PostCategory", "CategoryId", "dbo.Category");
            DropForeignKey("dbo.PostCategory", "PostId", "dbo.Post");
            DropIndex("dbo.PostCategory", new[] { "CategoryId" });
            DropIndex("dbo.PostCategory", new[] { "PostId" });
            DropTable("dbo.PostCategory");
            DropTable("dbo.Post");
            DropTable("dbo.Category");
        }
    }
}
