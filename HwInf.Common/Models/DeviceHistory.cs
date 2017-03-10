using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HwInf.Common.Models
{
    [Table("DeviceHistory")]
    public class DeviceHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HistId { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public virtual Device Device { get; set; }
        [Required]
        public virtual Person Person { get; set; }
    }
}
