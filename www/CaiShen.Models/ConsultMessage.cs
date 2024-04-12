using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    /// <summary>
    /// 咨询产品留言
    /// </summary>
    public class ConsultMessage
    {
        public ConsultMessage()
        {
            this.CreateTime = DateTime.Now;
        }
        public int ID { get; set; }

        [StringLength(50)]
        [Display(Name = "标题")]
        public string Title { get; set; }

        [StringLength(50)]
        [Display(Name = "联系人")]
        public string UserName { get; set; }

        [StringLength(50)]
        [Display(Name = "联系方式")]
        public string Tel { get; set; }

        [StringLength(50)]
        [Display(Name = "联系地址")]
        public string Address { get; set; }

        [StringLength(200)]
        [Display(Name = "反馈结果")]
        public string Remark { get; set; }

        /// <summary>
        /// 培训需求
        /// </summary>
        [StringLength(200)]
        [Display(Name = "培训需求")]
        public string Content { get; set; }

        [Display(Name = "是否联系")]
        public int IsContact { get; set; }

        [Display(Name = "用户")]
        public int UserID { get; set; }

        /// <summary>
        /// 0未设置 1男 2女 3保密
        /// </summary>
        [Display(Name = "性别")]
        public int Gender { get; set; }

        [StringLength(50)]
        [Display(Name = "公司名称")]
        public string ShopName { get; set; }

        [Display(Name = "店铺")]
        public int UserShopID { get; set; }

        /// <summary>
        /// 提交时间
        /// </summary>
        [Display(Name = "提交时间")]
        public DateTime CreateTime { get; set; }


    }
}
