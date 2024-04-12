using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;

namespace Pannet.DAL
{
    public class PannetContext : DbContext
    {
        public PannetContext()
            : base("PannetContext")
        {
            //this.Configuration.LazyLoadingEnabled = false; 
           //this.Configuration.LazyLoadingEnabled = false;
           this.Configuration.ProxyCreationEnabled = false;
        }

        #region 管理员 + 角色 + 日志

        public DbSet<Manager> Managers { get; set; }

        public DbSet<ManagerGroup> ManagerGroups { get; set; }

        public DbSet<ManagerWithGroup> ManagerWithGroups { get; set; }

        public DbSet<ManagerLog> ManagerLogs { get; set; }

        #endregion

        #region 网站管理 + 配置 + 地区 + 文章 +  广告 + 导航 + 咨询留言 + 按需定制

        public DbSet<SystemInfo> SystemInfos { get; set; }
        public DbSet<AlipayConfig> AlipayConfigs { get; set; }
        public DbSet<WeixinConfig> WeixinConfigs { get; set; }
        public DbSet<ProfitPercentConfig> ProfitPercentConfigs { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleType> ArticleTypes { get; set; }
        public DbSet<Advertisement> Advertisements { get; set; }
        public DbSet<AdvertisementType> AdvertisementTypes { get; set; }
        public DbSet<Navigation> Navigations { get; set; }
        public DbSet<ConsultMessage> ConsultMessages { get; set; }
        public DbSet<OrderCustomMessage> OrderCustomMessages { get; set; }

        #endregion

        #region 会员+等级+角色+地址+相关记录+门店+门店产品+提现+签到+部门

        public DbSet<User> Users { get; set; }
        public DbSet<UserLog> UserLogs { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }
        public DbSet<UserAmountHistory> UserAmountHistorys { get; set; }
        public DbSet<UserLevel> UserLevels { get; set; }
        public DbSet<UserLevelHistory> UserLevelHistorys { get; set; }
        public DbSet<UserPayInfo> UserPayInfos { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserRoleHistory> UserRoleHistorys { get; set; }
        public DbSet<UserScoreHistory> UserScoreHistorys { get; set; }
        public DbSet<UserShop> UserShops { get; set; }
        public DbSet<UserShopGoods> UserShopGoodses { get; set; }
        public DbSet<UserShopGoodsDown> UserShopGoodsDowns { get; set; }
        public DbSet<UserShopGoodsSet> UserShopGoodsSets { get; set; }
        public DbSet<UserShopGoodsCategory> UserShopGoodsCategorys { get; set; }
        public DbSet<ShopGuaranteeHistory> ShopGuaranteeHistorys { get; set; }
        public DbSet<UserFile> UserFiles { get; set; }
        public DbSet<BrowseRecord> BrowseRecords { get; set; }
        public DbSet<TiXian> TiXians { get; set; }
        public DbSet<SignIn> SignIns { get; set; }
        public DbSet<Department> Departments { get; set; }

        #endregion

        #region 发票

        public DbSet<InvoiceLog> InvoiceLogs { get; set; }
        public DbSet<UserInvoiceAmount> UserInvoiceAmounts { get; set; }

        #endregion

        #region 产品+属性+品牌+类型+供应商+仓库

        public DbSet<Goods> Goods { get; set; }
        public DbSet<GoodsPhoto> GoodsPhotos { get; set; }
        public DbSet<GoodsPropertyValue> GoodsPropertyValues { get; set; }
        public DbSet<GoodsSKU> GoodsSKUs { get; set; }
        public DbSet<GoodsSKUPrice> GoodsSKUPrices { get; set; }
        public DbSet<GoodsType> GoodsTypes { get; set; }
        public DbSet<GoodsCategory> GoodsCategorys { get; set; }
        public DbSet<GoodsPriceArea> GoodsPriceAreas { get; set; }

        public DbSet<Brand> Brands { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SupplierArea> SupplierAreas { get; set; }
        public DbSet<SupplierGoods> SupplierGoods { get; set; }
        public DbSet<SupplierGoodsCategory> SupplierGoodsCategorys { get; set; }

        public DbSet<Property> Property { get; set; }

        public DbSet<PropertyValue> PropertyValues { get; set; }

        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<WarehouseArea> WarehouseAreas { get; set; }
        public DbSet<WarehouseGoods> WarehouseGoods { get; set; }

        public DbSet<Favorite> Favorites { get; set; }

        #endregion

        #region 订单+订单详情+配送+退货+评价 + 订单编辑记录

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderDetailLog> OrderDetailLogs { get; set; }
        public DbSet<OrderShipping> OrderShippings { get; set; }
        public DbSet<OrderComment> OrderComment { get; set; }
        public DbSet<OrderFeeList> OrderFeeList { get; set; }
        public DbSet<OrderChangeLog> OrderChangeLogs { get; set; }

        public DbSet<ReturnOrder> ReturnOrders { get; set; }
        public DbSet<ReturnOrderShipping> ReturnOrderShippings { get; set; }

        public DbSet<ShippingArea> ShippingAreas { get; set; }
        public DbSet<ShippingCompany> ShippingCompanys { get; set; }
        public DbSet<ShippingTemplate> ShippingTemplates { get; set; }

        #endregion

        #region 购物车

        public DbSet<Cart> Carts { get; set; }

        #endregion

        #region 积分商品

        public DbSet<ScoreProduct> ScoreProducts { get; set; }
        public DbSet<UserScoreProduct> UserScoreProducts { get; set; }

        #endregion

        #region 优惠券 + 红包

        public DbSet<CouponInfo> CouponInfos { get; set; }
        public DbSet<UserCoupon> UserCoupons { get; set; }
        public DbSet<HongBao> HongBaos { get; set; }
        public DbSet<UserHongBao> UserHongBaos { get; set; }

        #endregion

        #region 课程+测试+问卷

        /// <summary>
        /// 问题
        /// </summary>
        public DbSet<Question> Questions { get; set; }
        /// <summary>
        /// 问卷调查
        /// </summary>
        public DbSet<Questionnaire> Questionnaires { get; set; }
        public DbSet<QuestionnaireRecord> QuestionnaireRecords { get; set; }
        public DbSet<QuestionnaireRecordAnswer> QuestionnaireRecordAnswers { get; set; }
        public DbSet<Answer> Answer { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<TestRecord> TestRecords { get; set; }
        /// <summary>
        /// 活动报名记录
        /// </summary>
        public DbSet<AdvertisementRecord> AdvertisementRecords { get; set; }
        /// <summary>
        /// 课程（视频）目录
        /// </summary>
        public DbSet<GoodsArticle> GoodsArticles { get; set; }
        /// <summary>
        /// 课程学习记录
        /// </summary>
        public DbSet<GoodsArticleRecord> GoodsArticleRecords { get; set; }

        /// <summary>
        /// 作品
        /// </summary>
        public DbSet<DesignWork> DesignWorks { get; set; }
        /// <summary>
        /// 作品相册
        /// </summary>
        public DbSet<DesignWorkPhoto> DesignWorkPhotos { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public DbSet<Tag> Tags { get; set; }
        /// <summary>
        /// 评价
        /// </summary>
        public DbSet<Comment> Comments { get; set; }
        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
         
        }
    }
}
