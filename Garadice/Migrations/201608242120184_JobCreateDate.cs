namespace Garadice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobCreateDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Job", "CreatedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Job", "CreatedDate");
        }
    }
}
