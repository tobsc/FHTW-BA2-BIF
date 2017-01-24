using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HwInf.Common.DAL
{
    [Table("DeviceMeta")]
    public class DeviceMeta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MetaId { get; set; }
        [Required]
        public virtual Device Device { get; set; }
        [Required]
        public virtual Component Component { get; set; }
        [Required]
        public string MetaValue { get; set; }
    }
}