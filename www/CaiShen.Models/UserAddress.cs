using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    public class UserAddress
    {
        /// <summary>
        /// 会员收货地址
        /// </summary>
        public UserAddress()
        {
            this.Is_Default = 0;
            this.Time = DateTime.Now;
        }

        public int ID { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        [Display(Name = "省份")]
        public int Province { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        [Display(Name = "城市")]
        public int City { get; set; }

        /// <summary>
        /// 地区
        /// </summary>
        [Display(Name = "地区")]
        public int Region { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [StringLength(100)]
        [Display(Name = "地址")]
        public string Address { get; set; }

        /// <summary>
        /// 收货人
        /// </summary>
        [StringLength(50)]
        [Display(Name = "收货人")]
        public string UserName { get; set; }

        [StringLength(50)]
        [Display(Name = "手机号")]
        public string Mobile { get; set; }

        [StringLength(50)]
        [Display(Name = "电话")]
        public string Tel { get; set; }

        [StringLength(50)]
        [Display(Name = "邮箱")]
        public string Email { get; set; }

        [StringLength(50)]
        [Display(Name = "邮编")]
        public string Post_Code { get; set; }

        /// <summary>
        /// 编辑时间
        /// </summary>
        [Display(Name = "编辑时间")]
        public DateTime Time { get; set; }

        /// <summary>
        /// 默认使用
        /// </summary>
        [Display(Name = "默认使用")]
        public int Is_Default { get; set; }

        [Required]
        public int UserID { get; set; }

        public virtual User User { get; set; }

    }
}
