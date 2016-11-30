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

        public virtual DBDevice Device { get; set; }

        public virtual DBDeviceType DeviceType { get; set; }

        public string MetaKey { get; set; }

        public string MetaValue { get; set; }
    }
}