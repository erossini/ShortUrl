namespace PSC.Shorturl.Web.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        IDCategory = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(nullable: false),
                        Parent = c.Int(),
                        Username = c.String(),
                    })
                .PrimaryKey(t => t.IDCategory);
            
            CreateTable(
                "dbo.IPCities",
                c => new
                    {
                        IPFrom = c.String(nullable: false, maxLength: 15),
                        IPTo = c.String(nullable: false, maxLength: 15),
                        CountryCode = c.String(nullable: false, maxLength: 2),
                        Region = c.String(maxLength: 500),
                        City = c.String(maxLength: 500),
                        IPFromNumber = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IPToNumber = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.IPFrom, t.IPTo, t.CountryCode });
            
            CreateTable(
                "dbo.short_urls",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        long_url = c.String(nullable: false, maxLength: 1000),
                        description = c.String(maxLength: 1000),
                        segment = c.String(nullable: false, maxLength: 20),
                        added = c.DateTime(nullable: false),
                        ip = c.String(nullable: false, maxLength: 50),
                        num_of_clicks = c.Int(nullable: false),
                        username = c.String(),
                        CategoryID_IDCategory = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Categories", t => t.CategoryID_IDCategory)
                .Index(t => t.CategoryID_IDCategory);
            
            CreateTable(
                "dbo.stats",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        click_date = c.DateTime(nullable: false),
                        IP = c.String(nullable: false, maxLength: 50),
                        referer = c.String(maxLength: 500),
                        userAgent = c.String(maxLength: 500),
                        BrowserName = c.String(maxLength: 150),
                        BrowserVersion = c.String(maxLength: 50),
                        BrowserLanguage = c.String(maxLength: 50),
                        ConnectionSpeed = c.String(maxLength: 50),
                        Platform = c.String(maxLength: 50),
                        PlatformLanguage = c.String(maxLength: 50),
                        IPCountry = c.String(maxLength: 150),
                        IPLocation = c.String(maxLength: 150),
                        ISCrawler = c.Boolean(nullable: false),
                        JScriptVersion = c.String(maxLength: 50),
                        IsMobile = c.Boolean(nullable: false),
                        MobileDeviceModel = c.String(maxLength: 50),
                        MobileDeviceManufacturer = c.String(maxLength: 50),
                        ScreenPixelsHeight = c.String(maxLength: 10),
                        ScreenPixelsWidth = c.String(maxLength: 10),
                        QueryString = c.String(),
                        Username = c.String(maxLength: 256),
                        UtmMedium = c.String(),
                        UtmSource = c.String(),
                        UtmCampaign = c.String(),
                        UtmContext = c.String(),
                        ShortUrl_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.short_urls", t => t.ShortUrl_Id, cascadeDelete: true)
                .Index(t => t.ShortUrl_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.stats", "ShortUrl_Id", "dbo.short_urls");
            DropForeignKey("dbo.short_urls", "CategoryID_IDCategory", "dbo.Categories");
            DropIndex("dbo.stats", new[] { "ShortUrl_Id" });
            DropIndex("dbo.short_urls", new[] { "CategoryID_IDCategory" });
            DropTable("dbo.stats");
            DropTable("dbo.short_urls");
            DropTable("dbo.IPCities");
            DropTable("dbo.Categories");
        }
    }
}
