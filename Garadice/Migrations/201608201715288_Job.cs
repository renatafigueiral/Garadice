namespace Garadice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Job : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Job",
                c => new
                    {
                        JobID = c.Int(nullable: false, identity: true),
                        JobType = c.Int(nullable: false),
                        JobName = c.String(nullable: false, maxLength: 255),
                        Description = c.String(maxLength: 300),
                        EmployeeID = c.Int(nullable: false),
                        CompanyID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.JobID)
                .ForeignKey("dbo.Company", t => t.CompanyID, cascadeDelete: true)
                .ForeignKey("dbo.Employee", t => t.EmployeeID, cascadeDelete: true)
                .Index(t => t.EmployeeID)
                .Index(t => t.CompanyID);
            
            AlterColumn("dbo.Employee", "Position", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Job", "EmployeeID", "dbo.Employee");
            DropForeignKey("dbo.Job", "CompanyID", "dbo.Company");
            DropIndex("dbo.Job", new[] { "CompanyID" });
            DropIndex("dbo.Job", new[] { "EmployeeID" });
            AlterColumn("dbo.Employee", "Position", c => c.String(nullable: false, maxLength: 50));
            DropTable("dbo.Job");
        }
    }
}
