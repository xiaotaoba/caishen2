using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Pannet.Models;

namespace Pannet.DAL
{
    public class UnitOfWork : IDisposable
    {
        private PannetContext context;// = new PannetContext();//EFContextFactory.GetCurrentDbContext();//

        //2018-01-17调整-One Context Per Request
        public UnitOfWork()
        {
            //this.context = HttpContext.Current.Items["_PannetContext"] as PannetContext;
            if (this.context == null)
            {
                this.context = new PannetContext();
                //HttpContext.Current.Items["_PannetContext"] = this.context;
            }
        }
        public PannetContext Context
        {
            get
            {
                //if (this.context == null)
                //{
                //    this.context = new PannetContext();// EFContextFactory.GetCurrentDbContext();
                //}
                //return this.context;
                //2018-1-11静态引用Context每次都新开——>导致严重错误
                //PannetContext myContext = new PannetContext();
                //return myContext;
                //if (this.context.Database.Connection.State == System.Data.ConnectionState.Closed)
                //{
                //    this.context.Database.Connection.Open();
                //}
                return this.context;
            }
        }

        #region 会员 + 店铺相关

        //会员相关
        private GenericRepository<User> userRepository;
        private GenericRepository<UserRole> userRoleRepository;
        private GenericRepository<UserPayInfo> userPayInfoRepository;
        private GenericRepository<UserAddress> userAddressRepository;
        private GenericRepository<UserAmountHistory> userAmountHistoryRepository;
        private GenericRepository<UserLevel> userLevelRepository;
        private GenericRepository<UserLevelHistory> userLevelHistoryRepository;
        private GenericRepository<UserRoleHistory> userRoleHistoryRepository;
        private GenericRepository<UserScoreHistory> userScoreHistoryRepository;
        private GenericRepository<UserShop> userShopRepository;
        private GenericRepository<UserShopGoods> userShopGoodsRepository;
        private GenericRepository<UserShopGoodsDown> userShopGoodsDownRepository;
        private GenericRepository<UserShopGoodsCategory> userShopGoodsCategoryRepository;
        private GenericRepository<ShopGuaranteeHistory> shopGuaranteeHistoryRepository;
        private GenericRepository<UserLog> userLogRepository;
        private GenericRepository<UserFile> userFileRepository;
        private GenericRepository<UserShopGoodsSet> userShopGoodsSetRepository;
        private GenericRepository<BrowseRecord> browseRecordRepository;
        private GenericRepository<TiXian> tiXianRepository;
        private GenericRepository<SignIn> signInRepository;
        private GenericRepository<Department> departmentRepository;

        #endregion

        #region 管理员相关

        //管理员相关
        private GenericRepository<Manager> managerRepository;
        private GenericRepository<ManagerWithGroup> managerWithGroupRepository;
        private GenericRepository<ManagerGroup> managergroupRepository;
        private GenericRepository<ManagerLog> managerLogRepository;

        #endregion

        #region 网站内容 + 配置 + 咨询留言 + 按需定制

        //区域
        private GenericRepository<SystemInfo> systemInfoRepository;
        private GenericRepository<AlipayConfig> alipayConfigRepository;
        private GenericRepository<WeixinConfig> weixinConfigRepository;
        private GenericRepository<Area> areaRepository;
        private GenericRepository<Article> articleRepository;
        private GenericRepository<ArticleType> articleTypeRepository;
        private GenericRepository<Advertisement> advertisementRepository;
        private GenericRepository<AdvertisementType> advertisementTypeRepository;
        private GenericRepository<Navigation> navigationRepository;
        private GenericRepository<ConsultMessage> consultMessageRepository;
        private GenericRepository<OrderCustomMessage> orderCustomMessageRepository;

        #endregion

        #region 产品相关

