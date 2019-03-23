namespace HRTest_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateTimesheet : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Timesheets", "TimeDuration", c => c.Long(nullable: false));
            DropColumn("dbo.Timesheets", "IsCheckIn");
            DropColumn("dbo.Timesheets", "StarterId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Timesheets", "StarterId", c => c.Int(nullable: false));
            AddColumn("dbo.Timesheets", "IsCheckIn", c => c.Boolean(nullable: false));
            DropColumn("dbo.Timesheets", "TimeDuration");
        }
    }
}
