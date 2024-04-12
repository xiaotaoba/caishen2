using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Utility;
using Pannet.Models;

namespace Pannet.DAL
{
    public class DataConfig
    {
        #region 公用

        /// <summary>
        /// 是/否
        /// </summary>
        public static List<NameValueModel> YesOrNo
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "否", Value = "0" });
                list.Add(new NameValueModel { Name = "是", Value = "1" });
                return list;
            }
        }
        /// <summary>
        /// 是否启用Enum
        /// </summary>
        public enum YesOrNoEnum
        {
            否 = 0,
            是 = 1,
        }

        /// <summary>
        /// 是否启用
        /// </summary>
        public static List<NameValueModel> IsEnableValues
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "未启用", Value = "0" });
                list.Add(new NameValueModel { Name = "启用", Value = "1" });
                return list;
            }
        }
        /// <summary>
        /// 是否启用Enum
        /// </summary>
        public enum IsEnableEnum
        {
            未启用 = 0,
            启用 = 1,
        }

        /// <summary>
        /// 是否推荐
        /// </summary>
        public static List<NameValueModel> IsRecommendValues
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "不推荐", Value = "0" });
                list.Add(new NameValueModel { Name = "推荐", Value = "1" });
                return list;
            }
        }
        /// <summary>
        /// 是否推荐Enum
        /// </summary>
        public enum IsRecommendEnum
        {
            不推荐 = 0,
            推荐 = 1,
        }


        #endregion

        #region 会员 +　权限

        /// <summary>
        /// 是否启用
        /// </summary>
        public static List<NameValueModel> LevelIsEnable
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "未启用", Value = "0" });
                list.Add(new NameValueModel { Name = "启用", Value = "1" });
                return list;
            }
        }
        /// <summary>
        /// 是否启用Enum
        /// </summary>
        public enum LevelIsEnableEnum
        {
            WeiQiYong = 0,
            QiYong = 1,
        }

        /// <summary>
        /// 是否是特殊等级
        /// </summary>
        public static List<NameValueModel> LevelIsSpecial
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "非特殊等级", Value = "0" });
                list.Add(new NameValueModel { Name = "特殊等级", Value = "1" });
                return list;
            }
        }
        /// <summary>
        /// 是否是特殊等级
        /// </summary>
        public enum LevelIsSpecialEnum
        {
            FeiTeShu = 0,
            TeShu = 1,
        }

        /// <summary>
        /// 会员等级ID
        /// </summary>
        public enum LevelEnum
        {
            VIP1 = 1,
            VIP2 = 2,
            VIP3 = 3,
            VIP4 = 4,
            /// <summary>
            /// 注册会员
            /// </summary>
            ZhuCe = 5
        }


        /// <summary>
        /// 性别
        /// </summary>
        public static List<NameValueModel> UserGenders
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "未设置", Value = "0" });
                list.Add(new NameValueModel { Name = "男", Value = "1" });
                list.Add(new NameValueModel { Name = "女", Value = "2" });
                list.Add(new NameValueModel { Name = "保密", Value = "3" });
                return list;
            }
        }
        /// <summary>
        /// 性别
        /// </summary>
        public enum UserGenderEnum
        {
            未设置 = 0,
            男 = 1,
            女 = 2,
            保密 = 3
        }

        #endregion

        #region 会员角色

        /// <summary>
        /// 会员角色
        /// </summary>
        public enum RoleEnum
        {
            供应商 = 1,
            加盟店 = 2,
            /// <summary>
            /// 用户
            /// </summary>
            注册会员 = 3,
            分销商 = 10,
            /// <summary>
            /// 医生
            /// </summary>
            医生 = 11,
            商学院 = 13
        }
        /// <summary>
        /// 会员角色
        /// </summary>
        public static List<NameValueModel> RoleValues
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "供应商", Value = "1" });
                list.Add(new NameValueModel { Name = "加盟店", Value = "2" });
                list.Add(new NameValueModel { Name = "注册会员", Value = "3" });
                list.Add(new NameValueModel { Name = "分销商", Value = "10" });
                list.Add(new NameValueModel { Name = "医生", Value = "11" });
                list.Add(new NameValueModel { Name = "商学院", Value = "13" });
                return list;
            }
        }
        #endregion

        #region 会员账户余额+积分调整

        /// <summary>
        /// 调整类型
        /// </summary>
        public enum AmountHistoryTypeEnum
        {
            增加 = 1,
            减少 = 0
        }
        /// <summary>
        /// 调整类型
        /// </summary>
        public static List<NameValueModel> AmountHistoryTypeValues
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "增加", Value = "1" });
                list.Add(new NameValueModel { Name = "减少", Value = "0" });
                return list;
            }
        }

        /// <summary>
        /// 调整资金类型
        /// </summary>
        public enum AmountTypeEnum
        {
            账户余额 = 0,
            不可用余额 = 1
        }
        /// <summary>
        /// 调整类型
        /// </summary>
        public static List<NameValueModel> AmountTypes
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "账户余额", Value = "0" });
                list.Add(new NameValueModel { Name = "不可用余额", Value = "1" });
                return list;
            }
        }

        /// <summary>
        /// 调整积分类型
        /// </summary>
        public enum ScoreTypeEnum
        {
            账户积分 = 0,
            不可用积分 = 1
        }
        /// <summary>
        /// 调整积分类型
        /// </summary>
        public static List<NameValueModel> ScoreTypes
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "账户积分", Value = "0" });
                list.Add(new NameValueModel { Name = "不可用积分", Value = "1" });
                return list;
            }
        }

        #endregion

        #region 产品属性

        /// <summary>
        /// 属性展示形式
        /// </summary>
        public static List<NameValueModel> PropertyShowType
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "文本展示", Value = "0" });
                list.Add(new NameValueModel { Name = "下拉展示", Value = "1" });
                return list;
            }
        }
        /// <summary>
        /// 属性展示形式Enum
        /// </summary>
        public enum PropertyShowTypeEnum
        {
            文本展示 = 0,
            下拉展示 = 1,
        }

        /// <summary>
        /// 属性值前台展示形式
        /// </summary>
        public static List<NameValueModel> PropertyValueShowType
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "文字", Value = "0" });
                list.Add(new NameValueModel { Name = "输入框", Value = "1" });
                return list;
            }
        }
        /// <summary>
        /// 属性值展示形式Enum
        /// </summary>
        public enum PropertyValueShowTypeEnum
        {
            文字 = 0,
            输入框 = 1,
        }


        #endregion

        #region 产品->课程

        /// <summary>
        /// 产品上下架状态
        /// </summary>
        public static List<NameValueModel> GoodsStatus
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "上架", Value = "1" });
                list.Add(new NameValueModel { Name = "下架", Value = "0" });
                return list;
            }
        }
        /// <summary>
        /// 产品上下架状态Enum
        /// </summary>
        public enum GoodsStatusEnum
        {
            上架 = 1,
            下架 = 0,
        }

        /// <summary>
        /// 产品是否重货状态
        /// </summary>
        public static List<NameValueModel> GoodsWeightStatus
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "重货", Value = "1" });
                list.Add(new NameValueModel { Name = "泡货", Value = "0" });
                return list;
            }
        }
        /// <summary>
        /// 产品是否重货状态Enum
        /// </summary>
        public enum GoodsWeightStatusEnum
        {
            重货 = 1,
            泡货 = 0,
        }
        /// <summary>
        /// 课程类型
        /// </summary>
        public static List<NameValueModel> GoodsTypes
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "商品类", Value = "193" });
                list.Add(new NameValueModel { Name = "康复类", Value = "194" });
                return list;
            }
        }
        /// <summary>
        /// 课程类型Enum
        /// </summary>
        public enum GoodsTypeEnum
        {
            商品类 = 193,
            康复类 = 194
        }

        #endregion

        #region 按需定制

        /// <summary>
        /// 按需定制处理状态
        /// </summary>
        public static List<NameValueModel> OrderCustomStatus
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "未确认", Value = "0" });
                list.Add(new NameValueModel { Name = "待付款", Value = "1" });
                list.Add(new NameValueModel { Name = "已付款", Value = "2" });
                list.Add(new NameValueModel { Name = "已发货", Value = "3" });
                list.Add(new NameValueModel { Name = "交易完成", Value = "4" });
                return list;
            }
        }
        /// <summary>
        /// 按需定制处理状态Enum
        /// </summary>
        public enum OrderCustomStatusEnum
        {
            未确认 = 0,
            待付款 = 1,
            已付款 = 2,
            已发货 = 3,
            交易完成 = 4,
        }

        #endregion

        #region 门店

        /// <summary>
        /// 门店出售商品方式
        /// </summary>
        public static List<NameValueModel> ShopSaleGoodsWay
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "出售所有商品", Value = "1" });
                list.Add(new NameValueModel { Name = "出售指定分类商品", Value = "2" });
                list.Add(new NameValueModel { Name = "出售自定义商品", Value = "3" });
                return list;
            }
        }
        /// <summary>
        /// 门店出售商品方式Enum
        /// </summary>
        public enum ShopSaleGoodsWayEnum
        {
            出售所有商品 = 1,
            出售指定分类商品 = 2,
            出售自定义商品 = 3,
        }

        /// <summary>
        /// 额度调整类型
        /// </summary>
        public enum GugranteeHistoryTypeEnum
        {
            增加 = 1,
            减少 = 0
        }
        /// <summary>
        /// 额度调整类型
        /// </summary>
        public static List<NameValueModel> GugranteeHistoryType
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "增加", Value = "1" });
                list.Add(new NameValueModel { Name = "减少", Value = "0" });
                return list;
            }
        }
        /// <summary>
        /// 额度类型
        /// </summary>
        public enum GugranteeTypeEnum
        {
            保障金额度 = 0,
            剩余可用额度 = 1
        }
        /// <summary>
        /// 额度类型
        /// </summary>
        public static List<NameValueModel> GugranteeTypes
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "保障金额度", Value = "0" });
                list.Add(new NameValueModel { Name = "剩余可用额度", Value = "1" });
                return list;
            }
        }
        /// <summary>
        /// 加盟商类型
        /// </summary>
        public enum ShopTypeEnum
        {
            门店合伙人 = 1,
            商城合伙人 = 2
        }
        /// <summary>
        /// 加盟商类型
        /// </summary>
        public static List<NameValueModel> ShopTypes
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "门店合伙人", Value = "1" });
                list.Add(new NameValueModel { Name = "商城合伙人", Value = "2" });
                return list;
            }
        }

        /// <summary>
        /// 店铺评分
        /// </summary>
        public enum ShopStarEnum
        {
            一星 = 1,
            二星 = 2,
            三星 = 3,
            四星 = 4,
            五星 = 5
        }
        /// <summary>
        /// 店铺评分
        /// </summary>
        public static List<NameValueModel> ShopStars
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "一星", Value = "1" });
                list.Add(new NameValueModel { Name = "二星", Value = "2" });
                list.Add(new NameValueModel { Name = "三星", Value = "3" });
                list.Add(new NameValueModel { Name = "四星", Value = "4" });
                list.Add(new NameValueModel { Name = "五星", Value = "5" });
                return list;
            }
        }


        #endregion

        #region 供应商

        /// <summary>
        /// 供应商生产产品
        /// </summary>
        public static List<NameValueModel> SupplyGoodsWay
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "生产所有产品", Value = "1" });
                list.Add(new NameValueModel { Name = "生产指定分类产品", Value = "2" });
                list.Add(new NameValueModel { Name = "生产自定义产品", Value = "3" });
                return list;
            }
        }
        /// <summary>
        /// 供应商生成产品Enum
        /// </summary>
        public enum SupplyGoodsWayEnum
        {
            生产所有产品 = 1,
            生产指定分类产品 = 2,
            生产自定义产品 = 3,
        }

        #endregion

        #region 广告->活动

        /// <summary>
        /// 广告展示类型
        /// </summary>
        public static List<NameValueModel> AdShowWay
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "文字", Value = "0" });
                list.Add(new NameValueModel { Name = "图文", Value = "1" });
                return list;
            }
        }
        /// <summary>
        /// 广告展示类型 Enum
        /// </summary>
        public enum AdShowWayEnum
        {
            文字 = 0,
            图文 = 1,
        }
        /// <summary>
        /// 活动状态
        /// </summary>
        public static List<NameValueModel> AdvertisementState
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "未开始", Value = "0" });
                list.Add(new NameValueModel { Name = "报名中", Value = "1" });
                list.Add(new NameValueModel { Name = "报名结束", Value = "2" });
                list.Add(new NameValueModel { Name = "已满员", Value = "3" });
                list.Add(new NameValueModel { Name = "活动已开始", Value = "4" });
                list.Add(new NameValueModel { Name = "活动已结束", Value = "5" });
                list.Add(new NameValueModel { Name = "活动已关闭", Value = "6" });
                return list;
            }
        }
        /// <summary>
        /// 活动状态 Enum
        /// </summary>
        public enum AdvertisementStateEnum
        {
            未开始 = 0,
            报名中 = 1,
            报名结束 = 2,
            已满员 = 3,
            活动已开始 = 4,
            活动已结束 = 5,
            活动已关闭 = 6
        }

        #endregion

        #region 订单相关

        /// <summary>
        /// 订单状态
        /// </summary>
        public static List<NameValueModel> OrderStatus
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "待付款", Value = "0" });
                list.Add(new NameValueModel { Name = "已付款", Value = "1" });
                list.Add(new NameValueModel { Name = "交易成功", Value = "2" });
                list.Add(new NameValueModel { Name = "交易关闭", Value = "3" });
                list.Add(new NameValueModel { Name = "已支付定金", Value = "4" });
                list.Add(new NameValueModel { Name = "已支付货款", Value = "5" });
                list.Add(new NameValueModel { Name = "月结", Value = "6" });
                return list;
            }
        }

        /// <summary>
        /// 订单状态
        /// </summary>
        public enum OrderStatusEnum
        {
            待付款 = 0,
            已付款 = 1,
            交易成功 = 2,
            交易关闭 = 3,
            已支付定金 = 4,
            /// <summary>
            /// 已支付货款指加盟商进货时，先支付货款，后支付运费
            /// </summary>
            已支付货款 = 5,
            月结 = 6
        }

        /// <summary>
        /// 订单支付方式
        /// </summary>
        public static List<NameValueModel> OrderPayWay
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "在线支付", Value = "0" });
                list.Add(new NameValueModel { Name = "货到付款", Value = "1" });
                list.Add(new NameValueModel { Name = "预付定金", Value = "2" });
                list.Add(new NameValueModel { Name = "账户余额", Value = "3" });
                list.Add(new NameValueModel { Name = "月结", Value = "4" });
                return list;
            }
        }

        /// <summary>
        /// 订单支付方式
        /// </summary>
        public enum OrderPayWayEnum
        {
            在线支付 = 0,
            货到付款 = 1,
            预付定金 = 2,
            账户余额 = 3,
            月结 = 4
        }

        /// <summary>
        /// 订单配送时间
        /// </summary>
        public static List<NameValueModel> OrderShippingTime
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "工作日与节假日均可", Value = "0" });
                list.Add(new NameValueModel { Name = "仅工作日送货", Value = "1" });
                list.Add(new NameValueModel { Name = "仅节假日送货", Value = "2" });
                return list;
            }
        }

        /// <summary>
        /// 订单配送时间
        /// </summary>
        public enum OrderShippingTimeEnum
        {
            工作日与节假日均可 = 0,
            仅工作日送货 = 1,
            仅节假日送货 = 2,
        }

        /// <summary>
        /// 订单配送方式
        /// </summary>
        public static List<NameValueModel> OrderShippingWay
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "快递配送", Value = "0" });
                list.Add(new NameValueModel { Name = "上门自提", Value = "1" });
                list.Add(new NameValueModel { Name = "物流配送", Value = "2" });
                return list;
            }
        }

        /// <summary>
        /// 订单配送方式
        /// </summary>
        public enum OrderShippingWayEnum
        {
            快递配送 = 0,
            上门自提 = 1,
            物流配送 = 2,
        }

        // 订单是否开发票
        /// </summary>
        public static List<NameValueModel> OrderInvoice
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "否", Value = "0" });
                list.Add(new NameValueModel { Name = "个人", Value = "1" });
                list.Add(new NameValueModel { Name = "企业", Value = "2" });
                return list;
            }
        }

        /// <summary>
        /// 订单是否开发票
        /// </summary>
        public enum OrderInvoiceEnum
        {
            否 = 0,
            个人 = 1,
            企业 = 2,
        }

        // 订单发货状态
        /// </summary>
        public static List<NameValueModel> OrderShippingStatus
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "待发货", Value = "0" });
                list.Add(new NameValueModel { Name = "已发货", Value = "1" });
                list.Add(new NameValueModel { Name = "已收货", Value = "2" });
                list.Add(new NameValueModel { Name = "部分发货", Value = "3" });
                list.Add(new NameValueModel { Name = "已退货", Value = "-1" });
                return list;
            }
        }

        /// <summary>
        /// 订单发货状态
        /// </summary>
        public enum OrderShippingStatusEnum
        {
            待发货 = 0,
            已发货 = 1,
            已收货 = 2,
            部分发货 = 3,
            已退货 = -1,
        }

        // 订单包裹投递状态
        /// </summary>
        public static List<NameValueModel> OrderShippingDeliveryStatus
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "等待揽收", Value = "0" });
                list.Add(new NameValueModel { Name = "已揽收", Value = "1" });
                list.Add(new NameValueModel { Name = "运输中", Value = "2" });
                list.Add(new NameValueModel { Name = "正在投递", Value = "3" });
                list.Add(new NameValueModel { Name = "已签收", Value = "4" });
                list.Add(new NameValueModel { Name = "未妥投", Value = "5" });
                list.Add(new NameValueModel { Name = "转窗投", Value = "6" });
                return list;
            }
        }

        /// <summary>
        /// 订单包裹投递状态
        /// </summary>
        public enum OrderShippingDeliveryStatusEnum
        {
            等待揽收 = 0,
            已揽收 = 1,
            运输中 = 2,
            正在投递 = 3,
            已签收 = 4,
            未妥投 = 5,
            转窗投 = 6,
        }

        // 订单产品是否需要设计稿状态
        /// </summary>
        public static List<NameValueModel> OrderDetailIsHasDesignFile
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "不需要设计稿", Value = "0" });
                list.Add(new NameValueModel { Name = "有设计稿", Value = "1" });
                list.Add(new NameValueModel { Name = "无设计稿", Value = "2" });
                return list;
            }
        }

        /// <summary>
        /// 订单产品是否需要设计稿状态 enum
        /// </summary>
        public enum OrderDetailIsHasDesignFileEnum
        {
            不需要设计稿 = 0,
            有设计稿 = 1,
            无设计稿 = 2,
        }

        // 订单开发票状态
        /// </summary>
        public static List<NameValueModel> OrderInvoiceState
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "未开票", Value = "0" });
                list.Add(new NameValueModel { Name = "已开票", Value = "1" });
                return list;
            }
        }

        /// <summary>
        /// 订单开发票状态
        /// </summary>
        public enum OrderInvoiceStateEnum
        {
            未开票 = 0,
            已开票 = 1
        }

        #endregion

        #region 发票

        /// <summary>
        /// 发票类型
        /// </summary>
        public static List<NameValueModel> InvoiceType
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "增值税普通发票", Value = "0" });
                list.Add(new NameValueModel { Name = "增值税专用发票", Value = "1" });
                return list;
            }
        }
        /// <summary>
        /// 发票类型 Enum
        /// </summary>
        public enum InvoiceTypeEnum
        {
            增值税普通发票 = 0,
            增值税专用发票 = 1,
        }

        /// <summary>
        /// 开票用户类型
        /// </summary>
        public static List<NameValueModel> InvoiceUserType
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "企业", Value = "0" });
                list.Add(new NameValueModel { Name = "个人", Value = "1" });
                return list;
            }
        }
        /// <summary>
        /// 开票用户类型 Enum
        /// </summary>
        public enum InvoiceUserTypeEnum
        {
            企业 = 0,
            个人 = 1,
        }

        /// <summary>
        /// 开票状态
        /// </summary>
        public static List<NameValueModel> InvoiceStatus
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "待支付税点", Value = "0" });
                list.Add(new NameValueModel { Name = "待处理", Value = "1" });
                list.Add(new NameValueModel { Name = "已寄出", Value = "2" });
                list.Add(new NameValueModel { Name = "完成", Value = "3" });
                list.Add(new NameValueModel { Name = "已取消", Value = "4" });
                return list;
            }
        }
        /// <summary>
        /// 开票状态 Enum
        /// </summary>
        public enum InvoiceStatusEnum
        {
            待支付税点 = 0,
            /// <summary>
            /// 已支付税点，或普通发票默认状态
            /// </summary>
            待处理 = 1,
            已寄出 = 2,
            完成 = 3,
            已取消 = 4
        }

        #endregion

        #region 退货相关

        /// <summary>
        /// 退货处理状态
        /// </summary>
        public static List<NameValueModel> ReturnOrderStatus
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "已申请待商家确认", Value = "0" });
                list.Add(new NameValueModel { Name = "商家已同意退货", Value = "1" });
                list.Add(new NameValueModel { Name = "商家不同意退货", Value = "2" });
                list.Add(new NameValueModel { Name = "退货完成", Value = "3" });
                list.Add(new NameValueModel { Name = "已取消", Value = "4" });
                return list;
            }
        }
        /// <summary>
        /// 退货处理状态Enum
        /// </summary>
        public enum ReturnOrderStatusEnum
        {
            已申请待商家确认 = 0,
            商家已同意退货 = 1,
            商家不同意退货 = 2,
            退货完成 = 3,
            已取消 = 4,
        }

        /// <summary>
        /// 退货状态
        /// </summary>
        public static List<NameValueModel> ReturnOrderShippingStatus
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "待退货", Value = "0" });
                list.Add(new NameValueModel { Name = "已退货", Value = "1" });
                list.Add(new NameValueModel { Name = "已收退货", Value = "2" });
                return list;
            }
        }
        /// <summary>
        /// 退货状态Enum
        /// </summary>
        public enum ReturnOrderShippingStatusEnum
        {
            待退货 = 0,
            已退货 = 1,
            已收退货 = 2,
        }

        /// <summary>
        /// 退款状态
        /// </summary>
        public static List<NameValueModel> ReturnOrderPayStatus
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "未退款", Value = "0" });
                list.Add(new NameValueModel { Name = "已退款", Value = "1" });
                return list;
            }
        }
        /// <summary>
        /// 退款状态Enum
        /// </summary>
        public enum ReturnOrderPayStatusEnum
        {
            未退款 = 0,
            已退款 = 1,
        }

        /// <summary>
        /// 退货原因
        /// </summary>
        public static List<NameValueModel> ReturnOrderReason
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "商品质量问题", Value = "0" });
                list.Add(new NameValueModel { Name = "退运费", Value = "1" });
                list.Add(new NameValueModel { Name = "收到商品破损", Value = "2" });
                list.Add(new NameValueModel { Name = "商品错发/漏发", Value = "3" });
                return list;
            }
        }
        /// <summary>
        /// 退货原因Enum
        /// </summary>
        public enum ReturnOrderReasonEnum
        {
            商品质量问题 = 0,
            退运费 = 1,
            收到商品破损 = 2,
            商品错发漏发 = 3
        }

        /// <summary>
        /// 退货类型
        /// </summary>
        public static List<NameValueModel> ReturnOrderType
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "退货退款", Value = "0" });
                list.Add(new NameValueModel { Name = "仅退款", Value = "1" });
                return list;
            }
        }
        /// <summary>
        /// 退货类型Enum
        /// </summary>
        public enum ReturnOrderTypeEnum
        {
            退货退款 = 0,
            仅退款 = 1,
        }

        #endregion

        #region 运送+运费模板

        /// <summary>
        /// 运费模板所属用户类型
        /// </summary>
        public static List<NameValueModel> ShippingAreaUserType
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "平台默认", Value = "0" });
                list.Add(new NameValueModel { Name = "仓库", Value = "1" });
                list.Add(new NameValueModel { Name = "供应商", Value = "2" });
                list.Add(new NameValueModel { Name = "门店", Value = "3" });
                return list;
            }
        }

        /// <summary>
        /// 运费模板所属用户类型
        /// </summary>
        public enum ShippingAreaUserTypeEnum
        {
            平台默认 = 0,
            仓库 = 1,
            供应商 = 2,
            门店 = 3,
        }

        /// <summary>
        /// 免运费省份ID
        /// </summary>
        public static List<NameValueModel> ShippingFeeAreaID
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "江苏", Value = "16" });
                list.Add(new NameValueModel { Name = "浙江", Value = "31" });
                list.Add(new NameValueModel { Name = "上海", Value = "25" });
                list.Add(new NameValueModel { Name = "安徽", Value = "3" });
                return list;
            }
        }

        /// <summary>
        /// 免运费省份ID Enum
        /// </summary>
        public enum ShippingFeeAreaIDEnum
        {
            江苏 = 16,
            浙江 = 31,
            上海 = 25,
            安徽 = 3,
        }
        #endregion

        #region 商城价格比例关系- -已取消（作废）该计算方法 20171025

        public static List<PricePercentItem> PricePercents
        {
            get
            {
                List<PricePercentItem> list = new List<PricePercentItem>();
                list.Add(new PricePercentItem { TotalCostPrice = 10M, ShopPricePercent = 1.1, ClientPricePercent = 5, FenxiaoPricePercent = 0.8 });
                return list;
            }
        }

        /// <summary>
        /// 获取终端总价 -已取消（作废）该计算方法 20171025
        /// </summary>
        /// <param name="costprice"></param>
        /// <returns></returns>
        public static decimal GetClientPrice(decimal costprice)
        {
            return costprice;
            //PricePercentItem pricePercentItem = DataConfig.PricePercents.Where(m => m.TotalCostPrice > costprice || m.TotalCostPrice == costprice).OrderBy(m => m.TotalCostPrice).FirstOrDefault();
            //return Math.Round(Convert.ToDecimal(Convert.ToDouble(costprice) * pricePercentItem.ShopPricePercent * pricePercentItem.ClientPricePercent), 2);
        }
        /// <summary>
        /// 获取加盟商总价
        /// </summary>
        /// <param name="costprice"></param>
        /// <returns></returns>
        public static decimal GetShopPrice(decimal costprice)
        {
            PricePercentItem pricePercentItem = DataConfig.PricePercents.Where(m => m.TotalCostPrice > costprice || m.TotalCostPrice == costprice).OrderBy(m => m.TotalCostPrice).FirstOrDefault();
            return Math.Round(Convert.ToDecimal(Convert.ToDouble(costprice) * pricePercentItem.ShopPricePercent), 2);
        }
        /// <summary>
        /// 获取加盟商利润总价
        /// </summary>
        /// <param name="costprice"></param>
        /// <returns></returns>
        public static decimal GetShopProfitPrice(decimal costprice)
        {
            return Math.Round(GetFenxiaoPrice(costprice) - GetShopPrice(costprice), 2);
        }
        /// <summary>
        /// 获取分销商总价
        /// </summary>
        /// <param name="costprice"></param>
        /// <returns></returns>
        public static decimal GetFenxiaoPrice(decimal costprice)
        {
            PricePercentItem pricePercentItem = DataConfig.PricePercents.Where(m => m.TotalCostPrice > costprice || m.TotalCostPrice == costprice).OrderBy(m => m.TotalCostPrice).FirstOrDefault();
            return Math.Round(Convert.ToDecimal(Convert.ToDouble(costprice) * pricePercentItem.ShopPricePercent * pricePercentItem.ClientPricePercent * pricePercentItem.FenxiaoPricePercent), 2);
        }
        /// <summary>
        /// 获取分销商利润总价
        /// </summary>
        /// <param name="costprice"></param>
        /// <returns></returns>
        public static decimal GetFenxiaoProfitPrice(decimal costprice)
        {
            return Math.Round(GetClientPrice(costprice) - GetFenxiaoPrice(costprice), 2);
        }

        #endregion

        #region 打样--数据

        #region 打样一 + 打样二

        //打样一数据
        public static List<DaYang1AreaPriceModel> DaYang1AreaPriceData
        {
            get
            {
                List<DaYang1AreaPriceModel> list = new List<DaYang1AreaPriceModel>();

                List<DaYang1CountPriceModel> listCount = new List<DaYang1CountPriceModel>();
                listCount.Add(new DaYang1CountPriceModel { Count = 1, CostPrice0 = 20M, CostPrice1 = 24M });
                listCount.Add(new DaYang1CountPriceModel { Count = 2, CostPrice0 = 13, CostPrice1 = 16 });
                listCount.Add(new DaYang1CountPriceModel { Count = 5, CostPrice0 = 7.5M, CostPrice1 = 9 });
                listCount.Add(new DaYang1CountPriceModel { Count = 10, CostPrice0 = 6.5M, CostPrice1 = 7 });
                listCount.Add(new DaYang1CountPriceModel { Count = 20, CostPrice0 = 5.5M, CostPrice1 = 6.5M });
                //附加最小值
                list.Add(new DaYang1AreaPriceModel { UnitArea = 0, CountPriceList = listCount });
                list.Add(new DaYang1AreaPriceModel { UnitArea = 210 * 143, CountPriceList = listCount });


                listCount = new List<DaYang1CountPriceModel>();
                listCount.Add(new DaYang1CountPriceModel { Count = 1, CostPrice0 = 29, CostPrice1 = 36 });
                listCount.Add(new DaYang1CountPriceModel { Count = 2, CostPrice0 = 19, CostPrice1 = 23 });
                listCount.Add(new DaYang1CountPriceModel { Count = 5, CostPrice0 = 11M, CostPrice1 = 13.5M });
                listCount.Add(new DaYang1CountPriceModel { Count = 10, CostPrice0 = 9.5M, CostPrice1 = 11 });
                listCount.Add(new DaYang1CountPriceModel { Count = 20, CostPrice0 = 8M, CostPrice1 = 9M });
                list.Add(new DaYang1AreaPriceModel { UnitArea = 210 * 285, CountPriceList = listCount });


                listCount = new List<DaYang1CountPriceModel>();
                listCount.Add(new DaYang1CountPriceModel { Count = 1, CostPrice0 = 42, CostPrice1 = 49 });
                listCount.Add(new DaYang1CountPriceModel { Count = 2, CostPrice0 = 26, CostPrice1 = 30 });
                listCount.Add(new DaYang1CountPriceModel { Count = 5, CostPrice0 = 17.5M, CostPrice1 = 20.5M });
                listCount.Add(new DaYang1CountPriceModel { Count = 10, CostPrice0 = 13, CostPrice1 = 15 });
                listCount.Add(new DaYang1CountPriceModel { Count = 20, CostPrice0 = 10.5M, CostPrice1 = 12.5M });
                list.Add(new DaYang1AreaPriceModel { UnitArea = 420 * 285, CountPriceList = listCount });


                listCount = new List<DaYang1CountPriceModel>();
                listCount.Add(new DaYang1CountPriceModel { Count = 1, CostPrice0 = 55, CostPrice1 = 61.5M });
                listCount.Add(new DaYang1CountPriceModel { Count = 2, CostPrice0 = 36.5M, CostPrice1 = 40 });
                listCount.Add(new DaYang1CountPriceModel { Count = 5, CostPrice0 = 24.5M, CostPrice1 = 27.5M });
                listCount.Add(new DaYang1CountPriceModel { Count = 10, CostPrice0 = 19.5M, CostPrice1 = 22 });
                listCount.Add(new DaYang1CountPriceModel { Count = 20, CostPrice0 = 15.5M, CostPrice1 = 17.5M });
                list.Add(new DaYang1AreaPriceModel { UnitArea = 390 * 420, CountPriceList = listCount });


                listCount = new List<DaYang1CountPriceModel>();
                listCount.Add(new DaYang1CountPriceModel { Count = 1, CostPrice0 = 68, CostPrice1 = 74.5M });
                listCount.Add(new DaYang1CountPriceModel { Count = 2, CostPrice0 = 46, CostPrice1 = 49.5M });
                listCount.Add(new DaYang1CountPriceModel { Count = 5, CostPrice0 = 31, CostPrice1 = 34 });
                listCount.Add(new DaYang1CountPriceModel { Count = 10, CostPrice0 = 26, CostPrice1 = 28.5M });
                listCount.Add(new DaYang1CountPriceModel { Count = 20, CostPrice0 = 19.5M, CostPrice1 = 22 });
                list.Add(new DaYang1AreaPriceModel { UnitArea = 420 * 580, CountPriceList = listCount });


                listCount = new List<DaYang1CountPriceModel>();
                listCount.Add(new DaYang1CountPriceModel { Count = 1, CostPrice0 = 87, CostPrice1 = 95 });
                listCount.Add(new DaYang1CountPriceModel { Count = 2, CostPrice0 = 57, CostPrice1 = 62 });
                listCount.Add(new DaYang1CountPriceModel { Count = 5, CostPrice0 = 40, CostPrice1 = 44 });
                listCount.Add(new DaYang1CountPriceModel { Count = 10, CostPrice0 = 32, CostPrice1 = 36 });
                listCount.Add(new DaYang1CountPriceModel { Count = 20, CostPrice0 = 26, CostPrice1 = 29 });
                list.Add(new DaYang1AreaPriceModel { UnitArea = 880 * 390, CountPriceList = listCount });

                listCount = new List<DaYang1CountPriceModel>();
                listCount.Add(new DaYang1CountPriceModel { Count = 1, CostPrice0 = 106, CostPrice1 = 114 });
                listCount.Add(new DaYang1CountPriceModel { Count = 2, CostPrice0 = 68, CostPrice1 = 74 });
                listCount.Add(new DaYang1CountPriceModel { Count = 5, CostPrice0 = 49, CostPrice1 = 54 });
                listCount.Add(new DaYang1CountPriceModel { Count = 10, CostPrice0 = 39, CostPrice1 = 43 });
                listCount.Add(new DaYang1CountPriceModel { Count = 20, CostPrice0 = 33, CostPrice1 = 35 });
                list.Add(new DaYang1AreaPriceModel { UnitArea = 880 * 580, CountPriceList = listCount });
                //附加最大值
                list.Add(new DaYang1AreaPriceModel { UnitArea = 880 * 580, CountPriceList = listCount });

                return list;
            }
        }
        //打样二数据
        public static List<DaYang1AreaPriceModel> DaYang2AreaPriceData
        {
            get
            {
                List<DaYang1AreaPriceModel> list = new List<DaYang1AreaPriceModel>();

                List<DaYang1CountPriceModel> listCount = new List<DaYang1CountPriceModel>();
                listCount.Add(new DaYang1CountPriceModel { Count = 1, CostPrice0 = 40, CostPrice1 = 46 });
                listCount.Add(new DaYang1CountPriceModel { Count = 2, CostPrice0 = 25, CostPrice1 = 29 });
                listCount.Add(new DaYang1CountPriceModel { Count = 5, CostPrice0 = 14.5M, CostPrice1 = 17 });
                listCount.Add(new DaYang1CountPriceModel { Count = 10, CostPrice0 = 11.5M, CostPrice1 = 13 });
                listCount.Add(new DaYang1CountPriceModel { Count = 20, CostPrice0 = 10, CostPrice1 = 11 });
                //附加最小值
                list.Add(new DaYang1AreaPriceModel { UnitArea = 0, CountPriceList = listCount });
                list.Add(new DaYang1AreaPriceModel { UnitArea = 210 * 285, CountPriceList = listCount });


                listCount = new List<DaYang1CountPriceModel>();
                listCount.Add(new DaYang1CountPriceModel { Count = 1, CostPrice0 = 52, CostPrice1 = 59 });
                listCount.Add(new DaYang1CountPriceModel { Count = 2, CostPrice0 = 33, CostPrice1 = 37 });
                listCount.Add(new DaYang1CountPriceModel { Count = 5, CostPrice0 = 22, CostPrice1 = 25 });
                listCount.Add(new DaYang1CountPriceModel { Count = 10, CostPrice0 = 17, CostPrice1 = 19 });
                listCount.Add(new DaYang1CountPriceModel { Count = 20, CostPrice0 = 14, CostPrice1 = 16 });
                list.Add(new DaYang1AreaPriceModel { UnitArea = 420 * 285, CountPriceList = listCount });


                listCount = new List<DaYang1CountPriceModel>();
                listCount.Add(new DaYang1CountPriceModel { Count = 1, CostPrice0 = 66, CostPrice1 = 73 });
                listCount.Add(new DaYang1CountPriceModel { Count = 2, CostPrice0 = 43, CostPrice1 = 47 });
                listCount.Add(new DaYang1CountPriceModel { Count = 5, CostPrice0 = 29, CostPrice1 = 32 });
                listCount.Add(new DaYang1CountPriceModel { Count = 10, CostPrice0 = 23, CostPrice1 = 25.5M });
                listCount.Add(new DaYang1CountPriceModel { Count = 20, CostPrice0 = 18.5M, CostPrice1 = 20.5M });
                list.Add(new DaYang1AreaPriceModel { UnitArea = 390 * 420, CountPriceList = listCount });


                listCount = new List<DaYang1CountPriceModel>();
                listCount.Add(new DaYang1CountPriceModel { Count = 1, CostPrice0 = 80, CostPrice1 = 87 });
                listCount.Add(new DaYang1CountPriceModel { Count = 2, CostPrice0 = 53, CostPrice1 = 57 });
                listCount.Add(new DaYang1CountPriceModel { Count = 5, CostPrice0 = 36, CostPrice1 = 39 });
                listCount.Add(new DaYang1CountPriceModel { Count = 10, CostPrice0 = 30, CostPrice1 = 32 });
                listCount.Add(new DaYang1CountPriceModel { Count = 20, CostPrice0 = 23, CostPrice1 = 25 });
                list.Add(new DaYang1AreaPriceModel { UnitArea = 420 * 580, CountPriceList = listCount });


                listCount = new List<DaYang1CountPriceModel>();
                listCount.Add(new DaYang1CountPriceModel { Count = 1, CostPrice0 = 100, CostPrice1 = 107 });
                listCount.Add(new DaYang1CountPriceModel { Count = 2, CostPrice0 = 67, CostPrice1 = 72 });
                listCount.Add(new DaYang1CountPriceModel { Count = 5, CostPrice0 = 47, CostPrice1 = 50 });
                listCount.Add(new DaYang1CountPriceModel { Count = 10, CostPrice0 = 38, CostPrice1 = 41 });
                listCount.Add(new DaYang1CountPriceModel { Count = 20, CostPrice0 = 31, CostPrice1 = 33 });
                list.Add(new DaYang1AreaPriceModel { UnitArea = 880 * 390, CountPriceList = listCount });


                listCount = new List<DaYang1CountPriceModel>();
                listCount.Add(new DaYang1CountPriceModel { Count = 1, CostPrice0 = 120, CostPrice1 = 128 });
                listCount.Add(new DaYang1CountPriceModel { Count = 2, CostPrice0 = 81, CostPrice1 = 87 });
                listCount.Add(new DaYang1CountPriceModel { Count = 5, CostPrice0 = 57, CostPrice1 = 62 });
                listCount.Add(new DaYang1CountPriceModel { Count = 10, CostPrice0 = 45, CostPrice1 = 49 });
                listCount.Add(new DaYang1CountPriceModel { Count = 20, CostPrice0 = 38, CostPrice1 = 41 });
                list.Add(new DaYang1AreaPriceModel { UnitArea = 880 * 580, CountPriceList = listCount });

                listCount = new List<DaYang1CountPriceModel>();
                listCount.Add(new DaYang1CountPriceModel { Count = 1, CostPrice0 = 165, CostPrice1 = 195 });
                listCount.Add(new DaYang1CountPriceModel { Count = 2, CostPrice0 = 114, CostPrice1 = 134 });
                listCount.Add(new DaYang1CountPriceModel { Count = 5, CostPrice0 = 77, CostPrice1 = 92 });
                listCount.Add(new DaYang1CountPriceModel { Count = 10, CostPrice0 = 61, CostPrice1 = 71 });
                listCount.Add(new DaYang1CountPriceModel { Count = 20, CostPrice0 = 54, CostPrice1 = 62 });
                list.Add(new DaYang1AreaPriceModel { UnitArea = 1020 * 720, CountPriceList = listCount });

                listCount = new List<DaYang1CountPriceModel>();
                listCount.Add(new DaYang1CountPriceModel { Count = 1, CostPrice0 = 225, CostPrice1 = 255 });
                listCount.Add(new DaYang1CountPriceModel { Count = 2, CostPrice0 = 158, CostPrice1 = 177 });
                listCount.Add(new DaYang1CountPriceModel { Count = 5, CostPrice0 = 90, CostPrice1 = 100 });
                listCount.Add(new DaYang1CountPriceModel { Count = 10, CostPrice0 = 73, CostPrice1 = 83 });
                listCount.Add(new DaYang1CountPriceModel { Count = 20, CostPrice0 = 63, CostPrice1 = 70 });
                list.Add(new DaYang1AreaPriceModel { UnitArea = 1180 * 880, CountPriceList = listCount });


                return list;
            }
        }



        #endregion

        #region 打样三 + 打样四

        //打样三数据
        public static List<DaYang2AreaPriceModel> DaYang3AreaPriceData
        {
            get
            {
                List<DaYang2AreaPriceModel> list = new List<DaYang2AreaPriceModel>();

                List<DaYang2CountPriceModel> listCount = new List<DaYang2CountPriceModel>();
                listCount.Add(new DaYang2CountPriceModel { Count = 1, CostPrice = 55 });
                listCount.Add(new DaYang2CountPriceModel { Count = 2, CostPrice = 35 });
                listCount.Add(new DaYang2CountPriceModel { Count = 5, CostPrice = 25 });
                listCount.Add(new DaYang2CountPriceModel { Count = 10, CostPrice = 20 });
                listCount.Add(new DaYang2CountPriceModel { Count = 20, CostPrice = 15 });
                //附加最小值
                list.Add(new DaYang2AreaPriceModel { UnitArea = 0, CountPriceList = listCount });
                list.Add(new DaYang2AreaPriceModel { UnitArea = 210 * 285, CountPriceList = listCount });


                listCount = new List<DaYang2CountPriceModel>();
                listCount.Add(new DaYang2CountPriceModel { Count = 1, CostPrice = 65 });
                listCount.Add(new DaYang2CountPriceModel { Count = 2, CostPrice = 45 });
                listCount.Add(new DaYang2CountPriceModel { Count = 5, CostPrice = 32 });
                listCount.Add(new DaYang2CountPriceModel { Count = 10, CostPrice = 25 });
                listCount.Add(new DaYang2CountPriceModel { Count = 20, CostPrice = 19 });
                list.Add(new DaYang2AreaPriceModel { UnitArea = 420 * 285, CountPriceList = listCount });


                listCount = new List<DaYang2CountPriceModel>();
                listCount.Add(new DaYang2CountPriceModel { Count = 1, CostPrice = 80 });
                listCount.Add(new DaYang2CountPriceModel { Count = 2, CostPrice = 55 });
                listCount.Add(new DaYang2CountPriceModel { Count = 5, CostPrice = 40 });
                listCount.Add(new DaYang2CountPriceModel { Count = 10, CostPrice = 32 });
                listCount.Add(new DaYang2CountPriceModel { Count = 20, CostPrice = 30 });
                list.Add(new DaYang2AreaPriceModel { UnitArea = 390 * 420, CountPriceList = listCount });


                listCount = new List<DaYang2CountPriceModel>();
                listCount.Add(new DaYang2CountPriceModel { Count = 1, CostPrice = 92 });
                listCount.Add(new DaYang2CountPriceModel { Count = 2, CostPrice = 65 });
                listCount.Add(new DaYang2CountPriceModel { Count = 5, CostPrice = 45 });
                listCount.Add(new DaYang2CountPriceModel { Count = 10, CostPrice = 37 });
                listCount.Add(new DaYang2CountPriceModel { Count = 20, CostPrice = 35 });
                list.Add(new DaYang2AreaPriceModel { UnitArea = 420 * 580, CountPriceList = listCount });



                listCount = new List<DaYang2CountPriceModel>();
                listCount.Add(new DaYang2CountPriceModel { Count = 1, CostPrice = 115 });
                listCount.Add(new DaYang2CountPriceModel { Count = 2, CostPrice = 75 });
                listCount.Add(new DaYang2CountPriceModel { Count = 5, CostPrice = 55 });
                listCount.Add(new DaYang2CountPriceModel { Count = 10, CostPrice = 45 });
                listCount.Add(new DaYang2CountPriceModel { Count = 20, CostPrice = 43 });
                list.Add(new DaYang2AreaPriceModel { UnitArea = 880 * 390, CountPriceList = listCount });


                listCount = new List<DaYang2CountPriceModel>();
                listCount.Add(new DaYang2CountPriceModel { Count = 1, CostPrice = 135 });
                listCount.Add(new DaYang2CountPriceModel { Count = 2, CostPrice = 88 });
                listCount.Add(new DaYang2CountPriceModel { Count = 5, CostPrice = 65 });
                listCount.Add(new DaYang2CountPriceModel { Count = 10, CostPrice = 52 });
                listCount.Add(new DaYang2CountPriceModel { Count = 20, CostPrice = 50 });
                list.Add(new DaYang2AreaPriceModel { UnitArea = 880 * 580, CountPriceList = listCount });

                return list;
            }
        }
        //打样四数据
        public static List<DaYang2AreaPriceModel> DaYang4AreaPriceData
        {
            get
            {
                List<DaYang2AreaPriceModel> list = new List<DaYang2AreaPriceModel>();

                List<DaYang2CountPriceModel> listCount = new List<DaYang2CountPriceModel>();
                listCount.Add(new DaYang2CountPriceModel { Count = 1, CostPrice = 135 });
                listCount.Add(new DaYang2CountPriceModel { Count = 2, CostPrice = 102 });
                listCount.Add(new DaYang2CountPriceModel { Count = 5, CostPrice = 65 });
                listCount.Add(new DaYang2CountPriceModel { Count = 10, CostPrice = 50 });
                listCount.Add(new DaYang2CountPriceModel { Count = 20, CostPrice = 40 });
                //附加最小值
                list.Add(new DaYang2AreaPriceModel { UnitArea = 0, CountPriceList = listCount });
                list.Add(new DaYang2AreaPriceModel { UnitArea = 210 * 285, CountPriceList = listCount });


                listCount = new List<DaYang2CountPriceModel>();
                listCount.Add(new DaYang2CountPriceModel { Count = 1, CostPrice = 170 });
                listCount.Add(new DaYang2CountPriceModel { Count = 2, CostPrice = 125 });
                listCount.Add(new DaYang2CountPriceModel { Count = 5, CostPrice = 80 });
                listCount.Add(new DaYang2CountPriceModel { Count = 10, CostPrice = 63 });
                listCount.Add(new DaYang2CountPriceModel { Count = 20, CostPrice = 50 });
                list.Add(new DaYang2AreaPriceModel { UnitArea = 420 * 285, CountPriceList = listCount });


                listCount = new List<DaYang2CountPriceModel>();
                listCount.Add(new DaYang2CountPriceModel { Count = 1, CostPrice = 205 });
                listCount.Add(new DaYang2CountPriceModel { Count = 2, CostPrice = 150 });
                listCount.Add(new DaYang2CountPriceModel { Count = 5, CostPrice = 95 });
                listCount.Add(new DaYang2CountPriceModel { Count = 10, CostPrice = 75 });
                listCount.Add(new DaYang2CountPriceModel { Count = 20, CostPrice = 62 });
                list.Add(new DaYang2AreaPriceModel { UnitArea = 390 * 420, CountPriceList = listCount });


                listCount = new List<DaYang2CountPriceModel>();
                listCount.Add(new DaYang2CountPriceModel { Count = 1, CostPrice = 235 });
                listCount.Add(new DaYang2CountPriceModel { Count = 2, CostPrice = 176 });
                listCount.Add(new DaYang2CountPriceModel { Count = 5, CostPrice = 110 });
                listCount.Add(new DaYang2CountPriceModel { Count = 10, CostPrice = 90 });
                listCount.Add(new DaYang2CountPriceModel { Count = 20, CostPrice = 73 });
                list.Add(new DaYang2AreaPriceModel { UnitArea = 420 * 580, CountPriceList = listCount });



                listCount = new List<DaYang2CountPriceModel>();
                listCount.Add(new DaYang2CountPriceModel { Count = 1, CostPrice = 285 });
                listCount.Add(new DaYang2CountPriceModel { Count = 2, CostPrice = 225 });
                listCount.Add(new DaYang2CountPriceModel { Count = 5, CostPrice = 137 });
                listCount.Add(new DaYang2CountPriceModel { Count = 10, CostPrice = 110 });
                listCount.Add(new DaYang2CountPriceModel { Count = 20, CostPrice = 90 });
                list.Add(new DaYang2AreaPriceModel { UnitArea = 880 * 390, CountPriceList = listCount });


                listCount = new List<DaYang2CountPriceModel>();
                listCount.Add(new DaYang2CountPriceModel { Count = 1, CostPrice = 332 });
                listCount.Add(new DaYang2CountPriceModel { Count = 2, CostPrice = 270 });
                listCount.Add(new DaYang2CountPriceModel { Count = 5, CostPrice = 162 });
                listCount.Add(new DaYang2CountPriceModel { Count = 10, CostPrice = 130 });
                listCount.Add(new DaYang2CountPriceModel { Count = 20, CostPrice = 108 });
                list.Add(new DaYang2AreaPriceModel { UnitArea = 880 * 580, CountPriceList = listCount });

                return list;
            }
        }

        #endregion

        #endregion

        #region 优惠券 +红包

        /// <summary>
        /// 优惠券状态
        /// </summary>
        public static List<NameValueModel> CouponStatus
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "未开始", Value = "0" });
                list.Add(new NameValueModel { Name = "已开始", Value = "1" });
                list.Add(new NameValueModel { Name = "已结束", Value = "2" });
                list.Add(new NameValueModel { Name = "已中止", Value = "3" });
                list.Add(new NameValueModel { Name = "已领完", Value = "4" });
                return list;
            }
        }
        /// <summary>
        /// 优惠券状态Enum
        /// </summary>
        public enum CouponStatusEnum
        {
            未开始 = 0,
            已开始 = 1,
            已结束 = 2,
            已中止 = 3,
            已领完 = 4
        }

        /// <summary>
        /// 优惠券适用范围
        /// </summary>
        public static List<NameValueModel> CouponUsingRange
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "平台通用类", Value = "1" });
                list.Add(new NameValueModel { Name = "店铺通用类", Value = "2" });
                //list.Add(new NameValueModel { Name = "品类通用类", Value = "3" });
                //list.Add(new NameValueModel { Name = "特定商品使用", Value = "4" });
                return list;
            }
        }
        /// <summary>
        /// 优惠券适用范围Enum
        /// </summary>
        public enum CouponUsingRangeEnum
        {
            平台通用类 = 1,
            店铺通用类 = 2
            //    ,
            //品类通用类 = 3,
            //特定商品使用 = 4
        }

        /// <summary>
        /// 用户优惠券状态
        /// </summary>
        public static List<NameValueModel> UserCouponStatus
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "未使用", Value = "0" });
                list.Add(new NameValueModel { Name = "已使用", Value = "1" });
                return list;
            }
        }
        /// <summary>
        /// 用户优惠券状态Enum
        /// </summary>
        public enum UserCouponStatusEnum
        {
            未使用 = 0,
            已使用 = 1,
        }


        /// <summary>
        /// 红包状态
        /// </summary>
        public static List<NameValueModel> HongBaoStatus
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "未开始", Value = "0" });
                list.Add(new NameValueModel { Name = "已开始", Value = "1" });
                list.Add(new NameValueModel { Name = "已结束", Value = "2" });
                list.Add(new NameValueModel { Name = "已中止", Value = "3" });
                list.Add(new NameValueModel { Name = "已领完", Value = "4" });
                return list;
            }
        }
        /// <summary>
        /// 红包状态Enum
        /// </summary>
        public enum HongBaoStatusEnum
        {
            未开始 = 0,
            已开始 = 1,
            已结束 = 2,
            已中止 = 3,
            已领完 = 4
        }
        /// <summary>
        /// 用户红包状态
        /// </summary>
        public static List<NameValueModel> UserHongBaoStatus
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "未使用", Value = "0" });
                list.Add(new NameValueModel { Name = "已使用", Value = "1" });
                return list;
            }
        }
        /// <summary>
        /// 用户红包状态Enum
        /// </summary>
        public enum UserHongBaoStatusEnum
        {
            未使用 = 0,
            已使用 = 1,
        }

        /// <summary>
        /// 红包有效期天数
        /// </summary>
        public static List<NameValueModel> HongBaoValidDays
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "1天", Value = "1" });
                list.Add(new NameValueModel { Name = "2天", Value = "2" });
                list.Add(new NameValueModel { Name = "7天", Value = "7" });
                list.Add(new NameValueModel { Name = "15天", Value = "15" });
                list.Add(new NameValueModel { Name = "30天", Value = "30" });
                list.Add(new NameValueModel { Name = "60天", Value = "60" });
                list.Add(new NameValueModel { Name = "90天", Value = "90" });
                list.Add(new NameValueModel { Name = "180天", Value = "180" });
                list.Add(new NameValueModel { Name = "360天", Value = "360" });
                return list;
            }
        }
        /// <summary>
        /// 红包类型
        /// </summary>
        public static List<NameValueModel> HongBaoType
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "注册红包", Value = "1" });
                list.Add(new NameValueModel { Name = "活动红包", Value = "2" });
                return list;
            }
        }
        /// <summary>
        /// 红包类型Enum
        /// </summary>
        public enum HongBaoTypeEnum
        {
            注册红包 = 1,
            活动红包 = 2,
        }

        #endregion

        #region 积分礼品

        /// <summary>
        ///积分礼品上下架状态
        /// </summary>
        public static List<NameValueModel> ScoreProductStatus
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "上架", Value = "1" });
                list.Add(new NameValueModel { Name = "下架", Value = "0" });
                return list;
            }
        }
        /// <summary>
        /// 积分礼品上下架状态Enum
        /// </summary>
        public enum ScoreProductStatusEnum
        {
            上架 = 1,
            下架 = 0,
        }
        /// <summary>
        /// 用户兑换积分礼品状态
        /// </summary>
        public static List<NameValueModel> UserScoreProductStatus
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "待发货", Value = "0" });
                list.Add(new NameValueModel { Name = "已发货", Value = "1" });
                list.Add(new NameValueModel { Name = "兑换完成", Value = "2" });
                return list;
            }
        }
        /// <summary>
        /// 用户兑换积分礼品状态Enum
        /// </summary>
        public enum UserScoreProductStatusEnum
        {
            待发货 = 0,
            已发货 = 1,
            兑换完成 = 2,
        }

        #endregion

        #region 导入产品价格配置参数

        /// 产品价格配置类型
        /// </summary>
        public static List<NameValueModel> PriceModelType
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "现货", Value = "1" });
                list.Add(new NameValueModel { Name = "UV", Value = "2" });
                list.Add(new NameValueModel { Name = "不干胶", Value = "3" });
                list.Add(new NameValueModel { Name = "白卡纸盒", Value = "4" });
                list.Add(new NameValueModel { Name = "PVC", Value = "5" });
                return list;
            }
        }
        /// <summary>
        /// 是否启用Enum
        /// </summary>
        public enum PriceModelTypeEnum
        {
            现货 = 1,
            UV = 2,
            不干胶 = 3,
            白卡纸盒 = 4,
            PVC = 5
        }

        #endregion

        #region 导航链接

        /// <summary>
        /// 是/否
        /// </summary>
        public static List<NameValueModel> NavigationType
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "主导航", Value = "0" });
                list.Add(new NameValueModel { Name = "推荐分类产品", Value = "1" });
                return list;
            }
        }
        /// <summary>
        /// 是否启用Enum
        /// </summary>
        public enum NavigationTypeEnum
        {
            主导航 = 0,
            推荐分类产品 = 1,
        }

        /// <summary>
        /// 是/否
        /// </summary>
        public static List<NameValueModel> NavigationTarget
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "新窗口", Value = "_blank" });
                list.Add(new NameValueModel { Name = "当前窗口", Value = "_self" });
                return list;
            }
        }

        #endregion

        #region 浏览记录

        /// <summary>
        /// 是/否
        /// </summary>
        public static List<NameValueModel> BrowseRecordType
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "课程", Value = "1" });
                list.Add(new NameValueModel { Name = "分类", Value = "2" });
                list.Add(new NameValueModel { Name = "文章", Value = "3" });
                list.Add(new NameValueModel { Name = "设计作品", Value = "4" });
                list.Add(new NameValueModel { Name = "培训活动", Value = "5" });
                //课程的视频
                list.Add(new NameValueModel { Name = "课程文章", Value = "6" });
                list.Add(new NameValueModel { Name = "PPT", Value = "7" });
                list.Add(new NameValueModel { Name = "运势", Value = "8" });
                list.Add(new NameValueModel { Name = "抽签", Value = "9" });
                list.Add(new NameValueModel { Name = "昵称", Value = "10" });
                return list;
            }
        }
        /// <summary>
        /// 是否启用Enum
        /// </summary>
        public enum BrowseRecordTypeEnum
        {
            课程 = 1,
            分类 = 2,
            文章 = 3,
            设计作品 = 4,
            培训活动 = 5,
            /// <summary>
            /// 即详细视频
            /// </summary>
            课程文章 = 6,
            PPT = 7,
            运势 = 8,
            抽签 = 9,
            昵称 = 10
        }

        #endregion

        #region 提现

        /// <summary>
        /// 提现状态
        /// </summary>
        public static List<NameValueModel> TiXianStatus
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "待处理", Value = "0" });
                list.Add(new NameValueModel { Name = "已审核", Value = "1" });
                list.Add(new NameValueModel { Name = "完成", Value = "2" });
                list.Add(new NameValueModel { Name = "已取消", Value = "3" });
                list.Add(new NameValueModel { Name = "审核不通过", Value = "4" });
                return list;
            }
        }
        /// <summary>
        /// 提现状态 Enum
        /// </summary>
        public enum TiXianStatusEnum
        {
            待处理 = 0,
            /// <summary>
            /// 待支付提现金额
            /// </summary>
            已审核 = 1,
            /// <summary>
            /// 已支付提现金额
            /// </summary>
            完成 = 2,
            已取消 = 3,
            审核不通过 = 4
        }
        #endregion

        #region 课程相关

        /// <summary>
        /// 题型
        /// </summary>
        public static List<NameValueModel> QuestionType
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "单选题", Value = "0" });
                list.Add(new NameValueModel { Name = "多选题", Value = "1" });
                list.Add(new NameValueModel { Name = "判断题", Value = "2" });
                list.Add(new NameValueModel { Name = "问答题", Value = "3" });
                return list;
            }
        }
        /// <summary>
        /// 题型 Enum
        /// </summary>
        public enum QuestionTypeEnum
        {
            单选题 = 0,
            多选题 = 1,
            判断题 = 2,
            问答题 = 3
        }

        /// <summary>
        /// 问题分组
        /// </summary>
        public static List<NameValueModel> QuestionGroup
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "课程测试", Value = "1" });
                list.Add(new NameValueModel { Name = "问卷调查", Value = "2" });
                return list;
            }
        }
        /// <summary>
        /// 问题分组 Enum
        /// </summary>
        public enum QuestionGroupEnum
        {
            课程测试 = 1,
            问卷调查 = 2
        }

        /// <summary>
        /// 课程学习记录状态
        /// </summary>
        public static List<NameValueModel> GoodsArticleRecordState
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "未完成", Value = "0" });
                list.Add(new NameValueModel { Name = "已完成", Value = "1" });
                return list;
            }
        }
        /// <summary>
        /// 课程学习记录状态 Enum
        /// </summary>
        public enum GoodsArticleRecordStateEnum
        {
            未完成 = 0,
            已完成 = 1,
        }
        /// <summary>
        /// 评价类型
        /// </summary>
        public static List<NameValueModel> CommentType
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "课程", Value = "0" });
                list.Add(new NameValueModel { Name = "文章", Value = "1" });
                return list;
            }
        }
        /// <summary>
        /// CommentType Enum
        /// </summary>
        public enum CommentTypeEnum
        {
            课程 = 0,
            文章 = 1,
        }

        #endregion

        #region 标签分类

        /// <summary>
        /// 标签分类
        /// </summary>
        public static List<NameValueModel> TagType
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "团队成员临床经验", Value = "1" });
                list.Add(new NameValueModel { Name = "团队成员所属分类", Value = "2" });
                list.Add(new NameValueModel { Name = "团队成员专长标签", Value = "4" });
                list.Add(new NameValueModel { Name = "学历", Value = "3" });
                return list;
            }
        }
        /// <summary>
        /// 标签分类 Enum
        /// </summary>
        public enum TagTypeEnum
        {
            团队成员临床经验 = 1,
            团队成员所属分类 = 2,
            团队成员专长标签 = 4,
            学历 = 3
        }

        /// <summary>
        /// 团队分类
        /// </summary>
        public static List<NameValueModel> TeamTypes
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "康复医师", Value = "22" });
                list.Add(new NameValueModel { Name = "专家团队", Value = "5" });
                list.Add(new NameValueModel { Name = "医护团队", Value = "6" });
                return list;
            }
        }
        /// <summary>
        /// 团队分类 Enum
        /// </summary>
        public enum TeamTypesEnum
        {
            康复医师 = 22,
            专家团队 = 5,
            医护团队 = 6
        }
        #endregion

        #region 文章类型

        /// <summary>
        /// 文章类型
        /// </summary>
        public static List<NameValueModel> ArticleTypes
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "康复资讯", Value = "1" });
                list.Add(new NameValueModel { Name = "案例见证", Value = "11" });
                list.Add(new NameValueModel { Name = "平台公告", Value = "12" });
                list.Add(new NameValueModel { Name = "滚动图片", Value = "13" });
                list.Add(new NameValueModel { Name = "搜索推荐关键词", Value = "17" });
                list.Add(new NameValueModel { Name = "商品推荐关键词", Value = "18" });
                return list;
            }
        }
        /// <summary>
        /// 文章类型 Enum
        /// </summary>
        public enum ArticleTypeEnum
        {
            康复资讯 = 1,
            案例见证 = 11,
            平台公告 = 12,
            滚动图片 = 13,
            搜索推荐关键词 = 17,
            商品推荐关键词 = 18
        }

        #endregion


        #region 活动报名记录审核状态

        /// <summary>
        /// 活动报名记录审核状态
        /// </summary>
        public static List<NameValueModel> AdvertisementRecordStatus
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "待审核", Value = "0" });
                list.Add(new NameValueModel { Name = "审核通过", Value = "1" });
                list.Add(new NameValueModel { Name = "未通过", Value = "2" });
                return list;
            }
        }
        /// <summary>
        /// 提现状态 Enum
        /// </summary>
        public enum AdvertisementRecordStatusEnum
        {
            待处理 = 0,
            审核通过 = 1,
            未通过 = 2
        }
        #endregion

        #region 管理员角色权限

        /// <summary>
        /// 管理员角色权限
        /// </summary>
        public static List<NameValueModel> RoleLimits
        {
            get
            {
                List<NameValueModel> list = new List<NameValueModel>();
                list.Add(new NameValueModel { Name = "systeminfo/index", Value = "11" });
                list.Add(new NameValueModel { Name = "systeminfo/weixinconfig", Value = "12" });
                list.Add(new NameValueModel { Name = "systeminfo/profitpercentconfig", Value = "13" });
                list.Add(new NameValueModel { Name = "systeminfo/smtpconfig", Value = "14" });
                list.Add(new NameValueModel { Name = "consultmessage/index", Value = "15" });
                list.Add(new NameValueModel { Name = "tag/index", Value = "16" });
                list.Add(new NameValueModel { Name = "department/index", Value = "17" });

                list.Add(new NameValueModel { Name = "manager/adminadd", Value = "21" });
                list.Add(new NameValueModel { Name = "manager/adminlist", Value = "22" });
                list.Add(new NameValueModel { Name = "manager/adminadd/0", Value = "23" });
                list.Add(new NameValueModel { Name = "manager/admindelete", Value = "24" });
                list.Add(new NameValueModel { Name = "manager/adminlogs", Value = "25" });
                list.Add(new NameValueModel { Name = "manager/grouplist", Value = "26" });

                list.Add(new NameValueModel { Name = "user/add", Value = "31" });
                list.Add(new NameValueModel { Name = "user/index", Value = "32" });
                list.Add(new NameValueModel { Name = "user/memberedit/0", Value = "33" });
                list.Add(new NameValueModel { Name = "user/delete", Value = "34" });
                list.Add(new NameValueModel { Name = "user/importuser", Value = "35" });
                list.Add(new NameValueModel { Name = "userrole/index", Value = "36" });
                list.Add(new NameValueModel { Name = "user/tongji", Value = "37" });

                list.Add(new NameValueModel { Name = "goods/add", Value = "41" });
                list.Add(new NameValueModel { Name = "goods/index", Value = "42" });
                list.Add(new NameValueModel { Name = "goods/edit/0", Value = "43" });
                list.Add(new NameValueModel { Name = "goods/delete", Value = "44" });
                list.Add(new NameValueModel { Name = "goodscategory/index", Value = "45" });
                list.Add(new NameValueModel { Name = "goods/goodsarticlerecord", Value = "46" });
                list.Add(new NameValueModel { Name = "comment/index", Value = "47" });
                list.Add(new NameValueModel { Name = "goods/test", Value = "48" });
                list.Add(new NameValueModel { Name = "goods/tongji", Value = "49" });

                list.Add(new NameValueModel { Name = "activity/add", Value = "51" });
                list.Add(new NameValueModel { Name = "activity/index", Value = "52" });
                list.Add(new NameValueModel { Name = "activity/add/0", Value = "53" });
                list.Add(new NameValueModel { Name = "activity/delete", Value = "54" });
                list.Add(new NameValueModel { Name = "advertisementtype/index", Value = "55" });
                list.Add(new NameValueModel { Name = "activity/record", Value = "56" });

                list.Add(new NameValueModel { Name = "designwork/add", Value = "61" });
                list.Add(new NameValueModel { Name = "designwork/index", Value = "62" });
                list.Add(new NameValueModel { Name = "designwork/edit/0", Value = "63" });
                list.Add(new NameValueModel { Name = "designwork/delete", Value = "64" });

                list.Add(new NameValueModel { Name = "questionnaire/add", Value = "71" });
                list.Add(new NameValueModel { Name = "questionnaire/index", Value = "72" });
                list.Add(new NameValueModel { Name = "questionnaire/add/0", Value = "73" });
                list.Add(new NameValueModel { Name = "questionnaire/delete", Value = "74" });
                list.Add(new NameValueModel { Name = "questionnaire/record", Value = "75" });

                list.Add(new NameValueModel { Name = "article/addzuoyeben", Value = "81" });
                list.Add(new NameValueModel { Name = "article/zuoyeben", Value = "82" });
                list.Add(new NameValueModel { Name = "article/addzuoyeben/0", Value = "83" });
                list.Add(new NameValueModel { Name = "article/deletezuoyeben", Value = "84" });

                list.Add(new NameValueModel { Name = "article/addwxnotice", Value = "91" });
                list.Add(new NameValueModel { Name = "article/wxnotice", Value = "92" });
                list.Add(new NameValueModel { Name = "article/addwxnotice/0", Value = "93" });
                list.Add(new NameValueModel { Name = "article/deletewxnotice", Value = "94" });

                list.Add(new NameValueModel { Name = "article/add", Value = "101" });
                list.Add(new NameValueModel { Name = "article/index", Value = "102" });
                list.Add(new NameValueModel { Name = "article/add/0", Value = "103" });
                list.Add(new NameValueModel { Name = "article/delete", Value = "104" });


                return list;
            }
        }
        #endregion
    }
    //==================================下面获取价格配置文件===============================

    #region 获取价格配置文件

    public class XlsPriceData
    {
        //// <summary>
        /// 从Excel提取数据--》Dataset
        /// </summary>
        /// <param name="filename">Excel文件路径名</param>
        public static DataSet GetXlsData(string fileName)
        {
            try
            {
                if (fileName == string.Empty)
                {
                    throw new ArgumentNullException("缺少Excel文件路径！");
                }

                string oleDBConnString = String.Empty;
                oleDBConnString = "Provider=Microsoft.Jet.OLEDB.4.0;";
                oleDBConnString += "Data Source=";
                oleDBConnString += fileName;
                //oleDBConnString += ";Extended Properties=Excel 8.0;";
                oleDBConnString += ";Extended Properties='Excel 8.0;HDR=NO; IMEX=1;'";
                OleDbConnection oleDBConn = null;
                OleDbDataAdapter oleAdMaster = null;
                DataTable m_tableName = new DataTable();
                DataSet ds = new DataSet();

                oleDBConn = new OleDbConnection(oleDBConnString);
                oleDBConn.Open();
                m_tableName = oleDBConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                if (m_tableName != null && m_tableName.Rows.Count > 0)
                {

                    m_tableName.TableName = m_tableName.Rows[0]["TABLE_NAME"].ToString();

                }
                string sqlMaster;
                sqlMaster = " SELECT *  FROM [" + m_tableName.TableName + "A:CV]";
                oleAdMaster = new OleDbDataAdapter(sqlMaster, oleDBConn);
                oleAdMaster.Fill(ds, "m_tableName");
                oleAdMaster.Dispose();
                oleDBConn.Close();
                oleDBConn.Dispose();

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 加载不干胶xls数据到实体
        /// </summary>
        /// <param name="type">1牛皮纸不干胶，2透明不干胶，3涤纶不干胶(金），4普通铜版纸不干胶，5普通铜版纸不干胶覆亮膜，6普通铜版纸不干胶覆哑膜</param>
        /// <returns></returns>
        public static List<PriceResultModel> GetBuGanJiaoListNew(string type = "1")
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + "/xls/bgj_new" + type + ".xls";
            DataSet ds = GetXlsData(fullPath);

            #region 备份201800515-数量单价
            List<PriceResultModel> list = null;
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                list = new List<PriceResultModel>();
                PriceResultModel model = null;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    //数量列不为空，且是数字
                    if (!string.IsNullOrEmpty(dr[4].ToString()) && PageValidate.IsNumber(dr[4].ToString()))
                    {
                        model = new PriceResultModel();

                        model.Type = Convert.ToInt16(DataConfig.PriceModelTypeEnum.不干胶);
                        model.UnitArea = Convert.ToDouble(dr[2]) * Convert.ToDouble(dr[3]) / 1000000;//长MM*宽MM/1000000
                        model.Count = Convert.ToInt32(dr[4]);
                        model.SquareCostPrice = Convert.ToDecimal(Convert.ToDouble(dr[5]) / model.Count / model.UnitArea);
                        model.CostPrice = Convert.ToDecimal(dr[5]);
                        model.Freight = Convert.ToDecimal(dr[6]);
                        model.CostTotalPrice = Convert.ToDecimal(dr[8]);
                        model.ShopPrice = Convert.ToDecimal(dr[10]);
                        model.ShopPriceRate = Math.Round(Convert.ToDouble(dr[10]) / Convert.ToDouble(dr[8]), 2);
                        model.ClientPrice = Convert.ToDecimal(dr[12]);
                        model.ClientPriceRate = Math.Round(Convert.ToDouble(dr[12]) / Convert.ToDouble(dr[10]), 2);
                        model.MinCostPrice = 0;
                        model.MinClientPrice = 0;

                        list.Add(model);
                    }
                }
            }
            #endregion
            return list;
        }

        /// <summary>
        /// 高档UV银卡纸盒
        /// </summary>
        /// <returns></returns>
        public static List<PriceResultModel> GetUVPriceList()
        {
            //string fullPath = AppDomain.CurrentDomain.BaseDirectory + "/xls/uv.xls";
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + "/xls/pf_uv.xls";

            DataSet ds = GetXlsData(fullPath);

            List<PriceResultModel> list = null;
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                list = new List<PriceResultModel>();
                PriceResultModel model = null;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    //数量列不为空，且是数字
                    if (!string.IsNullOrEmpty(dr[0].ToString()) && PageValidate.IsNumber(dr[0].ToString()))
                    {
                        //Log.WriteLog("单条数据" + string.Format("{0},{1},{2},{3},{4}", dr[1], dr[4], dr[9], dr[13], dr[15]), "uv", DateTime.Now.ToString("yyyyMMdd"));
                        model = new PriceResultModel();

                        model.Type = Convert.ToInt16(DataConfig.PriceModelTypeEnum.UV);
                        model.UnitArea = 0;
                        model.Count = Convert.ToInt32(dr[0]);
                        model.SquareCostPrice = Convert.ToDecimal(dr[1]);
                        model.CostPrice = 0;
                        model.Freight = 0;
                        model.CostTotalPrice = 0;
                        model.ShopPriceRate = Convert.ToDouble(dr[2]);
                        model.ShopPrice = 0;
                        model.ClientPriceRate = Convert.ToDouble(dr[4]);
                        model.ClientPrice = 0;
                        model.MinCostPrice = Convert.ToDecimal(dr[7]);
                        model.MinClientPrice = Convert.ToDecimal(dr[8]);

                        list.Add(model);
                    }
                }
            }
            //Log.WriteLog("导入文件：" + fullPath + ",共导入：" + list.Count + "条数据", "buganjiaoNew", DateTime.Now.ToString("yyyyMMdd"));
            return list;
        }

        /// <summary>
        /// 白卡纸盒
        /// </summary>
        /// <param name="type">1:350g,2:400g</param>
        /// <returns></returns>
        public static List<PriceResultModel> GetBaiKaPriceList(int type = 1)
        {
            string fileName = "baika350";
            if (type == 2)
            {
                fileName = "baika400";
            }
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + "/xls/" + fileName + ".xls";

            DataSet ds = GetXlsData(fullPath);

            List<PriceResultModel> list = null;
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                list = new List<PriceResultModel>();
                PriceResultModel model = null;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    //数量列不为空，且是数字
                    if (!string.IsNullOrEmpty(dr[4].ToString()) && PageValidate.IsNumber(dr[4].ToString()))
                    {
                        //Log.WriteLog("单条数据" + string.Format("{0},{1},{2},{3},{4}", dr[1], dr[4], dr[9], dr[13], dr[15]), "baika", DateTime.Now.ToString("yyyyMMdd"));
                        model = new PriceResultModel();

                        double unitAraa = 0;
                        string areaStr = dr[1].ToString();
                        string length = "0";
                        string width = "0";
                        if (areaStr.Contains("*"))
                        {
                            length = areaStr.Split('*')[0];
                            width = areaStr.Split('*')[1];
                        }
                        if (PageValidate.IsNumber(length) && PageValidate.IsNumber(width))
                        {
                            unitAraa = Convert.ToDouble(length) * Convert.ToDouble(width) / 1000000;
                        }
                        model.Type = Convert.ToInt16(DataConfig.PriceModelTypeEnum.白卡纸盒);
                        model.UnitArea = unitAraa;
                        model.Count = Convert.ToInt32(dr[4]);
                        model.SquareCostPrice = Convert.ToDecimal(Convert.ToDouble(dr[3]) / model.Count / model.UnitArea);
                        model.CostPrice = Convert.ToDecimal(dr[3]);
                        model.Freight = Convert.ToInt32(dr[5]);
                        //model.CostPrice = Convert.ToDecimal(Convert.ToDouble(dr[9]) / model.Count / model.UnitArea);
                        model.CostTotalPrice = Convert.ToDecimal(dr[9]);
                        model.ShopPrice = Convert.ToDecimal(dr[13]);
                        model.ShopPriceRate = Math.Round(Convert.ToDouble(dr[13]) / Convert.ToDouble(dr[9]), 2);
                        model.ClientPriceRate = Math.Round(Convert.ToDouble(dr[15]) / Convert.ToDouble(dr[13]), 2);
                        model.ClientPrice = Convert.ToDecimal(dr[15]);
                        model.MinCostPrice = 0;
                        model.MinClientPrice = 0;

                        list.Add(model);
                    }
                }
            }

            #region 从配置充取最低成本和终端价格

            PriceResultModel minPriceModel = list.OrderBy(m => m.CostPrice).First();
            foreach (var item in list)
            {
                if (minPriceModel != null)
                {
                    item.MinCostPrice = minPriceModel.CostPrice;
                    item.MinClientPrice = minPriceModel.ClientPrice;
                }
            }
            #endregion

            //Log.WriteLog("导入文件：" + fullPath + ",共导入：" + list.Count + "条数据", "buganjiaoNew", DateTime.Now.ToString("yyyyMMdd"));
            return list;
        }

        /// <summary>
        /// 加载PVC异形卡xls数据到实体
        /// </summary>
        /// <param name="type">材质类型：PVC亮光卡(lg)，PVC哑光卡(yg)，PVC磨砂卡(ms)</param>
        /// <returns></returns>
        public static List<PriceResultModel> GetPVCPriceList(string type = "lg")
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + "/xls/pvc_" + type + ".xls";
            DataSet ds = GetXlsData(fullPath);

            List<PriceResultModel> list = null;
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                list = new List<PriceResultModel>();
                PriceResultModel model = null;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    //数量列不为空，且是数字
                    if (!string.IsNullOrEmpty(dr[6].ToString()) && PageValidate.IsNumber(dr[6].ToString()))
                    {
                        //Log.WriteLog("单挑数据" + string.Format("{0},{1},{2},{3},{4},{5}", dr[5], dr[6], dr[7], dr[8], dr[16], dr[17]), "pvc", DateTime.Now.ToString("yyyyMMdd"));
                        model = new PriceResultModel();

                        model.Type = Convert.ToInt16(DataConfig.PriceModelTypeEnum.PVC);
                        model.UnitArea = Convert.ToDouble(dr[5]);
                        model.Count = Convert.ToInt32(dr[6]);
                        model.SquareCostPrice = Convert.ToDecimal(Convert.ToDouble(dr[7]));
                        model.CostPrice = Convert.ToDecimal(dr[8]);
                        model.Freight = 0;
                        model.CostTotalPrice = Convert.ToDecimal(dr[8]);
                        model.ShopPrice = Convert.ToDecimal(dr[11]);
                        model.ShopPriceRate = Math.Round(Convert.ToDouble(dr[9]), 2);
                        model.ClientPrice = Convert.ToDecimal(dr[16]);
                        model.ClientPriceRate = Math.Round(Convert.ToDouble(dr[13]), 2);
                        model.MinCostPrice = Convert.ToDecimal(dr[17]);
                        model.MinClientPrice = Convert.ToDecimal(dr[17]);

                        list.Add(model);
                    }
                }
            }
            //Log.WriteLog("导入文件：" + fullPath + ",共导入：" + list.Count + "条数据", "buganjiaoNew", DateTime.Now.ToString("yyyyMMdd"));
            return list;
        }
    }

    #endregion
}
