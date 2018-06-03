using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HwInf.DataAccess.Entities
{
    [Table("Accessories")]
    public class Accessory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccessoryId { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
    }
}