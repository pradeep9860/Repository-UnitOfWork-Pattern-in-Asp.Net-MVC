namespace HRTest_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Timesheet : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Timesheets", "IsCheckIn", c => c.Boolean(nullable: false));
            AddColumn("dbo.Timesheets", "StarterId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Timesheets", "StarterId");
            DropColumn("dbo.Timesheets", "IsCheckIn");
        }
    }
}