        //产品相关
        private GenericRepository<Goods> goodsRepository;
        private GenericRepository<GoodsPhoto> goodsPhotoRepository;
        private GenericRepository<GoodsPropertyValue> goodsPropertyValueRepository;
        private GenericRepository<GoodsSKU> goodsSKURepository;
        private GenericRepository<GoodsSKUPrice> goodsSKUPriceRepository;
        private GenericRepository<GoodsType> goodsTypeRepository;
        private GenericRepository<GoodsCategory> goodsCategoryRepository;
        private GenericRepository<GoodsPriceArea> goodsPriceAreaRepository;
        private GenericRepository<Property> propertyRepository;
        private GenericRepository<PropertyValue> propertyValueRepository;
        private GenericRepository<Brand> brandRepository;
        private GenericRepository<Supplier> supplierRepository;
        private GenericRepository<SupplierArea> supplierAreaRepository;
        private GenericRepository<SupplierGoods> supplierGoodsRepository;
        private GenericRepository<SupplierGoodsCategory> supplierGoodsCategoryRepository;
        private GenericRepository<Warehouse> warehouseRepository;
        private GenericRepository<WarehouseArea> warehouseAreaRepository;
        private GenericRepository<WarehouseGoods> warehouseGoodsRepository;
        private GenericRepository<Favorite> favoriteRepository;

        #endregion

        #region 订单相关 + 购物车 + 发票

        private GenericRepository<Order> orderRepository;
        private GenericRepository<OrderComment> orderCommentRepository;
        private GenericRepository<OrderDetail> orderDetailRepository;
        private GenericRepository<OrderDetailLog> orderDetailLogRepository;
        private GenericRepository<OrderShipping> orderShippingRepository;
        private GenericRepository<ReturnOrder> returnOrderRepository;
        private GenericRepository<ReturnOrderShipping> returnOrderShippingRepository;
        private GenericRepository<ShippingArea> shippingAreaRepository;
        private GenericRepository<ShippingCompany> shippingCompanyRepository;
        private GenericRepository<ShippingTemplate> shippingTemplateRepository;
        private GenericRepository<Cart> cartRepository;
        private GenericRepository<OrderFeeList> orderFeeListRepository;
        private GenericRepository<OrderChangeLog> orderChangeLogRepository;
        private GenericRepository<InvoiceLog> invoiceLogRepository;
        private GenericRepository<UserInvoiceAmount> userInvoiceAmountRepository;

        #endregion

        #region 积分商城 + 优惠券 + 红包

        private GenericRepository<ScoreProduct> scoreProductRepository;
        private GenericRepository<UserScoreProduct> userScoreProductRepository;
        private GenericRepository<CouponInfo> couponInfoRepository;
        private GenericRepository<UserCoupon> userCouponRepository;
        private GenericRepository<HongBao> hongBaoRepository;
        private GenericRepository<UserHongBao> userHongBaoRepository;

        #endregion

        #region 课程+测试+问卷 + 设计作品

        private GenericRepository<Question> questionRepository;
        private GenericRepository<Questionnaire> questionnaireRepository;
        private GenericRepository<QuestionnaireRecord> questionnaireRecordRepository;
        private GenericRepository<QuestionnaireRecordAnswer> questionnaireRecordAnswerRepository;
        private GenericRepository<Answer> answerRepository;
        private GenericRepository<Test> testRepository;
        private GenericRepository<TestRecord> testRecordRepository;
        private GenericRepository<AdvertisementRecord> advertisementRecordRepository;
        private GenericRepository<GoodsArticle> goodsArticleRepository;
        private GenericRepository<GoodsArticleRecord> goodsArticleRecordRepository;
        private GenericRepository<DesignWork> designWorkRepository;
        private GenericRepository<DesignWorkPhoto> designWorkPhotoRepository;
        private GenericRepository<Tag> tagRepository;
        private GenericRepository<Comment> commentRepository;

        #endregion

        /********************************************上面是声明部分，下面是方法主体***************************************/

        #region 会员 + 店铺相关

