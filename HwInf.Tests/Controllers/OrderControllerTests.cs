using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Results;
using HwInf.Common.BL;
using HwInf.Common.DAL;
using HwInf.Controllers;
using HwInf.ViewModels;
using NUnit.Framework;
using HwInf.Common.Models;
using HwInf.Tests.DAL;

namespace HwInf.Tests.Controllers
{
    [TestFixture]
    public class OrderControllerTests : ControllerTests
    {
        private readonly OrdersController ctr;

        public OrderControllerTests()
        {
            ctr = new OrdersController(_dal);

        }
        [Test]
        public void HelloWorld()
        {
        }

        [Test]
        public void ctr_should_create_order()
        {
            var vmdl = ControllerHelper.GetValidOrderViewModel();
            var obj = ctr.PostOrder(vmdl);
            Assert.NotNull(obj);
            var res = obj as OkNegotiatedContentResult<OrderViewModel>;
            Assert.NotNull(res);
            Assert.NotNull(res.Content);
        }

        [Test]
        public void ctr_should_return_orders()
        {
            var vmdl = ControllerHelper.GetValidOrderViewModel();
            var obj = ctr.PostOrder(vmdl);
            Assert.NotNull(obj);
            var res = obj as OkNegotiatedContentResult<OrderViewModel>;
            Assert.NotNull(res);
            Assert.NotNull(res.Content);

            var objGet = ctr.GetOrders();
            Assert.NotNull(objGet);
            var objGetRes = objGet as OkNegotiatedContentResult<List<OrderViewModel>>;
            Assert.NotNull(objGetRes);
            Assert.NotNull(objGetRes.Content);
        }

        [Test]
        public void ctr_should_return_order_by_guid()
        {
            var vmdl = ControllerHelper.GetValidOrderViewModel();
            var obj = ctr.PostOrder(vmdl);
            Assert.NotNull(obj);
            var res = obj as OkNegotiatedContentResult<OrderViewModel>;
            Assert.NotNull(res);
            Assert.NotNull(res.Content);

            var objGet = ctr.GetOrdersGuid(res.Content.OrderGuid);
            Assert.NotNull(objGet);
            var objGetRes = objGet as OkNegotiatedContentResult<OrderViewModel>;
            Assert.NotNull(objGetRes);
            Assert.NotNull(objGetRes.Content);
        }

        [Test]
        public void ctr_should_return_not_found_when_order_not_found_by_guid()
        {
            var vmdl = ControllerHelper.GetValidOrderViewModel();
            var obj = ctr.PostOrder(vmdl);
            Assert.NotNull(obj);
            var res = obj as OkNegotiatedContentResult<OrderViewModel>;
            Assert.NotNull(res);
            Assert.NotNull(res.Content);

            var objGet = ctr.GetOrdersGuid(Guid.NewGuid());
            Assert.NotNull(objGet);
            var objGetRes = objGet as NotFoundResult;
            Assert.NotNull(objGetRes);
        }

        [Test]
        public void ctr_should_return_not_found_when_order_not_found_by_id()
        {
            var vmdl = ControllerHelper.GetValidOrderViewModel();
            var obj = ctr.PostOrder(vmdl);
            Assert.NotNull(obj);
            var res = obj as OkNegotiatedContentResult<OrderViewModel>;
            Assert.NotNull(res);
            Assert.NotNull(res.Content);

            var objGet = ctr.GetOrders(99);
            Assert.NotNull(objGet);
            var objGetRes = objGet as NotFoundResult;
            Assert.NotNull(objGetRes);
        }
    }
}
