using HwInf.Services.PdfService;
using HwInf.Web.Controllers;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace HwInf.UnitTests.Controllers
{
    [TestFixture]
    public class PrintControllersTest : ControllerTests
    {
        private readonly PrintController ctr;


        public PrintControllersTest()
        {
            var log = new Mock<ILogger<PrintController>>();
            ctr = new PrintController(Bl, null, log.Object) {ControllerContext = _controllerContext};
        }

        [Test]
        public void HelloWorld()
        {
        }

        [Test]
        public void ctr_should_return_contract_pdf()
        {
            var obj = Bl.CreateOrder();
            var vmdl = ControllerHelper.GetValidOrderViewModel();
            vmdl.ApplyChanges(obj, Bl);
            var res = ctr.GetPrint(obj.OrderGuid);
            Assert.NotNull(res);
            Assert.NotNull(res.GetType().Name.Equals("document/pdf"));
        }

        [Test]
        public void ctr_should_return_return_pdf()
        {
            var obj = Bl.CreateOrder();
            var vmdl = ControllerHelper.GetValidOrderViewModel();
            vmdl.ApplyChanges(obj, Bl);
            var res = ctr.GetReturn(obj.OrderGuid);
            Assert.NotNull(res);
            Assert.NotNull(res.GetType().Name.Equals("document/pdf"));
        }

    }
}