using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
   
    public class UserRole
    {

        public UserRole()
        {
            this.Role_Discount_Percent = 0;
        }

        public int ID { get; set; }

        [StringLength(20, MinimumLength = 2, ErrorMessage = "会员角色由2-20个字符组成")]
        [Display(Name = "会员角色")]
        public string Role_Name { get; set; }

        [StringLength(200)]
        [Display(Name = "角色描述")]
        public string Role_Desc { get; set; }

        [Display(Name = "折扣(%)")]
        public double Role_Discount_Percent { get; set; }

        //[StringLength(1000)]
        //[Display(Name = "角色权限")]
        //public string Role_Limits { get; set; }

        public ICollection<User> Users { get; set; }

    }
}
