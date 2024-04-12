using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    public class ScoreProduct
    {
        public ScoreProduct()
        {
            this.SP_Count = 0;
            this.SP_IsRecommend = 0;
            this.SP_Status = 1;
            this.SP_CreateTime = DateTime.Now;
            this.SP_IsFreeShipping = 1;
            this.SP_PostFee = 0;
            this.UserShopID = 0;
            this.SP_Sort = 0;
            this.SP_ShowTimes = 0;
        }
        public int ID { get; set; }

        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "{0}由{2}-{1}个字符组成")]
        [Display(Name = "积分礼品名称")]
        public string SP_Name { get; set; }

        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50)]
        [Display(Name = "礼品编号")]
        public string SP_Number { get; set; }

        [Display(Name = "礼品描述")]
        public string SP_Desc { get; set; }

        /// <summary>
        ///兑换所需积分
        /// </summary>
        [Display(Name = "兑换所需积分")]
        public int SP_Score { get; set; }

        /// <summary>
        /// 可兑换数量
        /// </summary>
        [Display(Name = "可兑换数量")]
        public int SP_Count { get; set; }
        /// <summary>
        /// 1上架 0下架
        /// </summary>
        [Display(Name = "上架状态")]
        public int SP_Status { get; set; }
        [Display(Name = "推荐")]
        public int SP_IsRecommend { get; set; }

        [StringLength(300)]
        [Display(Name = "展示图片")]
        public string SP_Image { get; set; }

        [Display(Name = "排序号")]
        public int SP_Sort { get; set; }

        [Display(Name = "创建时间")]
        public DateTime SP_CreateTime { get; set; }

        /// <summary>
        /// 浏览次数
        /// </summary>
        [Display(Name = "浏览次数")]
        public int SP_ShowTimes { get; set; }

        /// <summary>
        /// 默认0:平台，是否为门店自定义礼品
        /// </summary>
        [Display(Name = "所属门店")]
        public int UserShopID { get; set; }

        /// <summary>
        /// 是否免邮费（包邮）,默认包邮
        /// </summary>
        [Display(Name = "包邮")]
        public int SP_IsFreeShipping { get; set; }
        [Display(Name = "邮费价格")]
        public decimal SP_PostFee { get; set; }

    }
}
