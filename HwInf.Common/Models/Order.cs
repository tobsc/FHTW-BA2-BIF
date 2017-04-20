using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HwInf.Common.Models
{
    [Table("Orders")]
   public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public DateTime From { get; set; }
        [Required]
        public DateTime To { get; set; }
        public DateTime ReturnDate { get; set; }
        [Required]
        public virtual Person Entleiher { get; set; }
        [Required]
        public virtual Person Verwalter { get; set; }
        [Required]
        public ICollection<OrderItem> OrderItems { get; set; }
        [Required]
        public Guid OrderGuid { get; set; }
        [Required]
        public string OrderReason { get; set; }
    }

    [Table("OrderItems")]
    public class OrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemId { get; set; }
        public virtual Device Device { get; set; }
        public virtual OrderStatus OrderStatus { get; set; }

    }

    [Table("OrderStatus")]
    public class OrderStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StatusId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Slug { get; set; }

    }

}
