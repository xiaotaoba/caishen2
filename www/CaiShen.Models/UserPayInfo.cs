using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    public class UserPayInfo
    {
        public int ID { get; set; }

     
        //alipay or bank
        //[Required]
        //[StringLength(20)]
        //public string Type { get; set; }

        [Display(Name = "微信号")]
        [StringLength(50)]
        public string Wechat { get; set; }

        [Display(Name = "支付宝账号", ShortName = "账号")]
        [StringLength(50)]
        public string AlipayNO { get; set; }

        [Display(Name = "卡号")]
        [StringLength(50)]
        public string BankNO { get; set; }

        [Display(Name = "开户行")]
        [StringLength(50)]
        public string BankName { get; set; }

        [Display(Name = "开户地")]
        [StringLength(200)]
        public string BankAddress { get; set; }

        [Required]
        public int UserID { get; set; }

        public virtual User User { get; set; }

    }
}
