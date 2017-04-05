using System;
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
        public virtual OrderStatus Status { get; set; }
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
        public virtual Device Device { get; set; }
        public Guid OrderGuid { get; set; }
    }
}
