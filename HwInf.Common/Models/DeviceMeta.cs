using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HwInf.Common.Models
{
    [Table("DeviceMeta")]
    public class DeviceMeta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MetaId { get; set; }
        [Required]
        public string FieldName { get; set; }
        [Required]
        public string FieldSlug { get; set; }
        [Required]
        public string FieldGroupName { get; set; }
        [Required]
        public string FieldGroupSlug { get; set; }
        [Required]
        public string MetaValue { get; set; }
        public Device Device { get; set; }


        public bool IsEqual(DeviceMeta dm)
        {
            return FieldSlug == dm.FieldSlug &&
                   FieldGroupSlug == dm.FieldGroupSlug &&
                   MetaValue == dm.MetaValue;
        }
    }

    public class DeviceMetaComparer : IEqualityComparer<DeviceMeta>
    {
        public bool Equals(DeviceMeta x, DeviceMeta y)
        {
            return x.FieldGroupSlug == y.FieldGroupSlug
                && x.FieldGroupSlug == y.FieldGroupSlug
                && int.Parse(x.MetaValue) <= int.Parse(y.MetaValue);
        }

        public int GetHashCode(DeviceMeta obj)
        {
            return obj.FieldGroupSlug.GetHashCode() ^ obj.FieldSlug.GetHashCode();
        }
    }
}