        public GenericRepository<User> UserRepository
        {
            get
            {
                if (this.userRepository == null)
                {
                    this.userRepository = new GenericRepository<User>(context);
                }
                return userRepository;
            }
        }
        public GenericRepository<UserRole> UserRoleRepository
        {
            get
            {
                if (this.userRoleRepository == null)
                {
                    this.userRoleRepository = new GenericRepository<UserRole>(context);
                }
                return userRoleRepository;
            }
        }
        public GenericRepository<UserPayInfo> PayInfoRepository
        {
            get
            {
                if (this.userPayInfoRepository == null)
                {
                    this.userPayInfoRepository = new GenericRepository<UserPayInfo>(context);
                }
                return userPayInfoRepository;
            }
        }
        public GenericRepository<UserAddress> UserAddressRepository
        {
            get
            {
                if (this.userAddressRepository == null)
                {
                    this.userAddressRepository = new GenericRepository<UserAddress>(context);
                }
                return userAddressRepository;
            }
        }
        public GenericRepository<UserAmountHistory> UserAmountHistoryRepository
        {
            get
            {
                if (this.userAmountHistoryRepository == null)
                {
                    this.userAmountHistoryRepository = new GenericRepository<UserAmountHistory>(context);
                }
                return userAmountHistoryRepository;
            }
        }
        public GenericRepository<UserLevel> UserLevelRepository
        {
            get
            {
                if (this.userLevelRepository == null)
                {
                    this.userLevelRepository = new GenericRepository<UserLevel>(context);
                }
                return userLevelRepository;
            }
        }
        public GenericRepository<UserLevelHistory> UserLevelHistoryRepository
        {
            get
            {
                if (this.userLevelHistoryRepository == null)
                {
                    this.userLevelHistoryRepository = new GenericRepository<UserLevelHistory>(context);
                }
                return userLevelHistoryRepository;
            }
        }
        public GenericRepository<UserRoleHistory> UserRoleHistoryRepository
        {
            get
            {
                if (this.userRoleHistoryRepository == null)
                {
                    this.userRoleHistoryRepository = new GenericRepository<UserRoleHistory>(context);
                }
                return userRoleHistoryRepository;
            }
        }
        public GenericRepository<UserScoreHistory> UserScoreHistoryRepository
        {
            get
            {
                if (this.userScoreHistoryRepository == null)
                {
                    this.userScoreHistoryRepository = new GenericRepository<UserScoreHistory>(context);
                }
                return userScoreHistoryRepository;
            }
        }
        public GenericRepository<UserShop> UserShopRepository
        {
            get
            {
                if (this.userShopRepository == null)
                {
                    this.userShopRepository = new GenericRepository<UserShop>(context);
                }
                return userShopRepository;
            }
        }
        public GenericRepository<UserShopGoods> UserShopGoodsRepository
        {
            get
            {
                if (this.userShopGoodsRepository == null)
                {
                    this.userShopGoodsRepository = new GenericRepository<UserShopGoods>(context);
                }
                return userShopGoodsRepository;
            }
        }
        public GenericRepository<UserShopGoodsDown> UserShopGoodsDownRepository
        {
            get
            {
                if (this.userShopGoodsDownRepository == null)
                {
                    this.userShopGoodsDownRepository = new GenericRepository<UserShopGoodsDown>(context);
                }
                return userShopGoodsDownRepository;
            }
        }
        public GenericRepository<UserShopGoodsCategory> UserShopGoodsCategoryRepository
        {
            get
            {
                if (this.userShopGoodsCategoryRepository == null)
                {
                    this.userShopGoodsCategoryRepository = new GenericRepository<UserShopGoodsCategory>(context);
                }
                return userShopGoodsCategoryRepository;
            }
        }
        public GenericRepository<ShopGuaranteeHistory> ShopGuaranteeHistoryRepository
        {
            get
            {
                if (this.shopGuaranteeHistoryRepository == null)
                {
                    this.shopGuaranteeHistoryRepository = new GenericRepository<ShopGuaranteeHistory>(context);
                }
                return shopGuaranteeHistoryRepository;
            }
        }
        public GenericRepository<UserLog> UserLogRepository
        {
            get
            {
                if (this.userLogRepository == null)
                {
                    this.userLogRepository = new GenericRepository<UserLog>(context);
                }
                return userLogRepository;
            }
        }
        public GenericRepository<UserFile> UserFileRepository
        {
            get
            {
                if (this.userFileRepository == null)
                {
                    this.userFileRepository = new GenericRepository<UserFile>(context);
                }
                return userFileRepository;
            }
        }
        public GenericRepository<UserShopGoodsSet> UserShopGoodsSetRepository
        {
            get
            {
                if (this.userShopGoodsSetRepository == null)
                {
                    this.userShopGoodsSetRepository = new GenericRepository<UserShopGoodsSet>(context);
                }
                return userShopGoodsSetRepository;
            }
        }
        public GenericRepository<BrowseRecord> BrowseRecordRepository
        {
            get
            {
                if (this.browseRecordRepository == null)
                {
                    this.browseRecordRepository = new GenericRepository<BrowseRecord>(context);
                }
                return browseRecordRepository;
            }
        }
        public GenericRepository<TiXian> TiXianRepository
        {
            get
            {
                if (this.tiXianRepository == null)
                {
                    this.tiXianRepository = new GenericRepository<TiXian>(context);
                }
                return tiXianRepository;
            }
        }
        public GenericRepository<SignIn> SignInRepository
        {
            get
            {
                if (this.signInRepository == null)
                {
                    this.signInRepository = new GenericRepository<SignIn>(context);
                }
                return signInRepository;
            }
        }
        public GenericRepository<Department> DepartmentRepository
        {
            get
            {
                if (this.departmentRepository == null)
                {
                    this.departmentRepository = new GenericRepository<Department>(context);
                }
                return departmentRepository;
            }
        }
        #endregion

