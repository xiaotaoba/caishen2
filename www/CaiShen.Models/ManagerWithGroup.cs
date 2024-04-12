using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pannet.Models
{
    public class ManagerWithGroup
    {
        public int ID { get; set; }

        [Required]
        public int ManagerID { get; set; }

        [Required]
        public int ManagerGroupID { get; set; }

        public virtual Manager Manager { get; set; }

        public virtual ManagerGroup ManagerGroup { get; set; }

    }
}
