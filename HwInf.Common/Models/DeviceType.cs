using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;
using HwInf.Common.DAL;

namespace HwInf.Common.Models
{
    [Table("DeviceTypes")]
    public class DeviceType: IComparable<DeviceType>, IComparable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TypeId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Slug { get; set; }
        public virtual ICollection<FieldGroup> FieldGroups { get; set; }
        public bool IsActive { get; set; }
        public int CompareTo(DeviceType obj)
        {
            return Name.CompareTo(obj.Name);
        }

        public int CompareTo(object obj)
        {
            var o = obj as DeviceType;
            return CompareTo(o);
        }
    }
}