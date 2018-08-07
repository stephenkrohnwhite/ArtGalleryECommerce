namespace ArtGallery_ECommerce.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removingfkpointerinordermodel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Carts", "OrderId", "dbo.Orders");
            DropIndex("dbo.Carts", new[] { "OrderId" });
            DropColumn("dbo.Carts", "OrderId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Carts", "OrderId", c => c.Int(nullable: false));
            CreateIndex("dbo.Carts", "OrderId");
            AddForeignKey("dbo.Carts", "OrderId", "dbo.Orders", "OrderId", cascadeDelete: true);
        }
    }
}
