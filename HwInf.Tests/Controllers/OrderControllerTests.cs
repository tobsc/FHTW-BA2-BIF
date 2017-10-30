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
            ctr = new OrdersController(_bl);

        }
        [Test]
        public void HelloWorld()
        {
        }

        [Test]
        public void ctr_should_create_order()
        {
            var vmdl = ControllerHelper.GetValidOrderViewModel();
            vmdl.OrderId = 76;
            var obj = ctr.PostOrder(vmdl);
            Assert.NotNull(obj);
            var res = obj as OkNegotiatedContentResult<List<OrderViewModel>>;
            Assert.NotNull(res);
            Assert.NotNull(res.Content);
        }

        [Test]
        public void ctr_should_return_orders()
        {
            var vmdl = ControllerHelper.GetValidOrderViewModel();
            vmdl.OrderId = 77;
            var obj = ctr.PostOrder(vmdl);
            Assert.NotNull(obj);
            var res = obj as OkNegotiatedContentResult<List<OrderViewModel>>;
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
            vmdl.OrderId = 78;
            var obj = ctr.PostOrder(vmdl);
            Assert.NotNull(obj);
            var res = obj as OkNegotiatedContentResult<List<OrderViewModel>>;
            Assert.NotNull(res);
            Assert.NotNull(res.Content);

            var objGet = ctr.GetOrdersGuid(res.Content.First().OrderGuid);
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
            var res = obj as OkNegotiatedContentResult<List<OrderViewModel>>;
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
            var res = obj as OkNegotiatedContentResult<List<OrderViewModel>>;
            Assert.NotNull(res);
            Assert.NotNull(res.Content);

            var objGet = ctr.GetOrders(99);
            Assert.NotNull(objGet);
            var objGetRes = objGet as NotFoundResult;
            Assert.NotNull(objGetRes);
        }

        [Test]
        public void ctr_should_update_order_status_to_accept()
        {
            var obj = ControllerHelper.GetValidOrderViewModel();
            Assert.NotNull(obj);

            var req = ctr.PostOrder(obj) as OkNegotiatedContentResult<List<OrderViewModel>>;
            Assert.NotNull(req);
            Assert.True(req.Content.First().OrderStatus.Slug.Equals("offen"));

            var res = ctr.PutOrderAccept(req.Content.First()) as OkNegotiatedContentResult<OrderViewModel>;
            Assert.NotNull(res);
            Assert.True(res.Content.OrderStatus.Slug.Equals("akzeptiert"));
            Assert.True(res.Content.OrderItems.First().Device.Status.Description.Equals("Ausgeliehen"));


        }

        [Test]
        public void ctr_should_update_order_status_to_decline()
        {
            var obj = ControllerHelper.GetValidOrderViewModel();
            Assert.NotNull(obj);

            var req = ctr.PostOrder(obj) as OkNegotiatedContentResult<List<OrderViewModel>>;
            Assert.NotNull(req);
            Assert.True(req.Content.First().OrderStatus.Slug.Equals("offen"));

            var res = ctr.PutOrderDecline(req.Content.First()) as OkNegotiatedContentResult<OrderViewModel>;
            Assert.NotNull(res);
            Assert.True(res.Content.OrderStatus.Slug.Equals("abgelehnt"));

        }

        [Test]
        public void ctr_should_update_order_status_to_return()
        {
            var obj = ControllerHelper.GetValidOrderViewModel();
            Assert.NotNull(obj);

            var req = ctr.PostOrder(obj) as OkNegotiatedContentResult<List<OrderViewModel>>;
            Assert.NotNull(req);
            Assert.True(req.Content.First().OrderStatus.Slug.Equals("offen"));

            var res = ctr.PutOrderReturn(req.Content.First()) as OkNegotiatedContentResult<OrderViewModel>;
            Assert.NotNull(res);
            Assert.True(res.Content.OrderStatus.Slug.Equals("abgeschlossen"));
            Assert.True(res.Content.OrderItems.First().Device.Status.Description.Equals("Verfügbar"));
        }

        [Test]
        public void ctr_should_update_order_status_to_lent()
        {
            var obj = ControllerHelper.GetValidOrderViewModel();
            Assert.NotNull(obj);

            var req = ctr.PostOrder(obj) as OkNegotiatedContentResult<List<OrderViewModel>>;
            Assert.NotNull(req);
            Assert.True(req.Content.First().OrderStatus.Slug.Equals("offen"));

            var res = ctr.PutOrderLend(req.Content.First()) as OkNegotiatedContentResult<OrderViewModel>;
            Assert.NotNull(res);
            Assert.True(res.Content.OrderStatus.Slug.Equals("ausgeliehen"));
        }

        [Test]
        public void ctr_should_update_order_status_to_open()
        {
            var obj = ControllerHelper.GetValidOrderViewModel();
            Assert.NotNull(obj);

            var req = ctr.PostOrder(obj) as OkNegotiatedContentResult<List<OrderViewModel>>;
            Assert.NotNull(req);
            Assert.True(req.Content.First().OrderStatus.Slug.Equals("offen"));

            var res = ctr.PutOrderAccept(req.Content.First()) as OkNegotiatedContentResult<OrderViewModel>;
            Assert.NotNull(res);
            Assert.True(res.Content.OrderStatus.Slug.Equals("akzeptiert"));
            Assert.True(res.Content.OrderItems.First().Device.Status.Description.Equals("Ausgeliehen"));

            var res2 = ctr.PutOrderReset(req.Content.First()) as OkNegotiatedContentResult<OrderViewModel>;
            Assert.NotNull(res2);
            Assert.True(res.Content.OrderStatus.Slug.Equals("offen"));
            Assert.True(res.Content.OrderItems.First().Device.Status.Description.Equals("Verfügbar"));
        }

        [Test]
        public void ctr_should_return_searched_orders_by_uid_admin()
        {
            var obj = ControllerHelper.GetValidOrderViewModel();
            Assert.NotNull(obj);

            var req = ctr.PostOrder(obj) as OkNegotiatedContentResult<List<OrderViewModel>>;
            Assert.NotNull(req);
            Assert.True(req.Content.First().OrderStatus.Slug.Equals("offen"));

            var sQuery = "if15b032";
            var search = ControllerHelper.GetValidOrderSearchViewModel(sQuery, true);
            var res = ctr.PostSearchOrders(search) as OkNegotiatedContentResult<OrderListViewModel>;
            Assert.NotNull(res);
            Assert.True(res.Content.Orders.First().Entleiher.Uid.Equals(sQuery));
        }

        [Test]
        public void ctr_should_return_searched_orders_by_status_admin()
        {
            var obj = ControllerHelper.GetValidOrderViewModel();
            Assert.NotNull(obj);

            var req = ctr.PostOrder(obj) as OkNegotiatedContentResult<List<OrderViewModel>>;
            Assert.NotNull(req);
            Assert.True(req.Content.First().OrderStatus.Slug.Equals("offen"));

            var sQuery = "1";
            var search = ControllerHelper.GetValidOrderSearchViewModel(sQuery, true);
            var res = ctr.PostSearchOrders(search) as OkNegotiatedContentResult<OrderListViewModel>;
            Assert.NotNull(res);
            Assert.True(res.Content.Orders.Any(i => i.OrderId.Equals(Int32.Parse(sQuery))));
        }

        [Test]
        public void ctr_should_return_searched_orders_by_uid_user()
        {
            var obj = ControllerHelper.GetValidOrderViewModel();
            Assert.NotNull(obj);

            var req = ctr.PostOrder(obj) as OkNegotiatedContentResult<List<OrderViewModel>>;
            Assert.NotNull(req);
            Assert.True(req.Content.First().OrderStatus.Slug.Equals("offen"));

            var sQuery = "if15b042";
            var search = ControllerHelper.GetValidOrderSearchViewModel(sQuery, false);
            var res = ctr.PostSearchOrders(search) as OkNegotiatedContentResult<OrderListViewModel>;
            Assert.NotNull(res);
            Assert.True(res.Content.Orders.First().Verwalter.Uid.Equals(sQuery));
        }

        [Test]
        public void ctr_should_return_searched_orders_by_id_user()
        {
            var obj = ControllerHelper.GetValidOrderViewModel();
            Assert.NotNull(obj);

            var req = ctr.PostOrder(obj) as OkNegotiatedContentResult<List<OrderViewModel>>;
            Assert.NotNull(req);
            Assert.True(req.Content.First().OrderStatus.Slug.Equals("offen"));

            var sQuery = "1";
            var search = ControllerHelper.GetValidOrderSearchViewModel(sQuery, false);
            var res = ctr.PostSearchOrders(search) as OkNegotiatedContentResult<OrderListViewModel>;
            Assert.NotNull(res);
            Assert.True(res.Content.Orders.Any(i => i.OrderId.Equals(Int32.Parse(sQuery))));
        }

        [Test]
        public void ctr_should_return_searched_orders_by_last_name_user()
        {
            var obj = ControllerHelper.GetValidOrderViewModel();
            Assert.NotNull(obj);

            var req = ctr.PostOrder(obj) as OkNegotiatedContentResult<List<OrderViewModel>>;
            Assert.NotNull(req);
            Assert.True(req.Content.First().OrderStatus.Slug.Equals("offen"));

            var sQuery = "Calanog";
            var search = ControllerHelper.GetValidOrderSearchViewModel(sQuery, false);
            var res = ctr.PostSearchOrders(search) as OkNegotiatedContentResult<OrderListViewModel>;
            Assert.NotNull(res);
            Assert.True(res.Content.Orders.Any(i => i.OrderItems.First().Device.Verwalter.LastName.Equals(sQuery)));
        }

    }
}

