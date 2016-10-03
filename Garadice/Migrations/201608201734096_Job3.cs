namespace Garadice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Job3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobTrack", "JobID", c => c.Int(nullable: false));
            CreateIndex("dbo.JobTrack", "JobID");
            AddForeignKey("dbo.JobTrack", "JobID", "dbo.Job", "JobID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobTrack", "JobID", "dbo.Job");
            DropIndex("dbo.JobTrack", new[] { "JobID" });
            DropColumn("dbo.JobTrack", "JobID");
        }
    }
}
