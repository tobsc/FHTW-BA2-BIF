using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HwInf.Common.DAL
{
    [Table("Devices")]
    public class Device
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DeviceId { get; set; }
        [Required]
        public string Name { get; set; }

        public string InvNum { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public virtual Person Person { get; set; }

        public string Room { get; set; }
        [Required]
        public virtual Status Status { get; set; }
        [Required]
        public virtual DeviceType Type { get; set; }
        [Required]
        public DateTime CreateDate { get; set; }
    }
}