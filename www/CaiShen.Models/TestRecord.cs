using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    /// <summary>
    /// 测试问题选择的答案记录
    /// </summary>
    public class TestRecord
    {
        public TestRecord()
        {
        }
        public int ID { get; set; }

        /// <summary>
        /// 关联课程
        /// </summary>
        [Display(Name = "关联课程ID")]
        public int GoodsID { get; set; }

        /// <summary>
        /// 关联测试
        /// </summary>
        [Display(Name = "关联测试ID")]
        public int TestID { get; set; }

        /// <summary>
        /// 关联问题
        /// </summary>
        [Display(Name = "关联问题ID")]
        public int QuestionID { get; set; }

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
        public string T_Answer { get; set; }

        /// <summary>
        /// 选择或输入答案ID,英文“,”隔开
        /// </summary>
        [StringLength(100)]
        [Display(Name = "选择答案ID")]
        public string T_AnswerIds { get; set; }

        /// <summary>
        /// 是否正确（做对），默认0否，1正确
        /// </summary>
        [Display(Name = "是否正确")]
        public int T_IsTrue { get; set; }

    }
}
