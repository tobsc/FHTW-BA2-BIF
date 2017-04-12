using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HwInf.Common.BL;
using HwInf.Controllers;
using HwInf.ViewModels;
using NUnit.Framework;

namespace HwInf.Tests
{
    [TestFixture]
    public class HwInfTests
    {
        private readonly BL _bl = new BL();
        private readonly DevicesController _ctrl = new DevicesController();

        public HwInfTests()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        }

        [Test]
        public void HelloWorld() {}
    }
}
