using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HwInf.Common.DAL
{

    [Table("Persons")]
    public class DBPerson
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PersId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public string Phone { get; set; }
        [Required]
        public string uid { get; set; }
    }
}