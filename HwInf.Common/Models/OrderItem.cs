using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HwInf.Common.DAL;

namespace HwInf.Common.Models
{
    [Table("OrderItems")]
    public class OrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemId { get; set; }
        [Required]
        public virtual Device Device { get; set; }
        [Required]
        public virtual Order Order { get; set; }
    }
}
