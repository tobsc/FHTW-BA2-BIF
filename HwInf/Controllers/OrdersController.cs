using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using HwInf.Common.DAL;
using HwInf.ViewModels;
using HwInf.Common;
using System.IO;
using MigraDoc.DocumentObjectModel.IO;
using MigraDoc.Rendering;
using System.Net.Http.Headers;
using System.Security;
using HwInf.Common.BL;
using log4net;


namespace HwInf.Controllers
{
    [Authorize]
    [RoutePrefix("api/orders")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class OrdersController : ApiController
    {
        private readonly IDAL _db;
        private readonly BL _bl;
        private readonly ILog _log = LogManager.GetLogger(typeof(OrdersController).Name);

        public OrdersController()
        {
            _db = new HwInfContext();
            _bl = new BL(_db);
        }

        public OrdersController(IDAL db)
        {
            _db = db;
            _bl = new BL(db);
        }

        [ResponseType(typeof(OrderViewModel))]
    
        [Route("filter")]
        public IHttpActionResult PostFilter([FromBody] OrderFilterViewModel vmdl)
        {
            try
            {
                var result = vmdl.FilteredList(_bl).Select(i => new OrderViewModel(i)).ToList();
                var count = result.Count;

                return Ok(new OrderListViewModel(result.Skip(vmdl.Offset).Take(vmdl.Limit), vmdl.Limit, count));
            }
            catch (SecurityException)
            {
                _log.ErrorFormat("'{0}' tried to list Admin/Verwalter View from Orders", _bl.GetCurrentUid());
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: {0}", ex.Message);
                return InternalServerError();
            }
        }

        /// <summary>
        /// Get Orders
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(OrderViewModel))]
        [Route("")]
        [Authorize(Roles = "Admin, Verwalter")]
        public IHttpActionResult GetOrders()
        {
            try
            {
                var orders = _bl.GetOrders()
                    .ToList()
                    .Select(i => new OrderViewModel(i).LoadOrderItems(i))
                    .ToList();

                return Ok(orders);
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: {0}", ex.Message);
                return InternalServerError();
            }
        }

