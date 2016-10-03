namespace Garadice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Job2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JobTrack",
                c => new
                    {
                        JobTrackID = c.Int(nullable: false, identity: true),
                        Description = c.String(maxLength: 300),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.JobTrackID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.JobTrack");
        }
    }
}
