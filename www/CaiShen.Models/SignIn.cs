using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pannet.Models
{
    /// <summary>
    /// 签到
    /// </summary>
    public class SignIn
    {
        public SignIn()
        {
            this.Sign_CreateTime = DateTime.Now;
        }

        public int ID { get; set; }

        [Display(Name = "操作人ID")]
        [Required]
        public int UserID { get; set; }

        [StringLength(12)]
        [Display(Name = "记录ID")]
        public string Sign_ItemID { get; set; }

        [StringLength(50)]
        [Display(Name = "IP地址")]
        public string Sign_IP { get; set; }

        [StringLength(300)]
        [Display(Name = "操作内容")]
        public string Sign_Content { get; set; }

        public DateTime Sign_CreateTime { get; set; }

    }
}
