using HwInf.Controllers;
using NUnit.Framework;

namespace HwInf.UnitTests.Controllers
{
    [TestFixture]
    public class PrintControllersTest : ControllerTests
    {
        private readonly PrintController ctr;


        public PrintControllersTest()
        {
            ctr = new PrintController(_bl);
        }

        [Test]
        public void HelloWorld()
        {
        }

        [Test]
        public void ctr_should_return_contract_pdf()
        {
            var obj = _bl.CreateOrder();
            var vmdl = ControllerHelper.GetValidOrderViewModel();
            vmdl.ApplyChanges(obj, _bl);
            var res = ctr.GetPrint(obj.OrderGuid);
            Assert.NotNull(res);
            Assert.NotNull(res.GetType().Name.Equals("document/pdf"));
        }

        [Test]
        public void ctr_should_return_return_pdf()
        {
            var obj = _bl.CreateOrder();
            var vmdl = ControllerHelper.GetValidOrderViewModel();
            vmdl.ApplyChanges(obj, _bl);
            var res = ctr.GetReturn(obj.OrderGuid);
            Assert.NotNull(res);
            Assert.NotNull(res.GetType().Name.Equals("document/pdf"));
        }

    }
}