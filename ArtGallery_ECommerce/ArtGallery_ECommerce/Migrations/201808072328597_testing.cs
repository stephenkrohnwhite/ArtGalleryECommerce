namespace ArtGallery_ECommerce.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class testing : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reviews", "ProductName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reviews", "ProductName");
        }
    }
}
