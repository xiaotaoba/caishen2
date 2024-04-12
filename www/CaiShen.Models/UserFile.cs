using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    public class UserFile
    {
        public UserFile()
        {
            this.UF_IsDelete = 0;
            this.UF_Is_Check = 1;
            this.UF_CreateTime = DateTime.Now;
        }
        public int ID { get; set; }
        
        /// <summary>
        /// 文件标题
        /// </summary>
        [Required]
        [StringLength(100)]
        [Display(Name = "文件标题")]
        public string UF_FileName { get; set; }

        /// <summary>
        /// 文件地址
        /// </summary>
        [StringLength(500)]
        [Display(Name = "文件地址")]
        public string UF_FilePath { get; set; }
        /// <summary>
        /// 删除 1是，0否
        /// </summary>
        public int UF_IsDelete { get; set; }

        /// <summary>
        /// 文件默认已审核，0待审核 1已审核
        /// </summary>
        [Display(Name = "是否审核")]
        public int UF_Is_Check { get; set; }

        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime UF_CreateTime { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }

    }
}
