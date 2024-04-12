using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    public class GoodsPhoto
    {
        public GoodsPhoto()
        {
            this.GP_IsFirst = 1;
            this.GP_CreateTime = DateTime.Now;
            this.GP_Sort = 0;
            this.GoodsID = 0;
        }
        public int ID { get; set; }

        [StringLength(200)]
        [Display(Name = "图片地址")]
        public string GP_Image { get; set; }

        [Display(Name = "是否首图")]
        public int GP_IsFirst { get; set; }

        [Display(Name = "产品ID")]
        public int GoodsID { get; set; }
        public virtual Goods Goods { get; set; }

        [Display(Name = "排序号")]
        public int GP_Sort { get; set; }

        public DateTime GP_CreateTime { get; set; }

    }
}
