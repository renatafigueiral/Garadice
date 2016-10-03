namespace Garadice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Job4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Job", "EstimatedDuration", c => c.Time(precision: 7));
            AddColumn("dbo.Employee", "MaxHoursMonthly", c => c.Time(precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employee", "MaxHoursMonthly");
            DropColumn("dbo.Job", "EstimatedDuration");
        }
    }
}
