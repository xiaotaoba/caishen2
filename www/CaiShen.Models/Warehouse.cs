using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    public class Warehouse
    {
        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse()
        {
            this.Is_Enable = 1;
            this.CreateTime = DateTime.Now;
            this.ShippingTemplateID = 0;
        }
        public int ID { get; set; }

        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "仓库名称由2-20个字符组成")]
        [Display(Name = "仓库名称")]
        public string Name { get; set; }

        [StringLength(50)]
        [Display(Name = "仓库编号")]
        public string Number { get; set; }

        [StringLength(200)]
        [Display(Name = "仓库描述")]
        public string Desc { get; set; }

        [Display(Name = "省份")]
        public int Province { get; set; }

        [Display(Name = "城市")]
        public int City { get; set; }

        [Display(Name = "地区")]
        public int Region { get; set; }

        [StringLength(500)]
        [Display(Name = "地址")]
        public string Address { get; set; }

        [StringLength(50)]
        [Display(Name = "联系电话")]
        public string Tel { get; set; }

        [StringLength(50)]
        [Display(Name = "联系人")]
        public string UserName { get; set; }

        [StringLength(200)]
        [Display(Name = "仓库官网网址")]
        public string URL { get; set; }

        [Display(Name = "是否启用")]
        public int Is_Enable { get; set; }

        [Display(Name = "创建时间")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 运费模板
        /// </summary>
        [Display(Name = "运费模板")]
        public int ShippingTemplateID { get; set; }
    }
}
