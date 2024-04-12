using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    /// <summary>
    /// 测试或调查问题
    /// </summary>
    public class Question
    {
        public Question()
        {
            this.Q_CreateTime = DateTime.Now;
            this.Q_Sort = 0;
            this.Q_IsTrue = 0;
            this.Q_Type = 0;
            this.Q_Group = 1;
            this.Q_GroupItemID = 0;
            this.Q_GroupItemSubID = 0;
        }
        public int ID { get; set; }

        [StringLength(100)]
        [Display(Name = "问题")]
        public string Q_Title { get; set; }

        /// <summary>
        ///  题型:单选题0, 多选题1 ,判断题2 ,问答题3
        /// </summary>
        [Display(Name = "题型")]
        public int Q_Type { get; set; }

        /// <summary>
        /// 问题分组：默认1课程测试，2：问卷调查
        /// </summary>
        [Display(Name = "问题分组")]
        public int Q_Group { get; set; }

        /// <summary>
        /// Q_Group为1课程测试，Q_GroupItemID对应课程ID；Q_Group为2问卷调查，Q_GroupItemID对应问卷ID
        /// </summary>
        [Display(Name = "事项ID")]
        public int Q_GroupItemID { get; set; }

        /// <summary>
        /// 目前用于 对应课程目录ID
        /// </summary>
        [Display(Name = "子事项ID")]
        public int Q_GroupItemSubID { get; set; }


        /// <summary>
        /// 如果是判断题或问答题，答案直接写入question表，其他单选，或多选类型答案放在answer表
        /// 默认0未设置（非对错题），1正确，2错误
        /// </summary>
        [Display(Name = "是否正确")]
        public int Q_IsTrue { get; set; }

        [Display(Name = "排序号")]
        public int Q_Sort { get; set; }

        /// <summary>
        /// 参考答案，只有题型为：问答题有效
        /// </summary>
        [StringLength(1000)]
        [Display(Name = "参考答案")]
        public string Q_Answer { get; set; }

        /// <summary>
        /// 问题解析
        /// </summary>
        [StringLength(1000)]
        [Display(Name = "问题解析")]
        public string Q_Analysis { get; set; }

        [StringLength(1000)]
        [Display(Name = "问题描述")]
        public string Q_Description { get; set; }

        public DateTime Q_CreateTime { get; set; }

    }
    public class QuestionAnswerVModel
    {
        public Question Question { get; set; }
        public IEnumerable<Answer> AnswerList { get; set; }

    }
    public class RequestTestQuestionVModel
    {
        /// <summary>
        /// Question ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 问题类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 答案，如果是多选：逗号隔开ID，如:id1,id2,id3
        /// </summary>
        public string Answer { get; set; }
    }
    public class ResponseTestQuestionVModel
    {
        /// <summary>
        /// Question ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 问题类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 答案，如果是多选：逗号隔开ID，如:id1,id2,id3
        /// </summary>
        public string Answer { get; set; }
        /// <summary>
        /// 测试结果，1：对，0：错，
        /// </summary>
        public int IsTrue { get; set; }
        /// <summary>
        ///选项测试结果集
        /// </summary>
        public List<ResponseAnswerVModel> AnswerRst { get; set; }
    }
    public class ResponseAnswerVModel
    {
        /// <summary>
        /// Answer ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 是否选择
        /// </summary>
        public int IsSelect { get; set; }
        /// <summary>
        /// 测试结果，1：对，0：错，
        /// </summary>
        public int IsTrue { get; set; }
    }
}
