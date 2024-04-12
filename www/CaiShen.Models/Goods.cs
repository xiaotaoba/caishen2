using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    public class Goods
    {
        public Goods()
        {
            this.G_MarketPrice = 0;
            this.G_Price = 0;
            this.G_IsFixedPrice = 0;
            this.G_IsZiti = 0;
            this.G_Volume = 0;
            this.G_Weight = 0;
            this.G_Count = 0;
            this.G_IsRecommend = 0;
            this.G_IsNew = 0;
            this.G_IsHot = 0;
            this.G_Status = 1;
            this.G_CreateTime = DateTime.Now;
            this.G_MakeDays = 0;
            this.G_IsFreeShipping = 0;
            this.G_ShippingTemplateID = 0;
            this.UserShopID = 0;
            this.G_IsPC = 0;
            this.G_DesignFee = 0;
            this.G_IsWeight = 0;
            this.G_Sort = 0;
            this.G_IsMobile = 1;
            this.G_UnitCount = 1;
            this.G_IsDesign = 0;
        }
        public int ID { get; set; }

        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "{0}由{2}-{1}个字符组成")]
        [Display(Name = "商品名称")]
        public string G_Name { get; set; }

        /// <summary>
        /// 商品简介或推荐语
        /// </summary>
        [StringLength(100, MinimumLength = 2, ErrorMessage = "{0}由{2}-{1}个字符组成")]
        [Display(Name = "商品简介")]
        public string G_Abstract { get; set; }

        /// <summary>
        /// 商品编号
        /// </summary>
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50)]
        [Display(Name = "商品编号")]
        public string G_Number { get; set; }

        /// <summary>
        /// 商品描述
        /// </summary>
        [Display(Name = "商品描述")]
        public string G_Desc { get; set; }

        /// <summary>
        /// 商品移动端描述
        /// </summary>
        [Display(Name = "商品移动端描述")]
        public string G_MobileDesc { get; set; }

        /// <summary>
        /// 市场价
        /// </summary>
        [Display(Name = "市场价")]
        public decimal G_MarketPrice { get; set; }

        /// <summary>
        /// 商品价格
        /// </summary>
        [Display(Name = "商品价格")]
        public decimal G_Price { get; set; }

        /// <summary>
        /// 一口价
        /// </summary>
        [Display(Name = "一口价")]
        public int G_IsFixedPrice { get; set; }

        /// <summary>
        /// 默认0:都显示，1仅pc显示，2仅移动显示
        /// </summary>
        [Display(Name = "PC端显示")]
        public int G_IsPC { get; set; }

        /// <summary>
        /// 是否现货
        /// </summary>
        [Display(Name = "是否现货")]
        public int G_IsExist { get; set; }

        /// <summary>
        /// 非现货设计费用，默认0
        /// </summary>
        [Display(Name = "设计费用")]
        public decimal G_DesignFee { get; set; }

        /// <summary>
        /// 制作周期(天数)
        /// </summary>
        [Display(Name = "制作周期")]
        public int G_MakeDays { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        [Display(Name = "单位")]
        [StringLength(10)]
        public string G_Unit { get; set; }

        /// <summary>
        /// 单位个数，如1件100个，默认1
        /// </summary>
        [Display(Name = "每件个数")]
        public int G_UnitCount { get; set; }

        /// <summary>
        /// 单位说明
        /// </summary>
        [Display(Name = "单位说明")]
        [StringLength(20)]
        public string G_UnitInfo { get; set; }

        /// <summary>
        /// 支持自提
        /// </summary>
        [Display(Name = "支持自提")]
        public int G_IsZiti { get; set; }

        /// <summary>
        ///体积(m³)
        /// </summary>
        [Display(Name = "体积(m³)")]
        public double G_Volume { get; set; }

        /// <summary>
        /// 重量(kg)
        /// </summary>
        [Display(Name = "重量(kg)")]
        public double G_Weight { get; set; }

        /// <summary>
        ///展开面积(m²)
        /// </summary>
        [Display(Name = "展开面积(m²)")]
        public double G_ExpandArea { get; set; }

        /// <summary>
        /// 平方克重，1平方米商品的重量
        /// </summary>
        [Display(Name = "平方克重(g)")]
        public double G_SquareWeight { get; set; }

        /// <summary>
        /// 库存
        /// </summary>
        [Display(Name = "库存")]
        public int G_Count { get; set; }

        /// <summary>
        /// 1上架 0下架
        /// </summary>
        [Display(Name = "上架状态")]
        public int G_Status { get; set; }

        /// <summary>
        /// 推荐
        /// </summary>
        [Display(Name = "推荐")]
        public int G_IsRecommend { get; set; }

        /// <summary>
        /// 新品
        /// </summary>
        [Display(Name = "新品")]
        public int G_IsNew { get; set; }

        /// <summary>
        /// 热卖
        /// </summary>
        [Display(Name = "热卖")]
        public int G_IsHot { get; set; }

        /// <summary>
        /// 是否免邮费（包邮）
        /// </summary>
        [Display(Name = "包邮")]
        public int G_IsFreeShipping { get; set; }

        /// <summary>
        /// 运费模板
        /// </summary>
        [Display(Name = "运费模板")]
        public int G_ShippingTemplateID { get; set; }

        [StringLength(200)]
        [Display(Name = "展示图片")]
        public string G_Image { get; set; }

        /// <summary>
        /// 手机展示图片
        /// </summary>
        [StringLength(200)]
        [Display(Name = "手机展示图片")]
        public string G_MobileImage { get; set; }

        //品牌
        [Display(Name = "品牌")]
        public int BrandID { get; set; }
        public virtual Brand Brand { get; set; }

        /// <summary>
        /// 商品类型
        /// </summary>
        [Display(Name = "商品类型")]
        public int GoodsTypeID { get; set; }

        /// <summary>
        /// 商品分类
        /// </summary>
        [Display(Name = "商品分类")]
        public int GoodsCategoryID { get; set; }

        [Display(Name = "排序号")]
        public int G_Sort { get; set; }

        /// <summary>
        /// 默认供应商
        /// </summary>
        [Display(Name = "默认供应商")]
        public int SupplierID { get; set; }
        //public virtual Supplier Supplier { get; set; }

        /// <summary>
        /// 默认仓库
        /// </summary>
        [Display(Name = "默认仓库")]
        public int WarehouseID { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        public DateTime G_CreateTime { get; set; }

        /// <summary>
        /// 浏览次数
        /// </summary>
        [Display(Name = "浏览次数")]
        public int G_ShowTimes { get; set; }

        /// <summary>
        /// 销售数量
        /// </summary>
        [Display(Name = "销售数量")]
        public int G_SaleCount { get; set; }

        /// <summary>
        /// 默认0:平台，是否为门店自定义商品
        /// </summary>
        [Display(Name = "门店")]
        public int UserShopID { get; set; }

        /// <summary>
        /// 视频链接
        /// </summary>
        [Display(Name = "视频链接")]
        [StringLength(200)]
        public string G_OrderProductUrl { get; set; }

        /// <summary>
        /// 加印LOGO商品链接
        /// </summary>
        [Display(Name = "加印LOGO商品链接")]
        [StringLength(200)]
        public string G_PrintLogoProductUrl { get; set; }

        /// <summary>
        /// 重货/泡货,默认1重货，0泡货
        /// </summary>
        [Display(Name = "重货/泡货")]
        public int G_IsWeight { get; set; }

        /// <summary>
        /// 手机展示
        /// </summary>
        [Display(Name = "手机展示")]
        public int G_IsMobile { get; set; }

        /// <summary>
        /// 手机推荐
        /// </summary>
        [Display(Name = "手机推荐")]
        public int G_IsRecommendMobile { get; set; }

        /// <summary>
        /// 手机排序号
        /// </summary>
        [Display(Name = "手机排序号")]
        public int G_SortMobile { get; set; }
        /// <summary>
        /// 商品标签，以逗号隔开的字符，如:,标签1,标签2,标签2,
        /// </summary>
        [StringLength(200)]
        [Display(Name = "商品标签")]
        public string G_Tags { get; set; }
        /// <summary>
        /// 是否设计类商品
        /// </summary>
        [Display(Name = "设计类商品")]
        public int G_IsDesign { get; set; }
        /// <summary>
        /// 作者/发布者
        /// </summary>
        [StringLength(50)]
        [Display(Name = "发布者")]
        public string G_Author { get; set; }
        /// <summary>
        /// 康复-服务时长
        /// </summary>
        [StringLength(50)]
        [Display(Name = "服务时长")]
        public string G_ServiceTime { get; set; }
    }

    public class ShopGoodsVModel
    {
        public ShopGoodsVModel()
        {
            this.UG_IsRecommend = 0;
            this.UG_Sort = 0;
            this.UG_Discount = 1;
        }
        public Goods Goods { get; set; }
        public string GoodsCategoryName { get; set; }


        /// <summary>
        ///价格折扣（相对终端价），取值0.1即1折
        /// </summary>
        [Display(Name = "价格折扣")]
        public double UG_Discount { get; set; }
        /// <summary>
        /// 推荐
        /// </summary>
        [Display(Name = "推荐")]
        public int UG_IsRecommend { get; set; }

        /// <summary>
        /// 手机推荐
        /// </summary>
        [Display(Name = "手机推荐")]
        public int UG_IsRecommendMobile { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        [Display(Name = "排序号")]
        public int UG_Sort { get; set; }

        /// <summary>
        /// 手机排序号
        /// </summary>
        [Display(Name = "手机排序号")]
        public int UG_SortMobile { get; set; }

        ///<summary>
        ///展示图片
        ///</summary>
        [StringLength(200)]
        [Display(Name = "展示图片")]
        public string UG_Image { get; set; }

        /// <summary>
        /// 手机展示图片
        /// </summary>
        [StringLength(200)]
        [Display(Name = "手机展示图片")]
        public string UG_MobileImage { get; set; }
    }

    /// <summary>
    /// 返回到前台商品价格结果实体
    /// </summary>
    public class GoodsPriceResult
    {
        /// <summary>
        /// json获取SKUPrice价格结果状态,success或error
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 结果提示内容
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 购买数量
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// count件终端价格（如果是加盟商，显示加盟商价格）
        /// </summary>
        public decimal price { get; set; }
        /// <summary>
        /// count件加盟商价格
        /// </summary>
        public decimal shopprice { get; set; }
        /// <summary>
        /// count件成本价格
        /// </summary>
        public decimal costprice { get; set; }
        /// <summary>
        /// count件运费
        /// </summary>
        public decimal freight { get; set; }
        /// <summary>
        /// 单件体积
        /// </summary>
        public double volume { get; set; }
        /// <summary>
        /// 单件重量
        /// </summary>
        public double weight { get; set; }
        /// <summary>
        /// 单个面积
        /// </summary>
        public double unitarea { get; set; }
    }

    /// <summary>
    /// 单件商品的重量体积实体
    /// </summary>
    public class GoodsWeightVolume
    {
        /// <summary>
        /// 单件体积
        /// </summary>
        public double UnitVolume { get; set; }
        /// <summary>
        /// 单件重量
        /// </summary>
        public double UnitWeight { get; set; }
    }

    public class GoodsTongJiVModel
    {
        public GoodsTongJiVModel()
        {
        }
        public GoodsArticle GoodsArticle { get; set; }
        public Goods Goods { get; set; }
        public IEnumerable<Test> Tests { get; set; }
        public int TestCount { get; set; }
        public int TestPersonCount { get; set; }
        /// <summary>
        /// 合格数
        /// </summary>
        public int TestHegeCount { get; set; }
        public int ViewCount { get; set; }
        public int ViewPersonCount { get; set; }
    }
}