        #region 管理员+组

        public GenericRepository<Manager> ManagerRepository
        {
            get
            {
                if (this.managerRepository == null)
                {
                    this.managerRepository = new GenericRepository<Manager>(context);
                }
                return managerRepository;
            }
        }
        public GenericRepository<ManagerWithGroup> ManagerWithGroupRepository
        {
            get
            {
                if (this.managerWithGroupRepository == null)
                {
                    this.managerWithGroupRepository = new GenericRepository<ManagerWithGroup>(context);
                }
                return managerWithGroupRepository;
            }
        }
        public GenericRepository<ManagerGroup> ManagerGroupRepository
        {
            get
            {
                if (this.managergroupRepository == null)
                {
                    this.managergroupRepository = new GenericRepository<ManagerGroup>(context);
                }
                return managergroupRepository;
            }
        }

        public GenericRepository<ManagerLog> ManagerLogRepository
        {
            get
            {
                if (this.managerLogRepository == null)
                {
                    this.managerLogRepository = new GenericRepository<ManagerLog>(context);
                }
                return managerLogRepository;
            }
        }

        #endregion

        #region 网站内容 + 配置 + 咨询留言 + 按需定制

        /// <summary>
        /// 系统设置信息
        /// </summary>
        public GenericRepository<SystemInfo> SystemInfoRepository
        {
            get
            {
                if (this.systemInfoRepository == null)
                {
                    this.systemInfoRepository = new GenericRepository<SystemInfo>(context);
                }
                return systemInfoRepository;
            }
        }
        /// <summary>
        /// 支付宝
        /// </summary>
        public GenericRepository<AlipayConfig> AlipayConfigRepository
        {
            get
            {
                if (this.alipayConfigRepository == null)
                {
                    this.alipayConfigRepository = new GenericRepository<AlipayConfig>(context);
                }
                return alipayConfigRepository;
            }
        }
        /// <summary>
        /// 微信
        /// </summary>
        public GenericRepository<WeixinConfig> WeixinConfigRepository
        {
            get
            {
                if (this.weixinConfigRepository == null)
                {
                    this.weixinConfigRepository = new GenericRepository<WeixinConfig>(context);
                }
                return weixinConfigRepository;
            }
        }

        /// <summary>
        /// 地区
        /// </summary>
        public GenericRepository<Area> AreaRepository
        {
            get
            {
                if (this.areaRepository == null)
                {
                    this.areaRepository = new GenericRepository<Area>(context);
                }
                return areaRepository;
            }
        }
        public GenericRepository<Article> ArticleRepository
        {
            get
            {
                if (this.articleRepository == null)
                {
                    this.articleRepository = new GenericRepository<Article>(context);
                }
                return articleRepository;
            }
        }
        public GenericRepository<ArticleType> ArticleTypeRepository
        {
            get
            {
                if (this.articleTypeRepository == null)
                {
                    this.articleTypeRepository = new GenericRepository<ArticleType>(context);
                }
                return articleTypeRepository;
            }
        }
        public GenericRepository<Advertisement> AdvertisementRepository
        {
            get
            {
                if (this.advertisementRepository == null)
                {
                    this.advertisementRepository = new GenericRepository<Advertisement>(context);
                }
                return advertisementRepository;
            }
        }
        public GenericRepository<AdvertisementType> AdvertisementTypeRepository
        {
            get
            {
                if (this.advertisementTypeRepository == null)
                {
                    this.advertisementTypeRepository = new GenericRepository<AdvertisementType>(context);
                }
                return advertisementTypeRepository;
            }
        }
        public GenericRepository<Navigation> NavigationRepository
        {
            get
            {
                if (this.navigationRepository == null)
                {
                    this.navigationRepository = new GenericRepository<Navigation>(context);
                }
                return navigationRepository;
            }
        }

