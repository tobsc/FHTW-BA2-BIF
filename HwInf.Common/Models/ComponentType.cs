using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HwInf.Common.Models
{
    [Table("ComponentTypes")]
    public class ComponentType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompTypeId { get; set; }
        public string Name { get; set; }
        public string FieldType { get; set; }
    }
}
