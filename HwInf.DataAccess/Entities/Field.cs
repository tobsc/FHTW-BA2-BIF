using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HwInf.DataAccess.Context;

namespace HwInf.DataAccess.Entities
{
    [Table("Fields")]
    public class Field
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FieldId { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public FieldGroup FieldGroup { get; set; }
    }

    [Table("FieldGroups")]
    public class FieldGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GroupId { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        private HashSet<Field> _fields = new HashSet<Field>();
        public IEnumerable<Field> Fields => _fields;
        private HashSet<DeviceTypeFieldGroup> _deviceTypeFieldGroups = new HashSet<DeviceTypeFieldGroup>();
        public virtual IEnumerable<DeviceTypeFieldGroup> DeviceTypeFieldGroups => _deviceTypeFieldGroups;
        public bool IsActive { get; set; }
        public bool IsCountable { get; set; }
        public void AddField(Field field, HwInfContext context)
        {
            if (_fields != null)
            {
                _fields.Add(field);
            }
            else if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            else if (context.Entry(this).IsKeySet)
            {
                context.Add(field);
            }
            else
            {
                throw new InvalidOperationException("Could not add Field to FieldGroup");
            }
        }

        public void AddDeviceType(DeviceType deviceType, HwInfContext context)
        {
            var deviceTypeFieldGroup = new DeviceTypeFieldGroup
            {
                FieldGroup = this,
                FieldGroupId = GroupId,
                DeviceType = deviceType,
                DeviceTypeId = deviceType.TypeId
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
                throw new InvalidOperationException("Could not add DeviceType to FieldGroup");
            }
        }
    }
}