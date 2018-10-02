namespace Blockchaninaccounts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SecretPhase : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "SecretPhase", c => c.String());
            AddColumn("dbo.AspNetUsers", "CreatedDateTime", c => c.DateTime());
            AddColumn("dbo.AspNetUsers", "GoogleAuthetication", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "UpdatedDateTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "SecretPhase");
            DropColumn("dbo.AspNetUsers", "CreatedDateTime");
            DropColumn("dbo.AspNetUsers", "GoogleAuthetication");
            DropColumn("dbo.AspNetUsers", "UpdatedDateTime");
        }
    }
}
