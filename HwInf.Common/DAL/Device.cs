﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HwInf.Common.DAL
{
    [Table("Device")]
    public class Device
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DeviceId { get; set; }
        [Required]
        public string Name { get; set; }

        public string InvNum { get; set; }

        public string Brand { get; set; }
        [Required]
        public int Status { get; set; }
        [Required]
        public virtual DeviceType Type { get; set; }
    }
}