using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HwInf.Common.DAL;

namespace HwInf.Common.Models
{
    [Table("DeviceMeta")]
    public class DeviceMeta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MetaId { get; set; }
        [Required]
        public Field Field { get; set; }
        public FieldGroup FieldGroup { get; set; }
        [Required]
        public string MetaValue { get; set; }
    }
}