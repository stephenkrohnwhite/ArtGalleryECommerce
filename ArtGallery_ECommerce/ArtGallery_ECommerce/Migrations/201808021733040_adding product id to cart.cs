namespace ArtGallery_ECommerce.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingproductidtocart : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "ProductId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "ProductId");
        }
    }
}
