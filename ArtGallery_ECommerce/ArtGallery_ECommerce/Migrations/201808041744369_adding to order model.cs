namespace ArtGallery_ECommerce.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingtoordermodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "PublicOrderNum", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "PublicOrderNum");
        }
    }
}
