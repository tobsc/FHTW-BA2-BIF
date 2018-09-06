using HwInf.BusinessLogic.Interfaces;
using HwInf.Common.Models;

namespace HwInf.Web.ViewModels
{
    public class SettingViewModel
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public SettingViewModel()
        {
            
        }

        public SettingViewModel(Setting obj)
        {
            Refresh(obj);
        }

        public void Refresh(Setting obj)
        {

            if (obj == null) return;

            var target = this;
            var source = obj;

            target.Key = source.Key;
            target.Value = source.Value;
        }

        public void ApplyChanges(Setting obj, IBusinessLogicFacade bl)
        {
            var target = obj;
            var source = this;

            target.Key = source.Key;
            target.Value = source.Value;

        }
    }
}