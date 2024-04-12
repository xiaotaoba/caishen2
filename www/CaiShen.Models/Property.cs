using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    public class Property
    {
        public Property()
        {
            this.Prop_IsEnable = 1;
            this.Prop_Sort = 0;
            this.Prop_ParentID = 0;
            this.GoodsTypeID = 0;
            this.Prop_IsHasSon = 0;
            this.Prop_IsSale = 1;
        }
        public int ID { get; set; }

        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "属性名称由2-20个字符组成")]
        [Display(Name = "属性名称")]
        public string Prop_Name { get; set; }

        [StringLength(100)]
        [Display(Name = "属性说明")]
        public string Prop_Desc { get; set; }

        [Display(Name = "是否启用")]
        public int Prop_IsEnable { get; set; }

        /// <summary>
        /// 一个产品只有一个数量属性，没有数量属性，默认显示数量输入框；
        /// 数量、单独计价价格，同时只能选中一个？ 颜色？
        /// </summary>
        [Display(Name = "是否数量属性")]
        public int Prop_IsNumber { get; set; }

        /// <summary>
        /// 是否销售属性
        /// </summary>
        [Display(Name = "是否销售属性")]
        public int Prop_IsSale { get; set; }

        /// <summary>
        /// 是否颜色属性
        /// </summary>
        [Display(Name = "是否颜色属性")]
        public int Prop_IsColor { get; set; }

        /// <summary>
        /// 前台展示：Text：文本显示所有值，Select：下拉框显示值，下拉显示值不允许为计价 ，不允许input属性值
        /// </summary>
        [Display(Name = "展示形式")]
        public int Prop_ShowType { get; set; }

        /// <summary>
        /// 是否单独计价属性
        /// </summary>
        [Display(Name = "是否单独计价属性")]
        public int Prop_IsPrice { get; set; }

        [Display(Name = "排序号")]
        public int Prop_Sort { get; set; }

        [Display(Name = " 父属性")]
        public int Prop_ParentID { get; set; }

        [Display(Name = " 是否有子属性")]
        public int Prop_IsHasSon { get; set; }

        [Required]
        [Display(Name = " 产品类型")]
        public int GoodsTypeID { get; set; }

        //public virtual GoodsType GoodsType { get; set; }

    }

    public class PropertyVModel
    {
        public Property Property { get; set; }
        public List<PropertyValue> Values { get; set; }

    }
}
