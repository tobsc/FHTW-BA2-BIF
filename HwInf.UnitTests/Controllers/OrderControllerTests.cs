using System;
using System.Collections.Generic;
using System.Linq;
using HwInf.Web.Controllers;
using HwInf.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace HwInf.UnitTests.Controllers
{
    [TestFixture]
    public class OrderControllerTests : ControllerTests
    {
        private readonly OrdersController ctr;

        public OrderControllerTests()
        {
            var log = new Mock<ILogger<OrdersController>>();
            ctr = new OrdersController(Bl, log.Object) {ControllerContext = _controllerContext};

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
            var response = obj as OkObjectResult;
            var res = response?.Value as List<OrderViewModel>;
            Assert.NotNull(res);
        }

        [Test]
        public void ctr_should_return_orders()
        {
            var vmdl = ControllerHelper.GetValidOrderViewModel();
            vmdl.OrderId = 77;
            var obj = ctr.PostOrder(vmdl);
            Assert.NotNull(obj);
            var response = obj as OkObjectResult;
            var res = response?.Value as List<OrderViewModel>;
            Assert.NotNull(res);

            var objGet = ctr.GetOrders();
            Assert.NotNull(objGet);
            var objGetResponse = objGet as OkObjectResult;
            var objGetRes = objGetResponse?.Value as List<OrderViewModel>;
            Assert.NotNull(objGetRes);
        }

        [Test]
        public void ctr_should_return_order_by_guid()
        {
            var vmdl = ControllerHelper.GetValidOrderViewModel();
            vmdl.OrderId = 78;
            var obj = ctr.PostOrder(vmdl) as OkObjectResult;
            Assert.NotNull(obj);
            var res = obj.Value as List<OrderViewModel>;
            Assert.NotNull(res);

            var objGet = ctr.GetOrdersGuid(res.First().OrderGuid) as OkObjectResult;
            Assert.NotNull(objGet);
            var objGetRes = objGet.Value as OrderViewModel;
            Assert.NotNull(objGetRes);
        }

        [Test]
        public void ctr_should_return_not_found_when_order_not_found_by_guid()
        {
            var vmdl = ControllerHelper.GetValidOrderViewModel();
            var obj = ctr.PostOrder(vmdl) as OkObjectResult;
            Assert.NotNull(obj);
            var res = obj.Value as List<OrderViewModel>;
            Assert.NotNull(res);

            var objGet = ctr.GetOrdersGuid(Guid.NewGuid());
            Assert.NotNull(objGet);
            var objGetRes = objGet as NotFoundResult;
            Assert.NotNull(objGetRes);
        }

        [Test]
        public void ctr_should_return_not_found_when_order_not_found_by_id()
        {
            var vmdl = ControllerHelper.GetValidOrderViewModel();
            var obj = ctr.PostOrder(vmdl) as OkObjectResult;
            Assert.NotNull(obj);
            var res = obj.Value as List<OrderViewModel>;
            Assert.NotNull(res);

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

            var req = ctr.PostOrder(obj) as OkObjectResult;
            var res = req.Value as List<OrderViewModel>;
            Assert.NotNull(res);
            Assert.True(res.First().OrderStatus.Slug.Equals("offen"));

            var obj2 = ctr.PutOrderAccept(res.First()) as OkObjectResult;
            var res2 = obj2?.Value as OrderViewModel;
            Assert.NotNull(res2);
            Assert.True(res2.OrderStatus.Slug.Equals("akzeptiert"));
            Assert.True(res2.OrderItems.First().Device.Status.Description.Equals("Ausgeliehen"));


        }

        [Test]
        public void ctr_should_update_order_status_to_decline()
        {
            var obj = ControllerHelper.GetValidOrderViewModel();
            Assert.NotNull(obj);

            var req = (ctr.PostOrder(obj) as OkObjectResult)?.Value as List<OrderViewModel>;
            Assert.NotNull(req);
            Assert.True(req.First().OrderStatus.Slug.Equals("offen"));

            var res = (ctr.PutOrderDecline(req.First()) as OkObjectResult)?.Value as OrderViewModel;
            Assert.NotNull(res);
            Assert.True(res.OrderStatus.Slug.Equals("abgelehnt"));

        }

        [Test]
        public void ctr_should_update_order_status_to_return()
        {
            var obj = ControllerHelper.GetValidOrderViewModel();
            Assert.NotNull(obj);

            var req = (ctr.PostOrder(obj) as OkObjectResult)?.Value as List<OrderViewModel>;
            Assert.NotNull(req);
            Assert.True(req.First().OrderStatus.Slug.Equals("offen"));

            var res = (ctr.PutOrderReturn(req.First()) as OkObjectResult)?.Value as OrderViewModel;
            Assert.NotNull(res);
            Assert.True(res.OrderStatus.Slug.Equals("abgeschlossen"));
            Assert.True(res.OrderItems.First().Device.Status.Description.Equals("Verfügbar"));
        }

        [Test]
        public void ctr_should_update_order_status_to_lent()
        {
            var obj = ControllerHelper.GetValidOrderViewModel();
            Assert.NotNull(obj);

            var req = (ctr.PostOrder(obj) as OkObjectResult)?.Value as List<OrderViewModel>;
            Assert.NotNull(req);
            Assert.True(req.First().OrderStatus.Slug.Equals("offen"));

            var res = (ctr.PutOrderLend(req.First()) as OkObjectResult)?.Value as OrderViewModel;
            Assert.NotNull(res);
            Assert.True(res.OrderStatus.Slug.Equals("ausgeliehen"));
        }

        [Test]
        public void ctr_should_update_order_status_to_open()
        {
            var obj = ControllerHelper.GetValidOrderViewModel();
            Assert.NotNull(obj);

            var req = (ctr.PostOrder(obj) as OkObjectResult)?.Value as List<OrderViewModel>;
            Assert.NotNull(req);
            Assert.True(req.First().OrderStatus.Slug.Equals("offen"));

            var res = (ctr.PutOrderAccept(req.First()) as OkObjectResult)?.Value as OrderViewModel;
            Assert.NotNull(res);
            Assert.True(res.OrderStatus.Slug.Equals("akzeptiert"));
            Assert.True(res.OrderItems.First().Device.Status.Description.Equals("Ausgeliehen"));

            var res2 = (ctr.PutOrderReset(req.First()) as OkObjectResult)?.Value as OrderViewModel;
            Assert.NotNull(res2);
            Assert.True(res.OrderStatus.Slug.Equals("offen"));
            Assert.True(res.OrderItems.First().Device.Status.Description.Equals("Verfügbar"));
        }

        [Test]
        public void ctr_should_return_searched_orders_by_uid_admin()
        {
            var obj = ControllerHelper.GetValidOrderViewModel();
            Assert.NotNull(obj);

            var req = (ctr.PostOrder(obj) as OkObjectResult)?.Value as List<OrderViewModel>;
            Assert.NotNull(req);
            Assert.True(req.First().OrderStatus.Slug.Equals("offen"));

            var sQuery = "if15b032";
            var search = ControllerHelper.GetValidOrderSearchViewModel(sQuery, true);
            var res = (ctr.PostSearchOrders(search) as OkObjectResult)?.Value as OrderListViewModel;
            Assert.NotNull(res);
            Assert.True(res.Orders.First().Entleiher.Uid.Equals(sQuery));
        }

        [Test]
        public void ctr_should_return_searched_orders_by_status_admin()
        {
            var obj = ControllerHelper.GetValidOrderViewModel();
            Assert.NotNull(obj);

            var req = (ctr.PostOrder(obj) as OkObjectResult)?.Value as List<OrderViewModel>;
            Assert.NotNull(req);
            Assert.True(req.First().OrderStatus.Slug.Equals("offen"));

            var sQuery = "1";
            var search = ControllerHelper.GetValidOrderSearchViewModel(sQuery, true);
            var res = (ctr.PostSearchOrders(search) as OkObjectResult)?.Value as OrderListViewModel;
            Assert.NotNull(res);
            Assert.True(res.Orders.Any(i => i.OrderId.Equals(Int32.Parse(sQuery))));
        }

        [Test]
        public void ctr_should_return_searched_orders_by_uid_user()
        {
            var obj = ControllerHelper.GetValidOrderViewModel();
            Assert.NotNull(obj);

            var req = (ctr.PostOrder(obj) as OkObjectResult)?.Value as List<OrderViewModel>;
            Assert.NotNull(req);
            Assert.True(req.First().OrderStatus.Slug.Equals("offen"));

            var sQuery = "if15b042";
            var search = ControllerHelper.GetValidOrderSearchViewModel(sQuery, false);
            var res = (ctr.PostSearchOrders(search) as OkObjectResult)?.Value as OrderListViewModel;
            Assert.NotNull(res);
            Assert.True(res.Orders.First().Verwalter.Uid.Equals(sQuery));
        }

        [Test]
        public void ctr_should_return_searched_orders_by_id_user()
        {
            var obj = ControllerHelper.GetValidOrderViewModel();
            Assert.NotNull(obj);

            var req = (ctr.PostOrder(obj) as OkObjectResult)?.Value as List<OrderViewModel>;
            Assert.NotNull(req);
            Assert.True(req.First().OrderStatus.Slug.Equals("offen"));

            var sQuery = "1";
            var search = ControllerHelper.GetValidOrderSearchViewModel(sQuery, false);
            var res = (ctr.PostSearchOrders(search) as OkObjectResult)?.Value as OrderListViewModel;
            Assert.NotNull(res);
            Assert.True(res.Orders.Any(i => i.OrderId.Equals(Int32.Parse(sQuery))));
        }

        [Test]
        public void ctr_should_return_searched_orders_by_last_name_user()
        {
            var obj = ControllerHelper.GetValidOrderViewModel();
            Assert.NotNull(obj);

            var req = (ctr.PostOrder(obj) as OkObjectResult)?.Value as List<OrderViewModel>;
            Assert.NotNull(req);
            Assert.True(req.First().OrderStatus.Slug.Equals("offen"));

            var sQuery = "Calanog";
            var search = ControllerHelper.GetValidOrderSearchViewModel(sQuery, false);
            var res = (ctr.PostSearchOrders(search) as OkObjectResult)?.Value as OrderListViewModel;
            Assert.NotNull(res);
            Assert.True(res.Orders.Any(i => i.OrderItems.First().Device.Verwalter.LastName.Equals(sQuery)));
        }

    }
}

