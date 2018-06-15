using System;
using System.Collections.Generic;
using System.Text;

namespace DinkToPdf
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class WkHtmlAttribute : Attribute
    {
        public string Name { get; private set; }

        public WkHtmlAttribute(string name)
        {
            Name = name;
        }
    }
}
