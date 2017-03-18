using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HwInf.Common.BL;

namespace HwInf
{
    public class SlugGenerator
    {
        private static BL _bl = new BL();

        public static string GenerateSlug(string value)
        {
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

            return value;
        }

        private string CheckDuplicates(string value)
        {

            var a = Regex.Matches(value, "(slug-name){1}[-]*[0-9]*");

            return value;
        }

        public static int test(string value, string entity)
        {
            List<string> l = new List<string>(); 

            switch (entity)
            {
                case "fieldGroup":
                    l = _bl.GetFieldGroups().Select(i => i.Slug).ToList();
                    break;

                case "field":
                    l = _bl.GetFields().Select(i => i.Slug).ToList();
                    break;
                case "deviceType":
                    l = _bl.GetDeviceTypes().Select(i => i.Slug).ToList();
                    break;

            }


            var fg =_bl.GetFieldGroups().Select(i => i.Slug).ToList();

            string filter = "("+value+"){1}[-]*[0-9]*";

            List<string> a = fg.Where(x => Regex.IsMatch(x, filter, RegexOptions.IgnoreCase)).ToList();

            if(a.Count != null)
            {
                
            }

            return a.Count;
        }
    }
}