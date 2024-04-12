using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    public class UserLevelHistory
    {
        /// <summary>
        /// 会员等级变动历史记录
        /// </summary>
        public UserLevelHistory()
        {
        }
        public int ID { get; set; }

        [Display(Name = "会员ID")]
        public int UserID { get; set; }

        public virtual User User { get; set; }

        [StringLength(50)]
        [Display(Name = "变动说明")]
        public string Title { get; set; }

        [StringLength(50)]
        [Display(Name = "操作人")]
        public string Operator { get; set; }

        [Display(Name = "变动前级别")]
        public int Prev_Level_ID { get; set; }

        [Display(Name = "变动后级别")]
        public int Current_Level_ID { get; set; }

        [Display(Name = "操作时间")]
        public DateTime Time { get; set; }

    }
}
