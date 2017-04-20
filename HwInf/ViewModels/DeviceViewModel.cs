using System;
using System.Collections.Generic;
using System.Linq;
using HwInf.Common.BL;
using HwInf.Common.Models;
using WebGrease.Css.Extensions;
using static System.String;

namespace HwInf.ViewModels
{
    public class DeviceViewModel
    {
        public int DeviceId { get; set; }
        public string Name { get; set; }
        public string InvNum { get; set; }
        public string Marke { get; set; }
        public string Raum { get; set; }
        public string CreateDate { get; set; }
        public DeviceTypeViewModel DeviceType { get; set; }
        public UserViewModel Verwalter { get; set; }
        public DeviceStatusViewModel Status { get; set; }
        public IEnumerable<DeviceMetaViewModel> DeviceMeta { get; set; }
        public IEnumerable<AdditionalInvNumViewModel> AdditionalInvNums { get; set; }
        public IEnumerable<object> FieldGroups { get; set; }

        public bool IsActive { get; set; } = true;

        public DeviceViewModel()
        {
        }

        public DeviceViewModel(Device obj)
        {
            Refresh(obj);
        }

        public void Refresh(Device obj)
        {
            if (obj == null)
            {
                return;
            }

            var target = this;
            var source = obj;


            target.DeviceId = source.DeviceId;
            target.Name = source.Name;
            target.InvNum = source.InvNum;
            target.Marke = source.Brand;
            target.Status = new DeviceStatusViewModel(source.Status);
            target.DeviceType= new DeviceTypeViewModel(source.Type);
            target.Raum = source.Room;
            target.Verwalter = new UserViewModel(source.Person);
            target.IsActive = source.IsActive;
            target.CreateDate = source.CreateDate.ToShortDateString();

            if (source.Type.FieldGroups != null)
            {
                target.FieldGroups = source.Type.FieldGroups.Select(i => new { Slug = i.Slug, Name = i.Name }).ToList();
            }
        }

        public void ApplyChanges(Device obj, BL bl)
        {
            var target = obj;
            var source = this;

            target.Name = source.Name;
            target.InvNum = source.InvNum;
            target.Brand = source.Marke;
            target.Status = bl.GetDeviceStatus(1);
            target.Type = bl.GetDeviceType(source.DeviceType.Slug);
            target.Person = bl.GetUsers(source.Verwalter.Uid);
            target.Room = source.Raum;
            target.IsActive = source.IsActive;
            target.DeviceMeta = new List<DeviceMeta>();
            source.DeviceMeta.ForEach(i => i.ApplyChanges(i, bl));
            source.DeviceMeta.ForEach(i => target.DeviceMeta.Add(i));


        }

        public DeviceViewModel LoadMeta(Device d)
        {
            DeviceMeta = d.DeviceMeta
                .Select(i => new DeviceMetaViewModel(i));

              
            return this; // fluent interface
        }
    }
}