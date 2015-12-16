namespace LaunchSample.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeMonth : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Launches", "Month", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Launches", "Month", c => c.Int(nullable: false));
        }
    }
}
