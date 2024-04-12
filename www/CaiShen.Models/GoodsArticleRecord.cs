using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    /// <summary>
    /// 课程视频目录学习记录
    /// </summary>
    public class GoodsArticleRecord
    {
        public GoodsArticleRecord()
        {
            this.GoodsID = 1;
            this.UserID = 0;
            this.GoodsArticleID = 0;
            this.GAR_State = 0;
            this.GAR_Time = DateTime.Now;
        }
        public int ID { get; set; }

        /// <summary>
        /// 关联课程
        /// </summary>
        [Display(Name = "课程ID")]
        public int GoodsID { get; set; }

        /// <summary>
        /// UserID
        /// </summary>
        [Display(Name = "UserID")]
        public int UserID { get; set; }

        /// <summary>
        /// 课程视频
        /// </summary>
        [Display(Name = "课程视频ID")]
        public int GoodsArticleID { get; set; }

        /// <summary>
        /// 默认,0已观看未完成，1已完成
        /// </summary>
        [Display(Name = "状态")]
        public int GAR_State { get; set; }

        /// <summary>
        /// 观看时间
        /// </summary>
        [Display(Name = "观看时间")]
        public DateTime GAR_Time { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [Display(Name = "结束时间")]
        public DateTime? GAR_EndTime { get; set; }

    }
    /// <summary>
    /// 课程视频目录学习记录
    /// </summary>
    public class GoodsArticleRecordVModel
    {
        public GoodsArticleRecord GoodsArticleRecord { get; set; }
        public string UserName { get; set; }
        public string GoodsName { get; set; }
        public string GoodsArticleTitle { get; set; }
    }

}
