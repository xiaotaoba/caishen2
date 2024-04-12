using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    public class UserScoreHistory
    {
        /// <summary>
        /// 会员积分变动历史记录
        /// </summary>
        public UserScoreHistory()
        {
            this.Is_Delete = 0;
            this.Type = 1;
        }
        public int ID { get; set; }

        [Display(Name = "会员ID")]
        public int UserID { get; set; }

        public virtual User User { get; set; }

        [StringLength(50)]
        [Display(Name = "事项")]
        public string Thing { get; set; }

        [StringLength(20)]
        [Display(Name = "分类")]
        public string Category { get; set; }

        /// <summary>
        /// 剩余积分
        /// </summary>
        [Display(Name = "剩余积分")]
        public int TotalScore { get; set; }

        /// <summary>
        /// 变动积分
        /// </summary>
        [Display(Name = "变动积分")]
        public int Score { get; set; }

        /// <summary>
        /// 剩余锁定积分
        /// </summary>
        [Display(Name = "剩余锁定积分")]
        public int LockTotalScore { get; set; }

        /// <summary>
        /// 变动锁定积分
        /// </summary>
        [Display(Name = "变动锁定积分")]
        public int LockScore { get; set; }

        [Display(Name = "时间")]
        public DateTime Time { get; set; }


        [Display(Name = "是否删除")]
        public int Is_Delete { get; set; }

        /// <summary>
        /// 1增加，0减少
        /// </summary>
        [Display(Name = "类型")]
        public int Type { get; set; }

        [Display(Name = "关联记录ID")]
        public int RecordID { get; set; }

        [StringLength(50)]
        [Display(Name = "操作人")]
        public string Operator { get; set; }
    }
}
