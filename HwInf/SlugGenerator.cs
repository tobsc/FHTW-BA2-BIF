using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HwInf.Common.BL;
using log4net;
using System;
using HwInf.Common.Interfaces;
using IBusinessLayer = HwInf.Common.Interfaces.IBusinessLayer;

namespace HwInf
{
    public class SlugGenerator
    {
        private static IBusinessLayer _bl;
        private static readonly ILog _log = LogManager.GetLogger(typeof(SlugGenerator).Name);

        public static string GenerateSlug(IBusinessLayer bl, string value, string entity = null)
        {
            _bl = bl;

            //First to lower case
            value = value.ToLowerInvariant();

            //Replace ß with ss
            value = value.Replace("ß", "ss");

            //Remove all accents
            var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(value);
            value = Encoding.ASCII.GetString(bytes);

            //Replace spaces
            value = Regex.Replace(value, @"\s", "-", RegexOptions.Compiled);


            //Remove invalid chars
            value = Regex.Replace(value, @"[^a-z0-9\s-_]", "", RegexOptions.Compiled);

            //Trim dashes from end
            value = value.Trim('-', '_');

            //Replace double occurences of - or _
            value = Regex.Replace(value, @"([-_]){2,}", "$1", RegexOptions.Compiled);


            if (!string.IsNullOrWhiteSpace(entity))
            {
                value = CheckDuplicates(value, entity);
            }


            return value;
        }

        private static string CheckDuplicates(string value, string entity)
        {

            var slugList = new List<string>();
            try
            {
                switch (entity)
                {
                    case "fieldGroup":
                        slugList = _bl.GetFieldGroups().Select(i => i.Slug).ToList();
                        break;

                    case "field":
                        slugList = _bl.GetFields().Select(i => i.Slug).ToList();
                        break;
                    case "deviceType":
                        slugList = _bl.GetDeviceTypes().Select(i => i.Slug).ToList();
                        break;
                    case "orderStatus":
                        slugList = _bl.GetOrderStatus().Select(i => i.Slug).ToList();
                        break;
                    case "accessory":
                        slugList = _bl.GetAccessories().Select(i => i.Slug).ToList();
                        break;
                    default:
                        break;

                }

                if (!slugList.Any()) return value;
                var filter = "(" + value + "){1}[-]*[0-9]*";
                var duplicatesList = slugList.Where(x => Regex.IsMatch(x, filter, RegexOptions.IgnoreCase)).ToList();

                if (duplicatesList.Count != 0)
                {
                    value = value + "-" + duplicatesList.Count;
                }
                return value;

            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: {0}", ex);
                throw;
            }

        }
    }
}