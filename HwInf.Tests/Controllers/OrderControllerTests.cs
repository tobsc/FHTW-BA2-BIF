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

        [Test]
        public void ctr_should_update_order_items_status()
        {

            var obj = _bl.CreateOrderItem();
            Assert.NotNull(obj);
            obj.ItemId = 1;
            obj.Device = _bl.GetSingleDevice("a5123");
            obj.OrderStatus = _bl.GetOrderStatus("offen");

            var objGet = _bl.GetOrderItem(1);
            Assert.NotNull(objGet);

            var res = ctr.PutOrderItem(objGet.ItemId, "akzeptiert") as OkNegotiatedContentResult<OrderItemViewModel>;
            Assert.NotNull(res);
            Assert.NotNull(res.Content);

            var objGetEdit = _bl.GetOrderItem(1);
            Assert.True(objGetEdit.OrderStatus.Slug.Equals("akzeptiert"));
        }

        [Test]
        public void ctr_should_return_not_found_on_edit_when_order_status_not_found()
        {

            var obj = _bl.CreateOrderItem();
            Assert.NotNull(obj);
            obj.ItemId = 2;
            obj.Device = _bl.GetSingleDevice("a5123");
            obj.OrderStatus = _bl.GetOrderStatus("offen");

            var objGet = _bl.GetOrderItem(2);
            Assert.NotNull(objGet);

            var res = ctr.PutOrderItem(objGet.ItemId, "test") as NotFoundResult;
            Assert.NotNull(res);

        }

        [Test]
        public void ctr_should_return_not_found_on_edit_when_order_item_not_found()
        {
            var res = ctr.PutOrderItem(78, "akzeptiert") as NotFoundResult;
            Assert.NotNull(res);
        }
    }
}

