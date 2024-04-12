using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pannet.Models
{
    public class UserLog
    {
        public UserLog()
        {
            this.UL_CreateTime = DateTime.Now;
            this.UL_ItemID = "0";
        }

        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "操作人")]
        public string UL_UserName { get; set; }

        [Display(Name = "操作人ID")]
        [Required]
        public int UserID { get; set; }

        [StringLength(12)]
        [Display(Name = "记录ID")]
        public string UL_ItemID { get; set; }

        [StringLength(20)]
        [Display(Name = "IP地址")]
        public string UL_IP { get; set; }

        [StringLength(300)]
        [Display(Name = "操作内容")]
        public string UL_Content { get; set; }

        public DateTime UL_CreateTime { get; set; }

    }
}
