﻿using System.Collections.Generic;
using System.Linq;
using HwInf.BusinessLogic.Interfaces;

namespace HwInf.Web.ViewModels
{
    public class AutoFillViewModel
    {

        public List<string> AutoFillList = new List<string>();

        public AutoFillViewModel()
        {
            
        }

        public AutoFillViewModel(IEnumerable<string> list)
        {
            AutoFillList = list.ToList();
        }

        public AutoFillViewModel RefreshList(string input, string type, string component, IBusinessLogicFacade bl)
        {
            var deviceType = bl.GetDeviceType(type);
            var devices = bl.GetDevices(true, deviceType.Slug);
            var meta = bl.GetDeviceMeta();

            switch (component.ToLower())
            {
                case "name":
                    var componentNameValues = devices
                        .Where(i => i.Type.Name.ToLower().Equals(type.ToLower()))
                        .Where(i => i.Name.ToLower().Contains(input.ToLower()))
                        .OrderBy(i => i.Name)
                        .Select(i => i.Name)
                        .Distinct();
                    AutoFillList.AddRange(componentNameValues);
                    break;
                case "marke":
                case "brand":
                    var componentBrandValues = devices
                        .Where(i => i.Type.Name.ToLower().Equals(type.ToLower()))
                        .Where(i => i.Brand.ToLower().Contains(input.ToLower()))
                        .OrderBy(i => i.Brand)
                        .Select(i => i.Brand)
                        .Distinct();
                    AutoFillList.AddRange(componentBrandValues);
                    break;
                default:
                    var componentMetaValues = meta
                        .Where(i => i.MetaValue.ToLower().Contains(input.ToLower()))
                        .OrderBy(i => i.MetaValue)
                        .Select(i => i.MetaValue)
                        .Distinct();
                    AutoFillList.AddRange(componentMetaValues);
                    break;
            }

            return this;
        }
    }
}