        /// <summary>
        /// 咨询产品留言
        /// </summary>
        public GenericRepository<ConsultMessage> ConsultMessageRepository
        {
            get
            {
                if (this.consultMessageRepository == null)
                {
                    this.consultMessageRepository = new GenericRepository<ConsultMessage>(context);
                }
                return consultMessageRepository;
            }
        }
        /// <summary>
        /// 按需定制
        /// </summary>
        public GenericRepository<OrderCustomMessage> OrderCustomMessageRepository
        {
            get
            {
                if (this.orderCustomMessageRepository == null)
                {
                    this.orderCustomMessageRepository = new GenericRepository<OrderCustomMessage>(context);
                }
                return orderCustomMessageRepository;
            }
        }

        #endregion

        #region 产品相关

        public GenericRepository<Goods> GoodsRepository
        {
            get
            {
                if (this.goodsRepository == null)
                {
                    this.goodsRepository = new GenericRepository<Goods>(context);
                }
                return goodsRepository;
            }
        }

        public GenericRepository<GoodsPhoto> GoodsPhotoRepository
        {
            get
            {
                if (this.goodsPhotoRepository == null)
                {
                    this.goodsPhotoRepository = new GenericRepository<GoodsPhoto>(context);
                }
                return goodsPhotoRepository;
            }
        }

        public GenericRepository<GoodsPropertyValue> GoodsPropertyValueRepository
        {
            get
            {
                if (this.goodsPropertyValueRepository == null)
                {
                    this.goodsPropertyValueRepository = new GenericRepository<GoodsPropertyValue>(context);
                }
                return goodsPropertyValueRepository;
            }
        }

        public GenericRepository<GoodsSKU> GoodsSKURepository
        {
            get
            {
                if (this.goodsSKURepository == null)
                {
                    this.goodsSKURepository = new GenericRepository<GoodsSKU>(context);
                }
                return goodsSKURepository;
            }
        }
        public GenericRepository<GoodsSKUPrice> GoodsSKUPriceRepository
        {
            get
            {
                if (this.goodsSKUPriceRepository == null)
                {
                    this.goodsSKUPriceRepository = new GenericRepository<GoodsSKUPrice>(context);
                }
                return goodsSKUPriceRepository;
            }
        }

        public GenericRepository<GoodsType> GoodsTypeRepository
        {
            get
            {
                if (this.goodsTypeRepository == null)
                {
                    this.goodsTypeRepository = new GenericRepository<GoodsType>(context);
                }
                return goodsTypeRepository;
            }
        }

        public GenericRepository<GoodsCategory> GoodsCategoryRepository
        {
            get
            {
                if (this.goodsCategoryRepository == null)
                {
                    this.goodsCategoryRepository = new GenericRepository<GoodsCategory>(context);
                }
                return goodsCategoryRepository;
            }
        }
        public GenericRepository<GoodsPriceArea> GoodsPriceAreaRepository
        {
            get
            {
                if (this.goodsPriceAreaRepository == null)
                {
                    this.goodsPriceAreaRepository = new GenericRepository<GoodsPriceArea>(context);
                }
                return goodsPriceAreaRepository;
            }
        }

        public GenericRepository<Property> PropertyRepository
        {
            get
            {
                if (this.propertyRepository == null)
                {
                    this.propertyRepository = new GenericRepository<Property>(context);
                }
                return propertyRepository;
            }
        }

        public GenericRepository<PropertyValue> PropertyValueRepository
        {
            get
            {
                if (this.propertyValueRepository == null)
                {
                    this.propertyValueRepository = new GenericRepository<PropertyValue>(context);
                }
                return propertyValueRepository;
            }
        }

        public GenericRepository<Brand> BrandRepository
        {
            get
            {
                if (this.brandRepository == null)
                {
                    this.brandRepository = new GenericRepository<Brand>(context);
                }
                return brandRepository;
            }
        }

