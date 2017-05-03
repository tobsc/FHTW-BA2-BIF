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
        public virtual Person Entleiher { get; set; }
        public virtual Person Verwalter { get; set; }
        [Required]
        public ICollection<OrderItem> OrderItems { get; set; }
        [Required]
        public Guid OrderGuid { get; set; }
        [Required]
        public string OrderReason { get; set; }
        [Required]
        public OrderStatus OrderStatus { get; set; }
        public DateTime ReturnDate { get; set; }
    }

    [Table("OrderItems")]
    public class OrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemId { get; set; }
        public virtual Device Device { get; set; }
        [Required]
        public DateTime From { get; set; }
        [Required]
        public DateTime To { get; set; }
        public DateTime ReturnDate { get; set; }
        [Required]
        public DateTime CreateDate { get; set; }
        public virtual Person Entleiher { get; set; }
        public virtual Person Verwalter { get; set; }
        public bool IsDeclined { get; set; }
        public IEnumerable<string> Accessories { get; set; }


    }

    [Table("OrderStatus")]
    public class OrderStatus : IComparable<OrderStatus>, IComparable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StatusId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Slug { get; set; }

        public int CompareTo(OrderStatus other)
        {
            return StatusId.CompareTo(other.StatusId);
        }

        public int CompareTo(object obj)
        {
            var o = obj as OrderStatus;
            return CompareTo(o);
        }
    }

}
