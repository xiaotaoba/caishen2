using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    public class Supplier
    {
        public Supplier()
        {
            this.Sup_Is_Enable = 1;
            this.Sup_CreateTime = DateTime.Now;
            this.Sup_SupplyGoodsWay = 1;
            this.ShippingTemplateID = 0;
            this.SettlementCycle = 0;
        }
        public int ID { get; set; }

        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "供应商名称由2-20个字符组成")]
        [Display(Name = "供应商名称")]
        public string Sup_Name { get; set; }

        [StringLength(50)]
        [Display(Name = "供应商编号")]
        public string Sup_Number { get; set; }

        [StringLength(200)]
        [Display(Name = "供应商描述")]
        public string Sup_Desc { get; set; }

        [Display(Name = "省份")]
        public int Sup_Province { get; set; }

        [Display(Name = "城市")]
        public int Sup_City { get; set; }

        [Display(Name = "地区")]
        public int Sup_Region { get; set; }

        [StringLength(500)]
        [Display(Name = "地址")]
        public string Sup_Address { get; set; }

        [StringLength(50)]
        [Display(Name = "联系电话")]
        public string Sup_Tel { get; set; }

        [StringLength(50)]
        [Display(Name = "联系人")]
        public string Sup_UserName { get; set; }

        [StringLength(200)]
        [Display(Name = "供应商官网网址")]
        public string Sup_URL { get; set; }


        [Display(Name = "会员ID")]
        public int UserID { get; set; }

        public virtual User User { get; set; }

        [Display(Name = "是否启用")]
        public int Sup_Is_Enable { get; set; }

        /// <summary>
        /// 默认1:供应所有产品,2:按分类选择，3:自定义选择产品
        /// </summary>
        [Display(Name = "设置供应产品")]
        public int Sup_SupplyGoodsWay { get; set; }

        [StringLength(200)]
        [Display(Name = "关闭理由")]
        public string Sup_CloseReason { get; set; }

        [Display(Name = "创建时间")]
        public DateTime Sup_CreateTime { get; set; }

        /// <summary>
        /// 运费模板
        /// </summary>
        [Display(Name = "运费模板")]
        public int ShippingTemplateID { get; set; }

        /// <summary>
        /// 供应商结算周期 天数，默认0，未设置
        /// </summary>
        [Display(Name = "结算周期")]
        public int SettlementCycle { get; set; }
    }
}