        public GenericRepository<Supplier> SupplierRepository
        {
            get
            {
                if (this.supplierRepository == null)
                {
                    this.supplierRepository = new GenericRepository<Supplier>(context);
                }
                return supplierRepository;
            }
        }
        public GenericRepository<SupplierArea> SupplierAreaRepository
        {
            get
            {
                if (this.supplierAreaRepository == null)
                {
                    this.supplierAreaRepository = new GenericRepository<SupplierArea>(context);
                }
                return supplierAreaRepository;
            }
        }
        public GenericRepository<SupplierGoods> SupplierGoodsRepository
        {
            get
            {
                if (this.supplierGoodsRepository == null)
                {
                    this.supplierGoodsRepository = new GenericRepository<SupplierGoods>(context);
                }
                return supplierGoodsRepository;
            }
        }

        public GenericRepository<SupplierGoodsCategory> SupplierGoodsCategoryRepository
        {
            get
            {
                if (this.supplierGoodsCategoryRepository == null)
                {
                    this.supplierGoodsCategoryRepository = new GenericRepository<SupplierGoodsCategory>(context);
                }
                return supplierGoodsCategoryRepository;
            }
        }

        public GenericRepository<Warehouse> WarehouseRepository
        {
            get
            {
                if (this.warehouseRepository == null)
                {
                    this.warehouseRepository = new GenericRepository<Warehouse>(context);
                }
                return warehouseRepository;
            }
        }
        public GenericRepository<WarehouseArea> WarehouseAreaRepository
        {
            get
            {
                if (this.warehouseAreaRepository == null)
                {
                    this.warehouseAreaRepository = new GenericRepository<WarehouseArea>(context);
                }
                return warehouseAreaRepository;
            }
        }
        public GenericRepository<WarehouseGoods> WarehouseGoodsRepository
        {
            get
            {
                if (this.warehouseGoodsRepository == null)
                {
                    this.warehouseGoodsRepository = new GenericRepository<WarehouseGoods>(context);
                }
                return warehouseGoodsRepository;
            }
        }

        public GenericRepository<Favorite> FavoriteRepository
        {
            get
            {
                if (this.favoriteRepository == null)
                {
                    this.favoriteRepository = new GenericRepository<Favorite>(context);
                }
                return favoriteRepository;
            }
        }


        #endregion

        #region 订单相关 + 发票

        public GenericRepository<Order> OrderRepository
        {
            get
            {
                if (this.orderRepository == null)
                {
                    this.orderRepository = new GenericRepository<Order>(context);
                }
                return orderRepository;
            }
        }
        public GenericRepository<OrderComment> OrderCommentRepository
        {
            get
            {
                if (this.orderCommentRepository == null)
                {
                    this.orderCommentRepository = new GenericRepository<OrderComment>(context);
                }
                return orderCommentRepository;
            }
        }
        public GenericRepository<OrderDetail> OrderDetailRepository
        {
            get
            {
                if (this.orderDetailRepository == null)
                {
                    this.orderDetailRepository = new GenericRepository<OrderDetail>(context);
                }
                return orderDetailRepository;
            }
        }
        public GenericRepository<OrderDetailLog> OrderDetailLogRepository
        {
            get
            {
                if (this.orderDetailLogRepository == null)
                {
                    this.orderDetailLogRepository = new GenericRepository<OrderDetailLog>(context);
                }
                return orderDetailLogRepository;
            }
        }
        public GenericRepository<OrderShipping> OrderShippingRepository
        {
            get
            {
                if (this.orderShippingRepository == null)
                {
                    this.orderShippingRepository = new GenericRepository<OrderShipping>(context);
                }
                return orderShippingRepository;
            }
        }
        public GenericRepository<ReturnOrder> ReturnOrderRepository
        {
            get
            {
                if (this.returnOrderRepository == null)
                {
                    this.returnOrderRepository = new GenericRepository<ReturnOrder>(context);
                }
                return returnOrderRepository;
            }
        }
        public GenericRepository<ReturnOrderShipping> ReturnOrderShippingRepository
        {
            get
            {
                if (this.returnOrderShippingRepository == null)
                {
                    this.returnOrderShippingRepository = new GenericRepository<ReturnOrderShipping>(context);
                }
                return returnOrderShippingRepository;
            }
        }
        public GenericRepository<ShippingArea> ShippingAreaRepository
        {
            get
            {
                if (this.shippingAreaRepository == null)
                {
                    this.shippingAreaRepository = new GenericRepository<ShippingArea>(context);
                }
                return shippingAreaRepository;
            }
        }
        public GenericRepository<ShippingCompany> ShippingCompanyRepository
        {
            get
            {
                if (this.shippingCompanyRepository == null)
                {
                    this.shippingCompanyRepository = new GenericRepository<ShippingCompany>(context);
                }
                return shippingCompanyRepository;
            }
        }
        public GenericRepository<ShippingTemplate> ShippingTemplateRepository
        {
            get
            {
                if (this.shippingTemplateRepository == null)
                {
                    this.shippingTemplateRepository = new GenericRepository<ShippingTemplate>(context);
                }
                return shippingTemplateRepository;
            }
        }
        public GenericRepository<OrderFeeList> OrderFeeListRepository
        {
            get
            {
                if (this.orderFeeListRepository == null)
                {
                    this.orderFeeListRepository = new GenericRepository<OrderFeeList>(context);
                }
                return orderFeeListRepository;
            }
        }
        public GenericRepository<OrderChangeLog> OrderChangeLogRepository
        {
            get
            {
                if (this.orderChangeLogRepository == null)
                {
                    this.orderChangeLogRepository = new GenericRepository<OrderChangeLog>(context);
                }
                return orderChangeLogRepository;
            }
        }
        public GenericRepository<InvoiceLog> InvoiceLogRepository
        {
            get
            {
                if (this.invoiceLogRepository == null)
                {
                    this.invoiceLogRepository = new GenericRepository<InvoiceLog>(context);
                }
                return invoiceLogRepository;
            }
        }
        public GenericRepository<UserInvoiceAmount> UserInvoiceAmountRepository
        {
            get
            {
                if (this.userInvoiceAmountRepository == null)
                {
                    this.userInvoiceAmountRepository = new GenericRepository<UserInvoiceAmount>(context);
                }
                return userInvoiceAmountRepository;
            }
        }

