using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace HwInf.Common.Models
{
    [Table("Settings")]
    public class Setting
    {
        [Key]
        [Index(IsUnique = true)]
        public string Key { get; set; }
        public string Value { get; set; }
    }
}