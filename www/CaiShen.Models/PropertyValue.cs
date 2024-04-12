using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    public class PropertyValue
    {
        public PropertyValue()
        {
            this.PV_IsEnable = 1;
            this.PV_Min = 0;
            this.PV_Max = 0;
            this.PV_Multiple = 1;
            this.PV_Increment = 1;
            this.PV_Price = 0;
            this.PV_ColorHEX = "";
            this.PV_IsFile = 0;
            this.PV_Sort = 0;
        }
        public int ID { get; set; }

        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "属性值名称由1-20个字符组成")]
        [Display(Name = "属性值名称")]
        public string PV_Name { get; set; }

        [Display(Name = "是否启用")]
        public int PV_IsEnable { get; set; }

        /// <summary>
        /// 单位/提示
        /// </summary>
        [Display(Name = "单位/提示")]
        [StringLength(20)]
        public string PV_Unit { get; set; }

        #region 针对Input展示类型（数字类型）的属性

        /// <summary>
        /// 最小取值
        /// </summary>
        [Display(Name = "最小取值")]
        public double PV_Min { get; set; }

        /// <summary>
        /// 最大取值
        /// </summary>
        [Display(Name = "最大取值")]
        public double PV_Max { get; set; }

        /// <summary>
        /// 倍数或基数
        /// </summary>
        [Display(Name = "倍数")]
        public double PV_Multiple { get; set; }

        /// <summary>
        /// 增减量
        /// </summary>
        [Display(Name = "增减量")]
        public double PV_Increment { get; set; }

        /// <summary>
        /// 针对单独计价属性，取值单价
        /// </summary>
        [Display(Name = "取值单价")]
        public decimal PV_Price { get; set; }

        //实际产品数量 = 客户输入量*倍数(GPV_Multiple)
        //如果是input属性并且是单独计价属性
        // 产品价格 = SKU价格 * 数量属性值 + 取值单价（GPV_Price）*客户输入取值单价属性数量

        #endregion

        /// <summary>
        /// 前台展示：0 Text：文本，1 Input：数据框(只限数字)，一个属性只能有一个input属性值
        /// 属性select展示时，值不允许使用input展示
        /// </summary>
        [Display(Name = "展示形式")]
        public int PV_ShowType { get; set; }

        /// <summary>
        /// 如果是颜色属性,颜色值十六进制
        /// </summary>
        [StringLength(20)]
        [Display(Name = "颜色值")]
        public string PV_ColorHEX { get; set; }

        /// <summary>
        /// 颜色图片,备用
        /// </summary>
        [StringLength(200)]
        [Display(Name = "颜色图片")]
        public string PV_ColorImage { get; set; }

        /// <summary>
        /// 是否需要上传附件
        /// </summary>
        [Display(Name = "是否附件")]
        public int PV_IsFile { get; set; }

        [Display(Name = "排序号")]
        public int PV_Sort { get; set; }

        [Display(Name = "属性名称")]
        public int PropertyID { get; set; }

    }
}
