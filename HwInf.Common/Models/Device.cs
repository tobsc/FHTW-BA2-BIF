using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public Person Person { get; set; }
        public string Room { get; set; }
        public DeviceStatus Status { get; set; }
        public DeviceType Type { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }
        public string DeviceGroupSlug { get; set; }
        public ICollection<DeviceMeta> DeviceMeta { get; set; }
        [NotMapped]
        public int Quantity { get; set; }
        [NotMapped]
        public int Stock { get; set; }
    }
}