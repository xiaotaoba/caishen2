using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    public class GoodsCategory
    {
        public GoodsCategory()
        {
            this.GC_IsEnable = 1;
            this.GC_IsRecommend = 0;
            this.GC_Sort = 0;
            this.GC_ParentID = 0;
            this.GC_Type = 0;
        }
        public int ID { get; set; }

        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "分类名称由2-20个字符组成")]
        [Display(Name = "分类名称")]
        public string GC_Name { get; set; }

        [Display(Name = "是否启用")]
        public int GC_IsEnable { get; set; }

        [Display(Name = "是否推荐")]
        public int GC_IsRecommend { get; set; }

        /// <summary>
        /// 类型-商品类型，默认0：不关联
        /// </summary>
        [Display(Name = "类型")]
        public int GC_Type { get; set; }

        [Display(Name = "排序号")]
        public int GC_Sort { get; set; }

        [Display(Name = "父分类")]
        public int GC_ParentID { get; set; }

        /// <summary>
        /// 分类图标
        /// </summary>
        [StringLength(300)]
        [Display(Name = "分类图标")]
        public string GC_Image { get; set; }

        /// <summary>
        /// 开放部门，即可观看视频部门
        /// </summary>
        [StringLength(1000)]
        [Display(Name = "开放部门")]
        public string GC_Department { get; set; }
    }

    public class GoodsCategoryHeadVModel
    {
        public GoodsCategory GoodsCategory { get; set; }

        public List<Goods> GoodsList { get; set; }

        public List<GoodsCategoryHeadVModel> GoodsCategorysListSub { get; set; }
    }


}
