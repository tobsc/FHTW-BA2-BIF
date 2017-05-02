using HwInf.Common.BL;
using HwInf.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HwInf.ViewModels
{
    public class DamageViewModel
    {
        public int DamageId { get; set; }
        public DateTime Date { get; set; }
        public UserViewModel Cause { get; set; }
        public UserViewModel Reporter { get; set; }
        public string Description { get; set; }
        public virtual DeviceViewModel Device { get; set; }
        public DamageStatusViewModel DamageStatus { get; set; }

        public DamageViewModel()
        {

        }

        public DamageViewModel(Damage obj)
        {
            Refresh(obj);
        }

        public void Refresh(Damage obj)
        {

            if (obj == null) return;

            var target = this;
            var source = obj;

            target.DamageId = source.DamageId;
            target.Date = source.Date;
            target.Cause = new UserViewModel(source.Cause);
            target.Reporter = new UserViewModel(source.Reporter);
            target.Description = source.Description;
            target.Device = new DeviceViewModel(source.Device);
            target.DamageStatus = new DamageStatusViewModel(source.DamageStatus);
        }

        public void ApplyChanges(Damage obj, BL bl)
        {
            var target = obj;
            var source = this;
            
            target.Date = DateTime.Now;
            target.Cause = bl.GetUsers(source.Cause.Uid);
            target.Reporter = bl.GetUsers(bl.GetCurrentUid());
            target.Description = source.Description;
            target.Device = bl.GetSingleDevice(source.Device.InvNum);
            target.DamageStatus = source.DamageStatus == null
                ? bl.GetDamageStatus("gemeldet")
                : bl.GetDamageStatus(source.DamageStatus.Slug);

        }

        public void Update(Damage obj, BL bl)
        {
            var target = obj;
            var source = this;

            
            target.Description = source.Description;
            target.Cause = bl.GetUsers(source.Cause.Uid);
            target.DamageStatus = source.DamageStatus == null
                ? bl.GetDamageStatus("gemeldet")
                : bl.GetDamageStatus(source.DamageStatus.Slug);
        }
    }
}