        #endregion

        #region 购物车

        public GenericRepository<Cart> CartRepository
        {
            get
            {
                if (this.cartRepository == null)
                {
                    this.cartRepository = new GenericRepository<Cart>(context);
                }
                return cartRepository;
            }
        }

        #endregion

        #region 积分商城 + 优惠券 + 红包

        public GenericRepository<ScoreProduct> ScoreProductRepository
        {
            get
            {
                if (this.scoreProductRepository == null)
                {
                    this.scoreProductRepository = new GenericRepository<ScoreProduct>(context);
                }
                return scoreProductRepository;
            }
        }
        public GenericRepository<UserScoreProduct> UserScoreProductRepository
        {
            get
            {
                if (this.userScoreProductRepository == null)
                {
                    this.userScoreProductRepository = new GenericRepository<UserScoreProduct>(context);
                }
                return userScoreProductRepository;
            }
        }
        public GenericRepository<CouponInfo> CouponInfoRepository
        {
            get
            {
                if (this.couponInfoRepository == null)
                {
                    this.couponInfoRepository = new GenericRepository<CouponInfo>(context);
                }
                return couponInfoRepository;
            }
        }
        public GenericRepository<UserCoupon> UserCouponRepository
        {
            get
            {
                if (this.userCouponRepository == null)
                {
                    this.userCouponRepository = new GenericRepository<UserCoupon>(context);
                }
                return userCouponRepository;
            }
        }
        public GenericRepository<HongBao> HongBaoRepository
        {
            get
            {
                if (this.hongBaoRepository == null)
                {
                    this.hongBaoRepository = new GenericRepository<HongBao>(context);
                }
                return hongBaoRepository;
            }
        }
        public GenericRepository<UserHongBao> UserHongBaoRepository
        {
            get
            {
                if (this.userHongBaoRepository == null)
                {
                    this.userHongBaoRepository = new GenericRepository<UserHongBao>(context);
                }
                return userHongBaoRepository;
            }
        }


        #endregion

        #region 课程+测试+问卷 + 设计作品

