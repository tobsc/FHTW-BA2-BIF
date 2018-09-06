using System.Collections.Generic;
using HwInf.Common.Models;

namespace HwInf.BusinessLogic.Interfaces
{
    public interface IDamageBusinessLogic
    {
        Damage GetDamage(int id);
        IEnumerable<Damage> GetDamages(string invNum);
        IEnumerable<Damage> GetDamages(int deviceId);
        IEnumerable<Damage> GetDamages();
        Damage CreateDamage();
        void DeleteDamage(Damage s);
        void UpdateDamage(Damage s);
        IEnumerable<DamageStatus> GetDamageStatus();
        DamageStatus GetDamageStatus(string slug);
        bool DamageExists(int damageId);
    }
}