using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Pannet.Models
{
    public class ManagerGroup
    {
        //管理员角色
        public int ID { get; set; }

        [Required(ErrorMessage = "{0}不能为空")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "角色名称由2-20个字符组成")]
        [Display(Name = "角色名称")]
        //[Remote("CheckGroupName", ErrorMessage = "角色名称已存在")]
        public string Name { get; set; }

        [StringLength(200)]
        [Display(Name = "角色描述")]
        public string Desc { get; set; }

        [StringLength(1000)]
        [Display(Name = "角色权限")]
        public string Limits { get; set; }

        public ICollection<ManagerWithGroup> ManagerWithGroup { get; set; }
    }
}
