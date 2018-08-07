namespace ArtGallery_ECommerce.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class testing1 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Carts", "ProductId");
            CreateIndex("dbo.OrderItems", "ProductId");
            AddForeignKey("dbo.Carts", "ProductId", "dbo.Products", "ProductId", cascadeDelete: true);
            AddForeignKey("dbo.OrderItems", "ProductId", "dbo.Products", "ProductId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderItems", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Carts", "ProductId", "dbo.Products");
            DropIndex("dbo.OrderItems", new[] { "ProductId" });
            DropIndex("dbo.Carts", new[] { "ProductId" });
        }
    }
}
