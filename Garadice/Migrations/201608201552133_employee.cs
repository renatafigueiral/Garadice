namespace Garadice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class employee : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Employee",
                c => new
                    {
                        EmployeeID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        Phone = c.String(),
                        Mobile = c.String(),
                        Email = c.String(nullable: false),
                        Position = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 300),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.EmployeeID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Employee");
        }
    }
}
