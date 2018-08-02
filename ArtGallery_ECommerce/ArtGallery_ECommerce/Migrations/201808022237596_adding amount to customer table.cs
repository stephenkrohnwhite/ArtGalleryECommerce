namespace ArtGallery_ECommerce.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingamounttocustomertable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "Amount", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "Amount");
        }
    }
}
