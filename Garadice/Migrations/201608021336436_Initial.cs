namespace Garadice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Company",
                c => new
                    {
                        CompanyID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        VatNumber = c.String(nullable: false),
                        CRO = c.String(nullable: false),
                        Phone = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        SiteUrl = c.String(),
                        Description = c.String(maxLength: 300),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CompanyID);
            
            CreateTable(
                "dbo.Contact",
                c => new
                    {
                        ContactID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        Phone = c.String(),
                        Mobile = c.String(),
                        Email = c.String(nullable: false),
                        Position = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 300),
                        CreatedDate = c.DateTime(nullable: false),
                        CompanyID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ContactID)
                .ForeignKey("dbo.Company", t => t.CompanyID, cascadeDelete: true)
                .Index(t => t.CompanyID);
            
            CreateTable(
                "dbo.Document",
                c => new
                    {
                        DocumentID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 255),
                        FileName = c.String(nullable: false, maxLength: 255),
                        ContentType = c.String(maxLength: 100),
                        Extension = c.String(maxLength: 100),
                        Content = c.Binary(),
                        Path = c.String(),
                        DocumentType = c.Int(nullable: false),
                        Description = c.String(maxLength: 300),
                        CreatedDate = c.DateTime(nullable: false),
                        CompanyID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DocumentID)
                .ForeignKey("dbo.Company", t => t.CompanyID, cascadeDelete: true)
                .Index(t => t.CompanyID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Document", "CompanyID", "dbo.Company");
            DropForeignKey("dbo.Contact", "CompanyID", "dbo.Company");
            DropIndex("dbo.Document", new[] { "CompanyID" });
            DropIndex("dbo.Contact", new[] { "CompanyID" });
            DropTable("dbo.Document");
            DropTable("dbo.Contact");
            DropTable("dbo.Company");
        }
    }
}
