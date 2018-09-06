using System.Collections.Generic;
using System.Linq;
using System.Security;
using HwInf.BusinessLogic.Interfaces;
using HwInf.Common.Models;
using HwInf.DataAccess.Interfaces;

namespace HwInf.BusinessLogic
{
    public class SettingBusinessLogic : ISettingBusinessLogic
    {
        private readonly IDataAccessLayer _dal;
        private readonly IBusinessLogicPrincipal _principal;

        public SettingBusinessLogic(IDataAccessLayer dal, IBusinessLogicPrincipal principal)
        {
            _dal = dal;
            _principal = principal;
        }
        public Setting GetSetting(string key)
        {
            return _dal.Settings.FirstOrDefault(i => i.Key.Equals(key));
        }

        public IEnumerable<Setting> GetSettings()
        {
            return _dal.Settings;
        }

        public Setting CreateSetting()
        {
            if (!_principal.IsAllowed) throw new SecurityException();

            return _dal.CreateSetting();
        }

        public void DeleteSetting(Setting s)
        {
            _dal.DeleteSetting(s);
        }

        public void UpdateSetting(Setting s)
        {
            if (!_principal.IsAllowed) return;

            _dal.UpdateObject(s);
        }
    }
}