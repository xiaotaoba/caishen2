namespace Pannet.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addFieldOfGoods3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Goods", "G_ServiceTime", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Goods", "G_ServiceTime");
        }
    }
}
