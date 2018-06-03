using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HwInf.DataAccess.Context;

namespace HwInf.DataAccess.Entities
{
    [Table("DeviceTypes")]
    public class DeviceType: IComparable<DeviceType>, IComparable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TypeId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Slug { get; set; }
        private HashSet<DeviceTypeFieldGroup> _deviceTypeFieldGroups = new HashSet<DeviceTypeFieldGroup>();
        public ICollection<DeviceTypeFieldGroup> DeviceTypesFieldGroups => _deviceTypeFieldGroups;
        public bool IsActive { get; set; }
        public int CompareTo(DeviceType obj)
        {
            return Name.CompareTo(obj.Name);
        }

        public int CompareTo(object obj)
        {
            var o = obj as DeviceType;
            return CompareTo(o);
        }

        public void AddFieldGroup(FieldGroup fieldGroup, HwInfContext context)
        {
            var deviceTypeFieldGroup = new DeviceTypeFieldGroup
            {
                FieldGroup = fieldGroup,
                FieldGroupId = fieldGroup.GroupId,
                DeviceType = this,
                DeviceTypeId = TypeId
            };
            if (_deviceTypeFieldGroups != null)
            {
                _deviceTypeFieldGroups.Add(deviceTypeFieldGroup);
            }
            else if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            else if (context.Entry(this).IsKeySet)
            {
                context.Add(deviceTypeFieldGroup);
            }
            else
            {
                throw new InvalidOperationException("Could not add FieldGroup to DeviceType");
            }
        }
    }
}