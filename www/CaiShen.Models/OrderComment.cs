using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    /// <summary>
    /// 订购评价
    /// </summary>
    public class OrderComment
    {
        public OrderComment()
        {
            this.OC_IsDelete = 0;
            this.OC_CreateTime = DateTime.Now;
            this.OC_IsHiddenName = 0;
            this.OC_Status = 1;
            this.OC_ScoreShipping = 0;
            this.OC_ScoreShop = 0;
            this.OC_ScoreGoods = 5;
        }
        public int ID { get; set; }


        [Display(Name = "评价时间")]
        public DateTime OC_CreateTime { get; set; }

        [Display(Name = "买家")]
        public int UserID { get; set; }

        /// <summary>
        /// 订单产品详情ID
        /// </summary>
        [Display(Name = "订单详情ID")]
        public int OrderDetailID { get; set; }

        [StringLength(200)]
        [Display(Name = "评价商品")]
        public string OC_Content { get; set; }

        [StringLength(200)]
        [Display(Name = "评价服务")]
        public string OC_ContentService { get; set; }

        [StringLength(1000)]
        [Display(Name = "晒图片")]
        public string OC_Images { get; set; }

        [StringLength(200)]
        [Display(Name = "回复内容")]
        public string OC_ReplyContent { get; set; }

        /// <summary>
        /// 店铺评分，1-5分
        /// </summary>
        [Display(Name = "店铺评分")]
        public int OC_ScoreShop { get; set; }

        /// <summary>
        /// 产品评分，1-5分
        /// </summary>
        [Display(Name = "产品评分")]
        public int OC_ScoreGoods { get; set; }

        /// <summary>
        /// 物流评分，1-5分
        /// </summary>
        [Display(Name = "物流评分")]
        public int OC_ScoreShipping { get; set; }

        /// <summary>
        /// 是否匿名
        /// </summary>
        [Display(Name = "是否匿名")]
        public int OC_IsHiddenName { get; set; }

        [Display(Name = "是否删除")]
        public int OC_IsDelete { get; set; }

        /// <summary>
        /// 默认1:已审核（显示），0：未审核（不显示）
        /// </summary>
        [Display(Name = "审核状态")]
        public int OC_Status { get; set; }
    }
    public class OrderCommentVModel
    {
        /// <summary>
        /// 订单产品详情ID
        /// </summary>
        public int OrderDetailID { get; set; }
        public string OC_Content { get; set; }
        public string OC_Images { get; set; }
        /// <summary>
        /// 产品评分，1-5分
        /// </summary>
        public int OC_ScoreGoods { get; set; }
    }
    public class OrderCommentListVModel
    {
        public OrderComment OrderComment { get; set; }
        public User User { get; set; }
        public UserShop UserShop { get; set; }
    }
    public class OrderCommentClient
    {
        public string Content { get; set; }
        public List<string> Photos { get; set; }
        public string ReplyContent { get; set; }
        public string NickName { get; set; }
        public string Time { get; set; }
        public string UserImg { get; set; }
        public int ScoreGoods { get; set; }
    }
}
