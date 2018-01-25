using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HwInf.Common.DAL;

namespace HwInf.Common.Models
{
    [Table("Devices")]
    public class Device
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DeviceId { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public string InvNum { get; set; }
        public string Brand { get; set; }
        public virtual Person Person { get; set; }
        public string Room { get; set; }
        public virtual DeviceStatus Status { get; set; }
        public virtual DeviceType Type { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }
        public string DeviceGroupSlug { get; set; }
        public virtual ICollection<DeviceMeta> DeviceMeta { get; set; } = new List<DeviceMeta>();
        [NotMapped]
        public int Quantity { get; set; }
        [NotMapped]
        public int Stock { get; set; }
    }
}