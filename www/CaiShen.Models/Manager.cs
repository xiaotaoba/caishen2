using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pannet.Models
{
    public class Manager
    {
        public Manager()
        {
            this.CreateTime = DateTime.Now;
        }

        public int ID { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "管理账号不少于2个字符")]
        [Display(Name = "管理账号")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "{0}不能为空")]
        [DataType(DataType.Password)]
        [StringLength(50)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [StringLength(50)]
        [Display(Name = "手机号码")]
        public string Phone { get; set; }

        public DateTime CreateTime { get; set; }

        public virtual ICollection<ManagerWithGroup> ManagerWithGroup { get; set; }
    }

    //登录
    public class AdminLoginVModel
    {
        [Required(ErrorMessage = "请输入{0}")]
        [Display(Name = "管理账号")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "请输入{0}")]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

    }

    //编辑
    public class AdminEditVModel
    {
        public AdminEditVModel()
        {
            this.ID = 0;
            this.GroupID = 0;
        }
        [Required(ErrorMessage = "{0}不能为空")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "管理账号不少于2个字符")]
        [Display(Name = "管理账号")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "密码由5-20个字符组成")]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [StringLength(50)]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "请输入正确的{0}")]
        [Display(Name = "手机号码")]
        public string Phone { get; set; }

        [Display(Name = "角色")]
        [Required]
        public int GroupID { get; set; }

        [Display(Name = "ID")]
        public int ID { get; set; }
    }
}
