using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    /// <summary>
    /// 用于活动
    /// </summary>
    public class Advertisement
    {
        public Advertisement()
        {
            this.AD_IsEnable = 1;
            this.AD_Sort = 0;
            this.AdvertisementTypeID = 0;
            this.AD_BeginTime = DateTime.Now;
            this.AD_EndTime = DateTime.Now.AddYears(1);
            this.AD_CreateTime = DateTime.Now;
            this.AD_ActivityBeginTime = DateTime.Now.AddDays(1);
            this.AD_ActivityEndTime = DateTime.Now.AddDays(2);

        }
        public int ID { get; set; }

        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "标题由2-20个字符组成")]
        [Display(Name = "标题")]
        public string AD_Title { get; set; }

        [Display(Name = "活动介绍")]
        public string AD_Desc { get; set; }

        [Display(Name = "是否显示")]
        public int AD_IsEnable { get; set; }

        [Display(Name = "分类")]
        public int AdvertisementTypeID { get; set; }

        [StringLength(200)]
        [Display(Name = "图片")]
        public string AD_Image { get; set; }

        [StringLength(200)]
        [Display(Name = "链接URL")]
        public string AD_URL { get; set; }

        /// <summary>
        /// 报名开始时间
        /// </summary>
        [Display(Name = "报名开始时间")]
        public DateTime AD_BeginTime { get; set; }

        /// <summary>
        /// 报名截止时间
        /// </summary>
        [Display(Name = "报名截止时间")]
        public DateTime AD_EndTime { get; set; }

        /// <summary>
        /// 活动开始时间
        /// </summary>
        [Display(Name = "活动开始时间")]
        public DateTime AD_ActivityBeginTime { get; set; }
        /// <summary>
        /// 活动结束时间
        /// </summary>
        [Display(Name = "活动结束时间")]
        public DateTime AD_ActivityEndTime { get; set; }

        [Display(Name = "创建时间")]
        public DateTime AD_CreateTime { get; set; }

        [Display(Name = "排序号")]
        public int AD_Sort { get; set; }

        /// <summary>
        /// 浏览/点击次数
        /// </summary>
        [Display(Name = "点击次数")]
        public int AD_Click { get; set; }

        /// <summary>
        /// 活动状态
        /// </summary>
        [Display(Name = "状态")]
        public int AD_State { get; set; }

        /// <summary>
        /// 活动参与部门-其他部门不用参与，0:所有人可参与
        /// </summary>
        [Display(Name = "活动参与部门")]
        public int AD_DepartmentID { get; set; }

        /// <summary>
        /// 是否发送微信模板消息
        /// </summary>
        [Display(Name = "是否发送微信模板消息")]
        public int AD_IsSendWxMessage { get; set; }

        [StringLength(50)]
        [Display(Name = "导语")]
        public string AD_WX_First { get; set; }

        [StringLength(50)]
        [Display(Name = "结束语")]
        public string AD_WX_Remark { get; set; }

        [StringLength(50)]
        [Display(Name = "活动地址")]
        public string AD_WX_Address { get; set; }

        [StringLength(50)]
        [Display(Name = "活动简介")]
        public string AD_WX_Content { get; set; }

        /// <summary>
        /// 发送员工所属部门
        /// </summary>
        [Display(Name = "状态")]
        public int AD_WX_DepartmentID { get; set; }
    }

    public class AdvertisementVModel
    {
        public Advertisement Advertisement { get; set; }
        public AdvertisementType AdvertisementType { get; set; }
    }
}
