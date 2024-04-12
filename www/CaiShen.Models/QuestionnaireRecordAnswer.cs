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
    public class QuestionnaireRecordAnswer
    {
        public QuestionnaireRecordAnswer()
        {
        }
        public int ID { get; set; }

        /// <summary>
        /// 问卷ID
        /// </summary>
        [Display(Name = "问卷ID")]
        public int Quest_ID { get; set; }

        /// <summary>
        /// 关联问题
        /// </summary>
        [Display(Name = "关联问题ID")]
        public int QuestionID { get; set; }

        /// <summary>
        /// 调查记录ID
        /// </summary>
        [Display(Name = "调查记录ID")]
        public int QuestionnaireRecordID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [Display(Name = "用户ID")]
        public int UserID { get; set; }

        /// <summary>
        /// 选择或输入答案，“|”隔开
        /// </summary>
        [StringLength(300)]
        [Display(Name = "选择答案")]
        public string Answer { get; set; }

        /// <summary>
        /// 选择或输入答案ID,英文“,”隔开
        /// </summary>
        [StringLength(100)]
        [Display(Name = "选择答案ID")]
        public string AnswerIds { get; set; }

    }
}
