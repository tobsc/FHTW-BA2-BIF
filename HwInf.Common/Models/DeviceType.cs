using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HwInf.Common.DAL;

namespace HwInf.Common.Models
{
    [Table("DeviceTypes")]
    public class DeviceType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TypeId { get; set; }
        [Required]
        public string Description { get; set; }
        public virtual ICollection<Component> Components { get; set; } = new List<Component>();

    }
}