        /// <summary>
        /// Get Single Order
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(OrderViewModel))]
        [Route("id/{id}")]
        public IHttpActionResult GetOrders(int id)
        {
            try
            {
                var order = _bl.GetOrders(id);

                if (order == null)
                {
                    _log.WarnFormat("Not Found: Order '{0}' not found", id);
                    return NotFound();
                }

                var vmdl = new OrderViewModel(order).LoadOrderItems(order);

                return Ok(vmdl);
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: {0}", ex.Message);
                return InternalServerError();
            }
        }

        /// <summary>
        /// Get OrderStatus
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(OrderViewModel))]
        [Route("orderstatus")]
        public IHttpActionResult GetOrderStatus()
        {
            try
            {
                var vmdl = _bl.GetOrderStatus()
                    .ToList();

                return Ok(vmdl);
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: {0}", ex.Message);
                return InternalServerError();
            }
        }


        /// <summary>
        /// Get Single Order by Guid
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [ResponseType(typeof(OrderViewModel))]
        [Route("guid/{guid}")]
        public IHttpActionResult GetOrdersGuid(Guid guid)
        {
            try
            {
                var order = _bl.GetOrders(guid);

                if (order == null)
                {
                    _log.WarnFormat("Not Found: Order '{0}' not found", guid);
                    return NotFound();
                }

                var vmdl = new OrderViewModel(order).LoadOrderItems(order);

                return Ok(vmdl);
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: {0}", ex.Message);
                return InternalServerError();
            }
        }

        /// <summary>
        /// Create Order
        /// </summary>
        /// <param name="vmdl"></param>
        /// <returns></returns>
        [ResponseType(typeof(OrderViewModel))]
        [Route("")]
        public IHttpActionResult PostOrder([FromBody]OrderViewModel vmdl)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var vmdls = SplitOrders(vmdl);

                vmdls.ForEach(i =>
                {
                    var order = _bl.CreateOrder();
                    i.ApplyChanges(order, _bl);
                    i.LoadOrderItems(order).Refresh(order);
                });

                _bl.SaveChanges();
                vmdls.ForEach(i =>
                {
                    _log.InfoFormat("Order '{0}' created by '{1}'", i.OrderGuid, User.Identity.Name);
                });

                return Ok(vmdls);

            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: {0}", ex.Message);
                return InternalServerError();
            }
        }

        /// <summary>
        /// Accept Order
        /// </summary>
        /// <param name="vmdl"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Verwalter")]
        [ResponseType(typeof(OrderItemViewModel))]
        [Route("order/accept/")]
        public IHttpActionResult PutOrderAccept([FromBody]OrderViewModel vmdl)
        {
            try
            {
                var order = _bl.GetOrders(vmdl.OrderId);
                var orderItems = order.OrderItems.ToList();
                var changed = vmdl.OrderItems
                    .Where(i => i.IsDeclined)
                    .Select(i => i.ItemId)
                    .ToList();

                orderItems
                    .Where(i => changed.Contains(i.ItemId))
                    .ToList()
                    .ForEach(i => i.IsDeclined = true);


                vmdl.Accept(order, _bl);
                _bl.SaveChanges();
                vmdl.Refresh(order);

                return Ok(vmdl);
            }
            catch (SecurityException)
            {
                _log.ErrorFormat("Security: '{0}' tried to update Order '{1}'", _bl.GetCurrentUid(), vmdl.OrderId);
                return Unauthorized();
            }

            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: '{0}'", ex.Message);
                return InternalServerError();
            }
        }

        /// <summary>
        /// Lend Order
        /// </summary>
        /// <param name="vmdl"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Verwalter")]
        [ResponseType(typeof(OrderItemViewModel))]
        [Route("order/lend/")]
        public IHttpActionResult PutOrderLend([FromBody]OrderViewModel vmdl)
        {
            try
            {
                var order = _bl.GetOrders(vmdl.OrderId);

                vmdl.Lend(order, _bl);
                _bl.SaveChanges();
                vmdl.Refresh(order);

                return Ok(vmdl);
            }
            catch (SecurityException)
            {
                _log.ErrorFormat("Security: '{0}' tried to update Order '{1}'", _bl.GetCurrentUid(), vmdl.OrderId);
                return Unauthorized();
            }

            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: '{0}'", ex.Message);
                return InternalServerError();
            }
        }

        /// <summary>
        /// Reset Order
        /// </summary>
        /// <param name="vmdl"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Verwalter")]
        [ResponseType(typeof(OrderItemViewModel))]
        [Route("order/reset/")]
        public IHttpActionResult PutOrderReset([FromBody]OrderViewModel vmdl)
        {
            try
            {
                var order = _bl.GetOrders(vmdl.OrderId);

                vmdl.Reset(order, _bl);
                _bl.SaveChanges();
                vmdl.Refresh(order);

                return Ok(vmdl);
            }
            catch (SecurityException)
            {
                _log.ErrorFormat("Security: '{0}' tried to update Order '{1}'", _bl.GetCurrentUid(), vmdl.OrderId);
                return Unauthorized();
            }

            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: '{0}'", ex.Message);
                return InternalServerError();
            }
        }

        /// <summary>
        /// Decline Order
        /// </summary>
        /// <param name="vmdl"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Verwalter")]
        [ResponseType(typeof(OrderItemViewModel))]
        [Route("order/decline/")]
        public IHttpActionResult PutOrderDecline([FromBody]OrderViewModel vmdl)
        {
            try
            {
                var order = _bl.GetOrders(vmdl.OrderId);

                vmdl.Decline(order, _bl);
                _bl.SaveChanges();
                vmdl.Refresh(order);

                return Ok(vmdl);
            }
            catch (SecurityException)
            {
                _log.ErrorFormat("Security: '{0}' tried to update Order '{1}'", _bl.GetCurrentUid(), vmdl.OrderId);
                return Unauthorized();
            }

            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: '{0}'", ex.Message);
                return InternalServerError();
            }
        }

        /// <summary>
        /// Return Order
        /// </summary>
        /// <param name="vmdl"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Verwalter")]
        [ResponseType(typeof(OrderItemViewModel))]
        [Route("order/return/")]
        public IHttpActionResult PutOrderReturn([FromBody]OrderViewModel vmdl)
        {
            try
            {
                var order = _bl.GetOrders(vmdl.OrderId);

                vmdl.Return(order, _bl);
                _bl.SaveChanges();
                vmdl.Refresh(order);

                return Ok(vmdl);
            }
            catch (SecurityException)
            {
                _log.ErrorFormat("Security: '{0}' tried to update Order '{1}'", _bl.GetCurrentUid(), vmdl.OrderId);
                return Unauthorized();
            }

            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: '{0}'", ex.Message);
                return InternalServerError();
            }
        }

        /// <summary>
        /// Search Orders
        /// </summary>
        /// <param name="vmdl"></param>
        /// <returns></returns>
        [ResponseType(typeof(OrderItemViewModel))]
        [Route("search")]
        public IHttpActionResult PostSearchOrders([FromBody]OrderSearchViewModel vmdl)
        {
            try
            {
                var result = vmdl.Seach(_bl)
                    .Select(i => new OrderViewModel(i))
                    .ToList();
                var count = result.Count;

                return Ok(new OrderListViewModel(result.Skip(vmdl.Offset).Take(vmdl.Limit), vmdl.Limit, count));
            }
            catch (SecurityException)
            {
                _log.ErrorFormat("Security: '{0}' tried to view Orders as Admin/Verwalter'", _bl.GetCurrentUid());
                return Unauthorized();
            }

            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: '{0}'", ex.Message);
                return InternalServerError();
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }


        private static MemoryStream CreateMDDLStream(string text)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter sw = new StreamWriter(stream);

            sw.Write(text);

            sw.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        private List<OrderViewModel> SplitOrders(OrderViewModel vmdl)
        {

            // Load Devices
            vmdl.OrderItems
                .ToList()
                .ForEach(x => x.Device = new DeviceViewModel(_bl.GetSingleDevice(x.Device.InvNum)));

            // Group by Verwalter
            var groupedOrderItems = vmdl.OrderItems.GroupBy(i => i.Device.Verwalter.Uid).Select(x => x.ToList()).ToList();

            // Create vmdls
            var vmdls = new List<OrderViewModel>();
            groupedOrderItems.ForEach(i =>
            {
                var tmp = new OrderViewModel(vmdl) {OrderItems = i.ToList()};
                vmdls.Add(tmp);
            });

            return vmdls;

        }
    }
}