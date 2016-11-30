using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HwInf.Common.DAL
{
    [Table("DeviceMeta")]
    public class DBDeviceMeta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MetaId { get; set; }
        [Required]
        public virtual DBDevice Device { get; set; }
        [Required]
        public virtual DBDeviceType DeviceType { get; set; }
        [Required]
        public string MetaKey { get; set; }
        [Required]
        public string MetaValue { get; set; }
    }
}