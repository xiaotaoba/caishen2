using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    /// <summary>
    /// 通用评价-针对产品或文章
    /// </summary>
    public class Comment
    {
        public Comment()
        {
            this.Comm_UserId = 0;
            this.Comm_P_Id = 0;
            this.Comm_ReplyId = 0;
            this.Comm_ReplyCount = 0;
            this.Comm_Zan = 0;
            this.Comm_Status = 0;
            this.Comm_ZanId = 0;
            this.Comm_Type = 0;
            this.Comm_Time = DateTime.Now;
        }
        public int ID { get; set; }
        /// <summary>
        /// 评论对象ID
        /// </summary>
        public int Comm_P_Id { get; set; }
        public int Comm_UserId { get; set; }
        [StringLength(50)]
        public string Comm_Title { get; set; }
        public DateTime Comm_Time { get; set; }
        [StringLength(500)]
        public string Comm_Content { get; set; }
        public int Comm_ReplyId { get; set; }
        public int Comm_ReplyCount { get; set; }
        public int Comm_Zan { get; set; }
        public int Comm_Status { get; set; }
        public int Comm_ZanId { get; set; }
        /// <summary>
        ///0课程 1新闻 
        /// </summary>
        public int Comm_Type { get; set; }

    }

    public class CommentGoodsVModel
    {
        public Comment Comment { get; set; }
        public string UserName { get; set; }
        public string GoodsName { get; set; }
        public string UserIMG { get; set; }
    }
}
