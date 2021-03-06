﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace HwInf.Common.Models
{
    [Table("Fields")]
    public class Field
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FieldId { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public FieldGroup FieldGroup { get; set; }
    }

    [Table("FieldGroups")]
    public class FieldGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GroupId { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public ICollection<Field> Fields { get; set; }
        public ICollection<DeviceTypeFieldGroup> DeviceTypeFieldGroups { get; set; } = new List<DeviceTypeFieldGroup>();
        [NotMapped]
        public ICollection<DeviceType> DeviceTypes => DeviceTypeFieldGroups.Select(i => i.DeviceType).ToList();
        public bool IsActive { get; set; }
        public bool IsCountable { get; set; }
    }
}