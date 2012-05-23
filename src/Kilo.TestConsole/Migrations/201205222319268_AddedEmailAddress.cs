namespace Kilo.TestConsole.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddedEmailAddress : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "EmailAddresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.String(nullable: false, maxLength: 250),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("Users", "EmailAddress_Id", c => c.Int());
            AddForeignKey("Users", "EmailAddress_Id", "EmailAddresses", "Id");
            CreateIndex("Users", "EmailAddress_Id");
        }
        
        public override void Down()
        {
            DropIndex("Users", new[] { "EmailAddress_Id" });
            DropForeignKey("Users", "EmailAddress_Id", "EmailAddresses");
            DropColumn("Users", "EmailAddress_Id");
            DropTable("EmailAddresses");
        }
    }
}
