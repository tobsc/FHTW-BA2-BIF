using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HwInf.Common.Models
{
    [Table("Damages")]
    public class Damage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DamageId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public virtual Person Cause { get; set; }
        public virtual Person Reporter { get; set; }
        [Required]
        public string Description { get; set; }
        public virtual Device Device { get; set; } 
        [Required]
        public DamageStatus DamageStatus { get; set; } 

    }

    [Table("DamageStatus")]
    public class DamageStatus : IComparable<DamageStatus>, IComparable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StatusId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Slug { get; set; }

        public int CompareTo(DamageStatus other)
        {
            return StatusId.CompareTo(other.StatusId);
        }

        public int CompareTo(object obj)
        {
            var o = obj as DamageStatus;
            return CompareTo(o);
        }
    }
}
