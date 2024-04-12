using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    /// <summary>
    /// 活动报名记录
    /// </summary>
    public class AdvertisementRecord
    {
        public AdvertisementRecord()
        {
            this.UserID = 0;
            this.ADR_CreateTime = DateTime.Now;
            this.ADR_State = 0;
        }
        public int ID { get; set; }

        public int UserID { get; set; }

        /// <summary>
        /// 活动ID
        /// </summary>
        public int AdvertisementID { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [StringLength(50)]
        [Display(Name = "姓名")]
        public string ADR_UserName { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [StringLength(50)]
        [Display(Name = "电话")]
        public string ADR_Tel { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [StringLength(100)]
        [Display(Name = "地址")]
        public string ADR_Address { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        [StringLength(50)]
        [Display(Name = "职位")]
        public string ADR_Position { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(200)]
        [Display(Name = "备注")]
        public string ADR_Remark { get; set; }

        [Display(Name = "创建时间")]
        public DateTime ADR_CreateTime { get; set; }

        /// <summary>
        /// 审核状态0待审核，1审核通过，2审核不通过
        /// </summary>
        public int ADR_State { get; set; }
    }
    public class AdvertisementRecordVModel
    {
        public AdvertisementRecord AdvertisementRecord { get; set; }
        /// <summary>
        /// 活动名称
        /// </summary>
        public string Title { get; set; }
    }
}
