using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HwInf.Common.DAL
{
    [Table("Components")]
    public class Component
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompId { get; set; }
        public string Name { get; set; }
        public string FieldType { get; set; }
        public virtual DeviceType DeviceType { get; set; }
    }
}
