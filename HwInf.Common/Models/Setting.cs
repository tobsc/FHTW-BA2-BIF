using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HwInf.Common.Models
{
    [Table("Settings")]
    public class Setting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Key { get; set; }
        public string Value { get; set; }
    }
}