using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    public class Area
    {
        public Area()
        {
            this.Area_Order = 0;
            this.Area_Type = 0;
            this.Area_ParentID = 0;
            this.Area_Kind = 0;
        }
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "名称")]
        public string Area_Name { get; set; }

        [StringLength(50)]
        [Display(Name = "字母")]
        public string Area_Letter { get; set; }

        [Required]
        public int Area_ParentID { get; set; }

        [Required]
        public int Area_Type { get; set; }

        [Required]
        public int Area_Order { get; set; }

        [Required]
        public int Area_Kind { get; set; }

    }
}
