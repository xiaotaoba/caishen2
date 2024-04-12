using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    /// <summary>
    /// 测试问题选项（答案）
    /// </summary>
    public class Answer
    {
        public Answer()
        {
            this.A_Sort = 0;
            this.A_IsTrue = 0;
        }
        public int ID { get; set; }

        /// <summary>
        /// 选项
        /// </summary>
        [StringLength(100)]
        [Display(Name = "选项")]
        public string A_Answer { get; set; }

        /// <summary>
        /// 关联问题
        /// </summary>
        [Display(Name = "关联问题ID")]
        public int QuestionID { get; set; }

        /// <summary>
        /// 是否正确答案，默认0否，1正确
        /// </summary>
        [Display(Name = "是否正确选项")]
        public int A_IsTrue { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        [Display(Name = "排序号")]
        public int A_Sort { get; set; }
    }
    public class AnswerVModel
    {
        public AnswerVModel()
        {
            this.sort = 0;
            this.istrue = 0;
        }
        public string answer { get; set; }
        public int istrue { get; set; }
        public int sort { get; set; }
        public int id { get; set; }
    }
}
