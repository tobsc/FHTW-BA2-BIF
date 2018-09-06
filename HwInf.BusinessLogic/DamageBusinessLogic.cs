using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using HwInf.BusinessLogic.Interfaces;
using HwInf.Common.Models;
using HwInf.DataAccess.Interfaces;

namespace HwInf.BusinessLogic
{
    public class DamageBusinessLogic : IDamageBusinessLogic
    {
        private readonly IDataAccessLayer _dal;
        private readonly IBusinessLogicPrincipal _principal;

        public DamageBusinessLogic(IDataAccessLayer dal, IBusinessLogicPrincipal principal)
        {
            _dal = dal;
            _principal = principal;
        }
        public Damage GetDamage(int id)
        {
            return _dal.Damages.FirstOrDefault(i => i.DamageId.Equals(id));
        }

        public IEnumerable<Damage> GetDamages(string invNum)
        {
            if (String.IsNullOrWhiteSpace(invNum))
            {
                return _dal.Damages;
            }
            return _dal.Damages.Where(i => i.Device.InvNum.Equals(invNum));
        }

        public IEnumerable<Damage> GetDamages(int deviceId)
        {
            if (deviceId < 1)
            {
                return _dal.Damages;
            }
            return _dal.Damages.Where(i => i.Device.DeviceId == deviceId);
        }

        public IEnumerable<Damage> GetDamages()
        {
            return _dal.Damages.Any() ? _dal.Damages : Enumerable.Empty<Damage>();
        }

        public Damage CreateDamage()
        {
            if (!_principal.IsAllowed) throw new SecurityException();

            return _dal.CreateDamage();
        }

        public void DeleteDamage(Damage s)
        {
            _dal.DeleteDamage(s);
        }

        public void UpdateDamage(Damage s)
        {
            if (!_principal.IsAllowed) return;

            _dal.UpdateObject(s);
        }

        public IEnumerable<DamageStatus> GetDamageStatus()
        {
            return _dal.DamageStatus;
        }

        public DamageStatus GetDamageStatus(string slug)
        {
            return _dal.DamageStatus.FirstOrDefault(i => i.Slug.Equals(slug));
        }

        public bool DamageExists(int damageId)
        {
            return _dal.Damages.Any(i => i.DamageId == damageId);
        }
    }
}