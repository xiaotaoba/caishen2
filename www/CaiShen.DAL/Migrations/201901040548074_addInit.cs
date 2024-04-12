namespace Pannet.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addInit : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.QuestionnaireRecordAnswer",
            //    c => new
            //        {
            //            ID = c.Int(nullable: false, identity: true),
            //            Quest_ID = c.Int(nullable: false),
            //            QuestionID = c.Int(nullable: false),
            //            QuestionnaireRecordID = c.Int(nullable: false),
            //            UserID = c.Int(nullable: false),
            //            Answer = c.String(maxLength: 300),
            //            AnswerIds = c.String(maxLength: 100),
            //        })
            //    .PrimaryKey(t => t.ID);
            
            //CreateTable(
            //    "dbo.QuestionnaireRecord",
            //    c => new
            //        {
            //            ID = c.Int(nullable: false, identity: true),
            //            Quest_ID = c.Int(nullable: false),
            //            UserID = c.Int(nullable: false),
            //            Score = c.Int(nullable: false),
            //            CreateTime = c.DateTime(nullable: false),
            //        })
            //    .PrimaryKey(t => t.ID);
            
            //AddColumn("dbo.AdvertisementRecord", "ADR_Address", c => c.String(maxLength: 100));
            //AddColumn("dbo.AdvertisementRecord", "ADR_Position", c => c.String(maxLength: 50));
            //AddColumn("dbo.AdvertisementRecord", "ADR_State", c => c.Int(nullable: false));
            //AddColumn("dbo.Advertisement", "AD_DepartmentID", c => c.Int(nullable: false));
            //AddColumn("dbo.Advertisement", "AD_IsSendWxMessage", c => c.Int(nullable: false));
            //AddColumn("dbo.Advertisement", "AD_WX_First", c => c.String(maxLength: 50));
            //AddColumn("dbo.Advertisement", "AD_WX_Remark", c => c.String(maxLength: 50));
            //AddColumn("dbo.Advertisement", "AD_WX_Address", c => c.String(maxLength: 50));
            //AddColumn("dbo.Advertisement", "AD_WX_Content", c => c.String(maxLength: 50));
            //AddColumn("dbo.Advertisement", "AD_WX_DepartmentID", c => c.Int(nullable: false));
            //AddColumn("dbo.Article", "Art_WX_Type", c => c.Int(nullable: false));
            //AddColumn("dbo.Article", "Art_WX_Openids", c => c.String(maxLength: 500));
            //AddColumn("dbo.Article", "Art_WX_DepartmentID", c => c.Int(nullable: false));
            //AddColumn("dbo.Department", "Dep_FollowID", c => c.Int(nullable: false));
            //AddColumn("dbo.DesignWork", "DW_IsRecommend", c => c.Int(nullable: false));
            //AddColumn("dbo.DesignWork", "DW_Sort", c => c.Int(nullable: false));
            //AddColumn("dbo.DesignWork", "DW_ShowTimes", c => c.Int(nullable: false));
            //AddColumn("dbo.DesignWork", "DW_TypeTags", c => c.String(maxLength: 200));
            //AddColumn("dbo.GoodsArticle", "GA_TimeLength", c => c.Int(nullable: false));
            AddColumn("dbo.GoodsCategory", "GC_Image", c => c.String(maxLength: 300));
            //AddColumn("dbo.GoodsCategory", "GC_Department", c => c.String(maxLength: 1000));
            //AddColumn("dbo.ManagerGroup", "Limits", c => c.String(maxLength: 1000));
            //AddColumn("dbo.ProfitPercentConfig", "Join_Platform_Profit_Percent", c => c.Double(nullable: false));
            //AddColumn("dbo.Questionnaire", "Quest_URL", c => c.String(maxLength: 200));
            //AddColumn("dbo.Questionnaire", "Quest_EndTime", c => c.DateTime());
            //AddColumn("dbo.Question", "Q_GroupItemSubID", c => c.Int(nullable: false));
            //AddColumn("dbo.User", "U_ShopName", c => c.String(maxLength: 50));
            //AddColumn("dbo.TestRecord", "TestID", c => c.Int(nullable: false));
            //AddColumn("dbo.Test", "T_GoodsArticleID", c => c.Int(nullable: false));
            //DropColumn("dbo.ProfitPercentConfig", "Join_Yinxia_Profit_Percent");
            //DropColumn("dbo.TestRecord", "TestResultID");
            //DropColumn("dbo.Test", "T_CurrentQuestionID");
        }
        
        public override void Down()
        {
            //AddColumn("dbo.Test", "T_CurrentQuestionID", c => c.Int(nullable: false));
            //AddColumn("dbo.TestRecord", "TestResultID", c => c.Int(nullable: false));
            //AddColumn("dbo.ProfitPercentConfig", "Join_Yinxia_Profit_Percent", c => c.Double(nullable: false));
            //DropColumn("dbo.Test", "T_GoodsArticleID");
            //DropColumn("dbo.TestRecord", "TestID");
            //DropColumn("dbo.User", "U_ShopName");
            //DropColumn("dbo.Question", "Q_GroupItemSubID");
            //DropColumn("dbo.Questionnaire", "Quest_EndTime");
            //DropColumn("dbo.Questionnaire", "Quest_URL");
            //DropColumn("dbo.ProfitPercentConfig", "Join_Platform_Profit_Percent");
            //DropColumn("dbo.ManagerGroup", "Limits");
            //DropColumn("dbo.GoodsCategory", "GC_Department");
            DropColumn("dbo.GoodsCategory", "GC_Image");
            //DropColumn("dbo.GoodsArticle", "GA_TimeLength");
            //DropColumn("dbo.DesignWork", "DW_TypeTags");
            //DropColumn("dbo.DesignWork", "DW_ShowTimes");
            //DropColumn("dbo.DesignWork", "DW_Sort");
            //DropColumn("dbo.DesignWork", "DW_IsRecommend");
            //DropColumn("dbo.Department", "Dep_FollowID");
            //DropColumn("dbo.Article", "Art_WX_DepartmentID");
            //DropColumn("dbo.Article", "Art_WX_Openids");
            //DropColumn("dbo.Article", "Art_WX_Type");
            //DropColumn("dbo.Advertisement", "AD_WX_DepartmentID");
            //DropColumn("dbo.Advertisement", "AD_WX_Content");
            //DropColumn("dbo.Advertisement", "AD_WX_Address");
            //DropColumn("dbo.Advertisement", "AD_WX_Remark");
            //DropColumn("dbo.Advertisement", "AD_WX_First");
            //DropColumn("dbo.Advertisement", "AD_IsSendWxMessage");
            //DropColumn("dbo.Advertisement", "AD_DepartmentID");
            //DropColumn("dbo.AdvertisementRecord", "ADR_State");
            //DropColumn("dbo.AdvertisementRecord", "ADR_Position");
            //DropColumn("dbo.AdvertisementRecord", "ADR_Address");
            //DropTable("dbo.QuestionnaireRecord");
            //DropTable("dbo.QuestionnaireRecordAnswer");
        }
    }
}
