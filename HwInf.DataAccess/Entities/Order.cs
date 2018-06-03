using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HwInf.DataAccess.Context;

namespace HwInf.DataAccess.Entities
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
        public Person Entleiher { get; set; }
        public Person Verwalter { get; set; }
        private HashSet<OrderItem> _orderItems = new HashSet<OrderItem>();
        [Required]
        public IEnumerable<OrderItem> OrderItems => _orderItems;
        [Required]
        public Guid OrderGuid { get; set; }
        [Required]
        public string OrderReason { get; set; }
        [Required]
        public OrderStatus OrderStatus { get; set; }
        public DateTime ReturnDate { get; set; }

        public void AddOrderItem(OrderItem orderItem, HwInfContext context)
        {
            if (_orderItems != null)
            {
                _orderItems.Add(orderItem);
            }
            else if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            else if (context.Entry(this).IsKeySet)
            {
                context.Add(orderItem);
            }
            else
            {
                throw new InvalidOperationException("Could not add OrderItem to Order");
            }
        }
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
        public Person Entleiher { get; set; }
        public Person Verwalter { get; set; }
        public bool IsDeclined { get; set; }
        public string Accessories { get; set; }
        public Order Order { get; set; }


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
