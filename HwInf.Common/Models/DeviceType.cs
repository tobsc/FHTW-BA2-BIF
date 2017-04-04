using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;
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
        public string Name { get; set; }
        public string Slug { get; set; }
        public virtual ICollection<FieldGroup> FieldGroups { get; set; }
        public bool IsActive { get; set; }
    }
}