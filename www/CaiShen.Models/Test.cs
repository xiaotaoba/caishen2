using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    /// <summary>
    /// 测试（视频学习测试或问卷调查）
    /// </summary>
    public class Test
    {
        public Test()
        {
            this.T_CreateTime = DateTime.Now;
        }
        public int ID { get; set; }

        /// <summary>
        /// 关联课程ID
        /// </summary>
        [Display(Name = "关联课程ID")]
        public int GoodsID { get; set; }

        /// <summary>
        /// 课程目录ID
        /// </summary>
        [Display(Name = "课程目录ID")]
        public int T_GoodsArticleID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [Display(Name = "用户ID")]
        public int UserID { get; set; }

        /// <summary>
        /// 得分
        /// </summary>
        [Display(Name = "得分")]
        public int T_Score { get; set; }

        /// <summary>
        /// 正确题数
        /// </summary>
        [Display(Name = "正确题数")]
        public int T_RightCount { get; set; }

        /// <summary>
        /// 错误题数
        /// </summary>
        [Display(Name = "错误题数")]
        public int T_WrongCount { get; set; }

        /// <summary>
        /// 状态，默认0不合格，1合格
        /// </summary>
        [Display(Name = "状态")]
        public int T_State { get; set; }

        /// <summary>
        /// 测试时间
        /// </summary>
        [Display(Name = "测试时间")]
        public DateTime T_CreateTime { get; set; }
    }
    public class TestGoodsVModel
    {
        public Test Test { get; set; }
        public string UserName { get; set; }
        public string RealName { get; set; }
        public int Department { get; set; }
        public int Province { get; set; }
        public int City { get; set; }
        public string GoodsName { get; set; }
        public string GoodsArticleTitle { get; set; }
        public string ShopName { get; set; }
    }
}
