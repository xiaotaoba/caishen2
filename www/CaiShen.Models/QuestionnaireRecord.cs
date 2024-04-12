using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    /// <summary>
    /// 问卷调查记录
    /// </summary>
    public class QuestionnaireRecord
    {
        public QuestionnaireRecord()
        {
            this.CreateTime = DateTime.Now;
        }
        public int ID { get; set; }

        /// <summary>
        /// 问卷ID
        /// </summary>
        [Display(Name = "问卷ID")]
        public int Quest_ID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [Display(Name = "用户ID")]
        public int UserID { get; set; }

        /// <summary>
        /// 得分
        /// </summary>
        [Display(Name = "得分")]
        public int Score { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [Display(Name = "开始时间")]
        public DateTime CreateTime { get; set; }
    }

    public class QuestionnaireRecordVModel
    {
        public string Quest_Title { get; set; }
        /// <summary>
        ///用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// 部门/身份
        /// </summary>
        public int Department { get; set; }
        /// <summary>
        /// 地区-省
        /// </summary>
        public int Province  { get; set; }
        /// <summary>
        /// 地区-市
        /// </summary>
        public int City { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public QuestionnaireRecord QuestionnaireRecord { get; set; }
        /// <summary>
        /// 门店
        /// </summary>
        public string ShopName { get; set; }
    }
}
