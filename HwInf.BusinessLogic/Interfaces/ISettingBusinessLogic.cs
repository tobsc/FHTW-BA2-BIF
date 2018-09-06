using System.Collections.Generic;
using HwInf.Common.Models;

namespace HwInf.BusinessLogic.Interfaces
{
    public interface ISettingBusinessLogic
    {
        Setting GetSetting(string key);
        IEnumerable<Setting> GetSettings();
        Setting CreateSetting();
        void DeleteSetting(Setting s);
        void UpdateSetting(Setting s);
    }
}