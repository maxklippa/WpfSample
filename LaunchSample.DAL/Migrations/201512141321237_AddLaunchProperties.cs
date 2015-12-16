namespace LaunchSample.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLaunchProperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Launches", "City", c => c.String());
            AddColumn("dbo.Launches", "StartDateTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Launches", "EndDateTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Launches", "Month", c => c.Int(nullable: false));
            AddColumn("dbo.Launches", "Status", c => c.Int(nullable: false));
            DropColumn("dbo.Launches", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Launches", "Name", c => c.String());
            DropColumn("dbo.Launches", "Status");
            DropColumn("dbo.Launches", "Month");
            DropColumn("dbo.Launches", "EndDateTime");
            DropColumn("dbo.Launches", "StartDateTime");
            DropColumn("dbo.Launches", "City");
        }
    }
}