        public GenericRepository<Question> QuestionRepository
        {
            get
            {
                if (this.questionRepository == null)
                {
                    this.questionRepository = new GenericRepository<Question>(context);
                }
                return this.questionRepository;
            }
        }
        public GenericRepository<Questionnaire> QuestionnaireRepository
        {
            get
            {
                if (this.questionnaireRepository == null)
                {
                    this.questionnaireRepository = new GenericRepository<Questionnaire>(context);
                }
                return this.questionnaireRepository;
            }
        }
        public GenericRepository<QuestionnaireRecord> QuestionnaireRecordRepository
        {
            get
            {
                if (this.questionnaireRecordRepository == null)
                {
                    this.questionnaireRecordRepository = new GenericRepository<QuestionnaireRecord>(context);
                }
                return this.questionnaireRecordRepository;
            }
        }
        public GenericRepository<QuestionnaireRecordAnswer> QuestionnaireRecordAnswerRepository
        {
            get
            {
                if (this.questionnaireRecordAnswerRepository == null)
                {
                    this.questionnaireRecordAnswerRepository = new GenericRepository<QuestionnaireRecordAnswer>(context);
                }
                return this.questionnaireRecordAnswerRepository;
            }
        }
        public GenericRepository<Answer> AnswerRepository
        {
            get
            {
                if (this.answerRepository == null)
                {
                    this.answerRepository = new GenericRepository<Answer>(context);
                }
                return this.answerRepository;
            }
        }
        public GenericRepository<Test> TestRepository
        {
            get
            {
                if (this.testRepository == null)
                {
                    this.testRepository = new GenericRepository<Test>(context);
                }
                return this.testRepository;
            }
        }
        public GenericRepository<TestRecord> TestRecordRepository
        {
            get
            {
                if (this.testRecordRepository == null)
                {
                    this.testRecordRepository = new GenericRepository<TestRecord>(context);
                }
                return this.testRecordRepository;
            }
        }
        public GenericRepository<AdvertisementRecord> AdvertisementRecordRepository
        {
            get
            {
                if (this.advertisementRecordRepository == null)
                {
                    this.advertisementRecordRepository = new GenericRepository<AdvertisementRecord>(context);
                }
                return this.advertisementRecordRepository;
            }
        }
        public GenericRepository<GoodsArticle> GoodsArticleRepository
        {
            get
            {
                if (this.goodsArticleRepository == null)
                {
                    this.goodsArticleRepository = new GenericRepository<GoodsArticle>(context);
                }
                return this.goodsArticleRepository;
            }
        }
        public GenericRepository<GoodsArticleRecord> GoodsArticleRecordRepository
        {
            get
            {
                if (this.goodsArticleRecordRepository == null)
                {
                    this.goodsArticleRecordRepository = new GenericRepository<GoodsArticleRecord>(context);
                }
                return this.goodsArticleRecordRepository;
            }
        }
        public GenericRepository<DesignWork> DesignWorkRepository
        {
            get
            {
                if (this.designWorkRepository == null)
                {
                    this.designWorkRepository = new GenericRepository<DesignWork>(context);
                }
                return this.designWorkRepository;
            }
        }
        public GenericRepository<DesignWorkPhoto> DesignWorkPhotoRepository
        {
            get
            {
                if (this.designWorkPhotoRepository == null)
                {
                    this.designWorkPhotoRepository = new GenericRepository<DesignWorkPhoto>(context);
                }
                return this.designWorkPhotoRepository;
            }
        }
        public GenericRepository<Tag> TagRepository
        {
            get
            {
                if (this.tagRepository == null)
                {
                    this.tagRepository = new GenericRepository<Tag>(context);
                }
                return this.tagRepository;
            }
        }
        public GenericRepository<Comment> CommentRepository
        {
            get
            {
                if (this.commentRepository == null)
                {
                    this.commentRepository = new GenericRepository<Comment>(context);
                }
                return this.commentRepository;
            }
        }
        #endregion

        #region 保存+ 释放

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;
        public void Dispose()
        {
            Dispose(true);

            //因为对象会被Dispose释放，所以需要调用GC.SuppressFinalize来让对象脱离终止队列，防止对象终止被执行两次
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        #endregion

    }

    #region Context工厂

    /// <summary>
    /// Context工厂
    /// </summary>
    public class EFContextFactory
    {
        public static PannetContext GetCurrentDbContext()
        {
            PannetContext dbContext = (PannetContext)CallContext.GetData("PannetContext");
            if (dbContext == null)
            {
                dbContext = new PannetContext();
                CallContext.SetData("PannetContext", dbContext);
            }
            return dbContext;
        }
    }

    #endregion
}
