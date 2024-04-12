using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pannet.Models
{
    public class ManagerLog
    {
        public ManagerLog()
        {
            this.ML_CreateTime = DateTime.Now;
        }

        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "操作人")]
        public string ML_UserName { get; set; }

        [Display(Name = "操作人ID")]
        [Required]
        public int ManagerID { get; set; }

        [StringLength(12)]
        [Display(Name = "记录ID")]
        public string ML_ItemID { get; set; }

        [StringLength(20)]
        [Display(Name = "IP地址")]
        public string ML_IP { get; set; }

        [StringLength(300)]
        [Display(Name = "操作内容")]
        public string ML_Content { get; set; }

        public DateTime ML_CreateTime { get; set; }

        public virtual  Manager Manager  { get; set; }
    }
}
