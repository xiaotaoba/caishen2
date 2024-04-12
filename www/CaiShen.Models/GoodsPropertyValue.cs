using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    public class GoodsPropertyValue
    {
        public GoodsPropertyValue()
        {
            this.GPV_SKU_ID = 0;
            this.GPV_IsSKU = 0;
            this.GPV_Min = 0;
            this.GPV_Max = 0;
            this.GPV_Multiple = 1;
            this.GPV_Increment = 1;
            this.GPV_Price = 0;
            this.GPV_ColorHEX = "";
        }
        public int ID { get; set; }

        [Display(Name = "产品ID")]
        public int GoodsID { get; set; }

        [Display(Name = "属性名ID")]
        public int PropertyID { get; set; }

        [Display(Name = "属性值ID")]
        public int PropertyValueID { get; set; }

        [Display(Name = "SKUID")]
        public int GPV_SKU_ID { get; set; }

        [Display(Name = "是否SKU")]
        public int GPV_IsSKU { get; set; }


        [Display(Name = "单位/提示")]
        [StringLength(20)]
        public string GPV_Unit { get; set; }

        #region 针对Input展示类型（数字类型）的属性

        [Display(Name = "最小取值")]
        public double GPV_Min { get; set; }

        [Display(Name = "最大取值")]
        public double GPV_Max { get; set; }

        /// <summary>
        /// 倍数或基数
        /// </summary>
        [Display(Name = "倍数")]
        public double GPV_Multiple { get; set; }

        [Display(Name = "增减量")]
        public double GPV_Increment { get; set; }

        [Display(Name = "取值单价")]
        public decimal GPV_Price { get; set; }

        //实际产品数量 = 客户输入量*倍数(GPV_Multiple)
        //如果是input属性并且是单独计价属性
        // 产品价格 = SKU价格 * 数量属性值 + 取值单价（GPV_Price）*客户输入取值单价属性数量

        #endregion

        /// <summary>
        /// 如果是颜色属性
        /// </summary>
        [StringLength(20)]
        [Display(Name = "颜色值")]
        public string GPV_ColorHEX { get; set; }

        [StringLength(200)]
        [Display(Name = "颜色图片")]
        public string GPV_ColorImage { get; set; }

        /// <summary>
        /// 是否需要上传附件
        /// </summary>
        [Display(Name = "是否附件")]
        public int GPV_IsFile { get; set; }

    }
}
