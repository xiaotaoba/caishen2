using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    public class ShippingArea
    {
        /// <summary>
        /// 配送区域及计价方式
        /// </summary>
        public ShippingArea()
        {
            this.SA_Is_Enable = 1;
            //this.UserID = 0;
            //this.SA_UserType = 1;
            this.ShippingTemplateID = 0;
            //包邮
            this.SA_IsFree = 0;
            this.SA_IsCountFree = 0;
            this.SA_CountFree = 0;
            this.SA_IsWeightFree = 0;
            this.SA_WeightFree = 0;
            this.SA_IsVolumeFree = 0;
            this.SA_VolumeFree = 0;
            this.SA_IsPriceFree = 0;
            this.SA_PriceFree = 0;
            //体积
            this.SA_VolumeFirst = 1;
            this.SA_VolumeFirstFee = 0;
            this.SA_ReVolume = 0;
            this.SA_ReVolumeFee = 0;
            this.SA_VolumeMinFee = 0;
            //重量
            this.SA_WeightFirst = 1;
            this.SA_WeightFirstFee = 0;
            this.SA_ReWeight = 0;
            this.SA_ReWeightFee = 0;
            this.SA_WeightMinFee = 0;
            //快递
            this.SA_ExpressFirst = 1;
            this.SA_ExpressFirstFee = 0;
            this.SA_ReExpress = 0;
            this.SA_ReExpressFee = 0;
            //件数
            this.SA_CountFirst = 1;
            this.SA_CountFirstFee = 0;
            this.SA_ReCount = 0;
            this.SA_ReCountFee = 0;
            //提货/送货
            this.SA_WeightTihuoFee = 0;
            this.SA_WeightTihuoFree = 0;
            this.SA_VolumeTihuoFee = 0;
            this.SA_VolumeTihuoFree = 0;
            this.SA_WeightSonghuoFee = 0;
            this.SA_WeightSonghuoFree = 0;
            this.SA_VolumeSonghuoFee = 0;
            this.SA_VolumeSonghuoFree = 0;


            this.SA_Sort = 0;
            this.SA_DayCount = 1;
            this.SA_Is_Default = 0;
        }
        public int ID { get; set; }

        /// <summary>
        /// 配送地区标题
        /// </summary>
        [StringLength(50)]
        [Display(Name = "配送地区标题")]
        [Required(ErrorMessage = "请输入{0}")]
        public string SA_Title { get; set; }

        [Display(Name = "运费模板")]
        public int ShippingTemplateID { get; set; }

        /// <summary>
        /// 逗号隔开的area_id
        /// </summary>
        [Display(Name = "运送地区")]
        public string AreaIds { get; set; }

        #region 包邮相关

        [Display(Name = "是否包邮")]
        public int SA_IsFree { get; set; }

        /// <summary>
        /// 按件数包邮
        /// </summary>
        [Display(Name = "按件数包邮")]
        public int SA_IsCountFree { get; set; }

        /// <summary>
        /// 包邮件数
        /// </summary>
        [Display(Name = "包邮件数")]
        public int SA_CountFree { get; set; }

        /// <summary>
        /// 按重量包邮
        /// </summary>
        [Display(Name = "按重量包邮")]
        public int SA_IsWeightFree { get; set; }

        /// <summary>
        /// 包邮重量
        /// </summary>
        [Display(Name = "包邮重量")]
        public double SA_WeightFree { get; set; }

        [Display(Name = "按体积包邮")]
        public int SA_IsVolumeFree { get; set; }

        /// <summary>
        /// 包邮体积
        /// </summary>
        [Display(Name = "包邮体积")]
        public double SA_VolumeFree { get; set; }

        /// <summary>
        /// 按价格包邮
        /// </summary>
        [Display(Name = "按价格包邮")]
        public int SA_IsPriceFree { get; set; }

        /// <summary>
        /// 包邮价格
        /// </summary>
        [Display(Name = "包邮价格")]
        public decimal SA_PriceFree { get; set; }

        #endregion

        #region 按件数计费

        /// <summary>
        /// 首件,单位件
        /// </summary>
        [Display(Name = "首件")]
        public int SA_CountFirst { get; set; }
        /// <summary>
        /// 首件费用
        /// </summary>
        [Display(Name = "首件费用")]
        public decimal SA_CountFirstFee { get; set; }
        /// <summary>
        /// 续件
        /// </summary>
        [Display(Name = "续件")]
        public int SA_ReCount { get; set; }
        /// <summary>
        /// 续件费用
        /// </summary>
        [Display(Name = "续件费用")]
        public decimal SA_ReCountFee { get; set; }

        #endregion

        #region 按重量计费

        /// <summary>
        /// 首重 ,单位KG
        /// </summary>
        [Display(Name = "首重")]
        public double SA_WeightFirst { get; set; }
        /// <summary>
        /// 首重费用
        /// </summary>
        [Display(Name = "首重费用")]
        public decimal SA_WeightFirstFee { get; set; }
        /// <summary>
        /// 续重 ,单位KG
        /// </summary>
        [Display(Name = "续重")]
        public double SA_ReWeight { get; set; }
        /// <summary>
        /// 续重费用
        /// </summary>
        [Display(Name = "续重费用")]
        public decimal SA_ReWeightFee { get; set; }

        /// <summary>
        /// 重量起步价（最低费用）
        /// </summary>
        [Display(Name = "重量起步价")]
        public decimal SA_WeightMinFee { get; set; }

        #endregion

        #region 按体积计费

        /// <summary>
        /// 首体积,立方米
        /// </summary>
        [Display(Name = "首体积")]
        public double SA_VolumeFirst { get; set; }

        /// <summary>
        /// 首体积费用
        /// </summary>
        [Display(Name = "首体积费用")]
        public decimal SA_VolumeFirstFee { get; set; }
        /// <summary>
        /// 续体积,立方米
        /// </summary>
        [Display(Name = "续体积")]
        public double SA_ReVolume { get; set; }

        /// <summary>
        /// 续体积费用
        /// </summary>
        [Display(Name = "续体积费用")]
        public decimal SA_ReVolumeFee { get; set; }

        /// <summary>
        /// 体积起步价（最低费用）
        /// </summary>
        [Display(Name = "体积起步价")]
        public decimal SA_VolumeMinFee { get; set; }

        #endregion

        #region 快递计费

        /// <summary>
        /// 首重 ,单位KG
        /// </summary>
        [Display(Name = "首重")]
        public double SA_ExpressFirst { get; set; }
        /// <summary>
        /// 首重费用
        /// </summary>
        [Display(Name = "首重费用")]
        public decimal SA_ExpressFirstFee { get; set; }
        /// <summary>
        /// 续重 ,单位KG
        /// </summary>
        [Display(Name = "续重")]
        public double SA_ReExpress { get; set; }
        /// <summary>
        /// 续重费用
        /// </summary>
        [Display(Name = "续重费用")]
        public decimal SA_ReExpressFee { get; set; }

        #endregion

        #region 物流提货、送货计费

        /// <summary>
        /// 重量-提货费
        /// </summary>
        [Display(Name = "提货费")]
        public decimal SA_WeightTihuoFee { get; set; }
        /// <summary>
        /// 重量-多少免提货费
        /// </summary>
        [Display(Name = "多少免提货费")]
        public decimal SA_WeightTihuoFree { get; set; }
        /// <summary>
        /// 重量-送货费
        /// </summary>
        [Display(Name = "送货费")]
        public decimal SA_WeightSonghuoFee { get; set; }
        /// <summary>
        /// 重量-多少免送货费
        /// </summary>
        [Display(Name = "多少免送货费")]
        public decimal SA_WeightSonghuoFree { get; set; }

        /// <summary>
        /// 体积-提货费
        /// </summary>
        [Display(Name = "提货费")]
        public decimal SA_VolumeTihuoFee { get; set; }
        /// <summary>
        /// 体积-多少免提货费
        /// </summary>
        [Display(Name = "多少免提货费")]
        public decimal SA_VolumeTihuoFree { get; set; }
        /// <summary>
        /// 体积-送货费
        /// </summary>
        [Display(Name = "送货费")]
        public decimal SA_VolumeSonghuoFee { get; set; }
        /// <summary>
        /// 体积-多少免送货费
        /// </summary>
        [Display(Name = "多少免送货费")]
        public decimal SA_VolumeSonghuoFree { get; set; }

        #endregion


        /// <summary>
        /// 预计配送天数
        /// </summary>
        [Display(Name = "预计配送天数")]
        public int SA_DayCount { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [Display(Name = "是否启用")]
        public int SA_Is_Enable { get; set; }

        /// <summary>
        /// 默认为，未设置地区 使用的计费方式
        /// </summary>
        [Display(Name = "是否默认")]
        public int SA_Is_Default { get; set; }

        /// <summary>
        /// 排序号(优先级)
        /// </summary>
        [Display(Name = "排序号(优先级)")]
        public int SA_Sort { get; set; }
    }
}
