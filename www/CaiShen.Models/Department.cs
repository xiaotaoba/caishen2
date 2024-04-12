using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{

    public class Department
    {
        public Department()
        {
            this.Dep_FollowID = 0;
        }

        public int ID { get; set; }

        [StringLength(20, MinimumLength = 2, ErrorMessage = "名称由2-20个字符组成")]
        [Display(Name = "科室名称")]
        public string Dep_Name { get; set; }

        /// <summary>
        /// 所属科室ID，默认0为一级
        /// </summary>
        [Display(Name = "所属科室")]
        public int Dep_FollowID { get; set; }

        [StringLength(200)]
        [Display(Name = "科室描述")]
        public string Dep_Desc { get; set; }

        [StringLength(200)]
        [Display(Name = "科室权限")]
        public string Dep_Limit { get; set; }

    }
}
