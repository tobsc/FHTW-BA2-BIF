using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HwInf.Common.Models
{

    [Table("Persons")]
    public class Person : IComparable, IComparable<Person>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PersId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        public string Tel { get; set; }
        [Required]
        public string Uid { get; set; }
        [Required]
        public virtual Role Role { get; set; }
        public string Room { get; set; }
        public string Studiengang { get; set; }
        public int CompareTo(Person obj)
        {
            return LastName.CompareTo(obj.LastName);
        }

        public int CompareTo(object obj)
        {
            var o = obj as Person;
            return CompareTo(o);
        }
    }
}