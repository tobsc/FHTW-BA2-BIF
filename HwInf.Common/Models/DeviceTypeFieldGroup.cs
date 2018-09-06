namespace HwInf.Common.Models
{
    public class DeviceTypeFieldGroup
    {
        public int DeviceTypeId { get; set; }
        public DeviceType DeviceType { get; set; }
        public FieldGroup FieldGroup { get; set; }
        public int FieldGroupId { get; set; }
    }
}