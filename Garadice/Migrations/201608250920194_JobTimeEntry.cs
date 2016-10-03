namespace Garadice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobTimeEntry : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JobTimeEntry",
                c => new
                    {
                        JobTimeEntryID = c.Int(nullable: false, identity: true),
                        Description = c.String(maxLength: 300),
                        Start = c.DateTime(nullable: false),
                        StartTime = c.Time(nullable: false, precision: 7),
                        NumberOfHours = c.Time(nullable: false, precision: 7),
                        JobID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.JobTimeEntryID)
                .ForeignKey("dbo.Job", t => t.JobID, cascadeDelete: true)
                .Index(t => t.JobID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobTimeEntry", "JobID", "dbo.Job");
            DropIndex("dbo.JobTimeEntry", new[] { "JobID" });
            DropTable("dbo.JobTimeEntry");
        }
    }
}
