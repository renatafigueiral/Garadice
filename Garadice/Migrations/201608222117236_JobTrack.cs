namespace Garadice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobTrack : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Job", "JobStatus", c => c.Int(nullable: false));
            AddColumn("dbo.JobTrack", "NumberOfHours", c => c.Time(nullable: false, precision: 7));
            AlterColumn("dbo.JobTrack", "End", c => c.DateTime());
            DropColumn("dbo.Employee", "MaxHoursMonthly");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Employee", "MaxHoursMonthly", c => c.Time(precision: 7));
            AlterColumn("dbo.JobTrack", "End", c => c.DateTime(nullable: false));
            DropColumn("dbo.JobTrack", "NumberOfHours");
            DropColumn("dbo.Job", "JobStatus");
        }
    }
}
