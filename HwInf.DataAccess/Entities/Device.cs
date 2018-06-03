using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using HwInf.DataAccess.Context;

namespace HwInf.DataAccess.Entities
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
        private HashSet<DeviceMeta> _deviceMeta = new HashSet<DeviceMeta>();
        public IEnumerable<DeviceMeta> DeviceMeta => _deviceMeta;
        [NotMapped]
        public int Quantity { get; set; }
        [NotMapped]
        public int Stock { get; set; }

        public void AddDeviceMeta(DeviceMeta deviceMeta, HwInfContext context = null)
        {
            if (_deviceMeta != null)
            {
                _deviceMeta.Add(deviceMeta);
            } else if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            else if (context.Entry(this).IsKeySet)
            {
                context.Add(deviceMeta);
            }
            else
            {
                throw new InvalidOperationException("Could not add DeviceMeta to Device");
            }
        }
    }
}