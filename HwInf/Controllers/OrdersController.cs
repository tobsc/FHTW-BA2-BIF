using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using HwInf.Common.DAL;
using HwInf.ViewModels;
using HwInf.Common;
using System.Security;
using HwInf.Common.BL;
using log4net;
using System.Threading.Tasks;

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

        /// <summary>
        /// Filter Orders
        /// </summary>
        /// <remarks>
        /// Returns a filtered List of &#x60;OrderListViewModel&#x60;
        /// See &#x60;OrderFilterViewModel&#x60; so get available filter options
        /// </remarks>
        /// <response code="200"></response>
        /// <response code="401">Unauthorized attempt to filter Orders</response>
        /// <response code="500">An error occured, please read log files</response>
        /// <param name="orderFilterViewModel">&#x60;OrderFilterViewModel&#x60;</param>
        [Route("filter")]
        public IHttpActionResult PostFilter([FromBody] OrderFilterViewModel orderFilterViewModel)
        {
            try
            {
                var result = orderFilterViewModel.FilteredList(_bl).Select(i => new OrderViewModel(i)).ToList();
                var count = result.Count;

                return Ok(new OrderListViewModel(result.Skip(orderFilterViewModel.Offset).Take(orderFilterViewModel.Limit), orderFilterViewModel.Limit, count));
            }
            catch (SecurityException)
            {
                _log.WarnFormat("'{0}' tried to list Admin/Verwalter View from Orders", _bl.GetCurrentUid());
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: {0}", ex);
                return InternalServerError();
            }
        }

        /// <summary>
        /// Get Orders
        /// </summary>
        /// <remarks>
        /// Returns a list of all &#x60;Orders&#x60;
        /// </remarks>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
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
                _log.ErrorFormat("Exception: {0}", ex);
                return InternalServerError();
            }
        }

        /// <summary>
        /// Get Single Order
        /// </summary>
        /// <remarks>
        /// Returns a single &#x60;Order&#x60;
        /// </remarks>
        /// <param name="orderId">Id of the &#x60;Order&#x60;</param>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
        [ResponseType(typeof(OrderViewModel))]
        [Route("id/{id}")]
        public IHttpActionResult GetOrders(int orderId)
        {
            try
            {
                var order = _bl.GetOrders(orderId);

                if (order == null)
                {
                    _log.WarnFormat("Not Found: Order '{0}' not found", orderId);
                    return NotFound();
                }

                var vmdl = new OrderViewModel(order).LoadOrderItems(order);

                return Ok(vmdl);
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: {0}", ex);
                return InternalServerError();
            }
        }

        /// <summary>
        /// Get OrderStatus
        /// </summary>
        /// <returns></returns>
        /// <summary>
        /// Get &#x60;OrderStatus&#x60;
        /// </summary>
        /// <remarks>
        /// Returns a List of all &#x60;OrderStatus&#x60;
        /// </remarks>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
        [ResponseType(typeof(List<OrderStatusViewModel>))]
        [Route("orderstatus")]
        public IHttpActionResult GetOrderStatus()
        {
            try
            {
                var vmdl = _bl.GetOrderStatus()
                    .Select(i => new OrderStatusViewModel(i))
                    .ToList();

                return Ok(vmdl);
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: {0}", ex);
                return InternalServerError();
            }
        }


        /// <summary>
        /// Get Single Order
        /// </summary>
        /// <remarks>
        /// Returns a single &#x60;Order&#x60;
        /// </remarks>
        /// <param name="orderGuid">Guid of the &#x60;Order&#x60;</param>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
        [ResponseType(typeof(OrderViewModel))]
        [Route("guid/{guid}")]
        public IHttpActionResult GetOrdersGuid(Guid orderGuid)
        {
            try
            {
                var order = _bl.GetOrders(orderGuid);

                if (order == null)
                {
                    _log.WarnFormat("Not Found: Order '{0}' not found", orderGuid);
                    return NotFound();
                }

                var vmdl = new OrderViewModel(order).LoadOrderItems(order);

                return Ok(vmdl);
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: {0}", ex);
                return InternalServerError();
            }
        }


        /// <summary>
        /// Create Order
        /// </summary>
        /// <remarks>
        /// Create a new Order &#x60;Order&#x60;
        /// </remarks>
        /// <param name="orderVmdl">&#x60;OrderViewmodel&#x60; of the new &#x60;Order&#x60;</param>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
        [ResponseType(typeof(OrderViewModel))]
        [Route("")]
        public IHttpActionResult PostOrder([FromBody]OrderViewModel orderVmdl)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var vmdls = SplitOrders(orderVmdl);

                vmdls.ForEach(i =>
                {
                    var order = _bl.CreateOrder();
                    i.ApplyChanges(order, _bl);
                    i.LoadOrderItems(order).Refresh(order);
                });

                _bl.SaveChanges();
                vmdls.ForEach(i =>
                {
                    Task task = new Task(() => SendMail(i));
                    task.Start();
                    _log.InfoFormat("Order '{0}' created by '{1}'", i.OrderGuid, User.Identity.Name);
                });

                
                return Ok(vmdls);

            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: {0}", ex);
                return InternalServerError();
            }
        }

        /// <summary>
        /// Accept Order
        /// </summary>
        /// <remarks>
        /// Accept a created &#x60;Order&#x60;
        /// </remarks>
        /// <param name="orderVmdl">&#x60;OrderViewmodel&#x60; of the &#x60;Order&#x60;</param>
        /// <response code="200"></response>
        /// <response code="401">Unauthorized attempt to accept an Order</response>
        /// <response code="500">An error occured, please read log files</response>
        [Authorize(Roles = "Admin, Verwalter")]
        [ResponseType(typeof(OrderViewModel))]
        [Route("order/accept/")]
        public IHttpActionResult PutOrderAccept([FromBody]OrderViewModel orderVmdl)
        {
            try
            {
                var order = _bl.GetOrders(orderVmdl.OrderId);
                var orderItems = order.OrderItems.ToList();
                var changed = orderVmdl.OrderItems
                    .Where(i => i.IsDeclined)
                    .Select(i => i.ItemId)
                    .ToList();

                orderItems
                    .Where(i => changed.Contains(i.ItemId))
                    .ToList()
                    .ForEach(i => i.IsDeclined = true);


                orderVmdl.Accept(order, _bl);
                _bl.SaveChanges();
                orderVmdl.LoadOrderItems(order).Refresh(order);

                ////////////////Mail Part
                Task task = new Task(() => SendMail(orderVmdl));
                task.Start();

                return Ok(orderVmdl);
            }
            catch (SecurityException)
            {
                _log.WarnFormat("Security: '{0}' tried to update Order '{1}'", _bl.GetCurrentUid(), orderVmdl.OrderId);
                return Unauthorized();
            }

            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: '{0}'", ex);
                return InternalServerError();
            }
        }

        /// <summary>
        /// Lend Order
        /// </summary>
        /// <remarks>
        /// Lend an &#x60;Order&#x60;, sets its &#x60;OrderStatus&#x60; to lent
        /// </remarks>
        /// <param name="orderVmdl">&#x60;OrderViewmodel&#x60; of the &#x60;Order&#x60;</param>
        /// <response code="200"></response>
        /// <response code="401">Unauthorized attempt to lend an Order</response>
        /// <response code="500">An error occured, please read log files</response>
        [Authorize(Roles = "Admin, Verwalter")]
        [ResponseType(typeof(OrderItemViewModel))]
        [Route("order/lend/")]
        public IHttpActionResult PutOrderLend([FromBody]OrderViewModel orderVmdl)
        {
            try
            {
                var order = _bl.GetOrders(orderVmdl.OrderId);

                orderVmdl.Lend(order, _bl);
                _bl.SaveChanges();
                orderVmdl.LoadOrderItems(order).Refresh(order);

                return Ok(orderVmdl);
            }
            catch (SecurityException)
            {
                _log.WarnFormat("Security: '{0}' tried to update Order '{1}'", _bl.GetCurrentUid(), orderVmdl.OrderId);
                return Unauthorized();
            }

            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: '{0}'", ex);
                return InternalServerError();
            }
        }


        /// <summary>
        /// Reset Order
        /// </summary>
        /// <remarks>
        /// Resets an &#x60;Order&#x60;, sets its &#x60;OrderStatus&#x60; to open
        /// </remarks>
        /// <param name="orderVmdl">&#x60;OrderViewmodel&#x60; of the &#x60;Order&#x60;</param>
        /// <response code="200"></response>
        /// <response code="401">Unauthorized attempt to reset an Order</response>
        /// <response code="500">An error occured, please read log files</response>
        [Authorize(Roles = "Admin, Verwalter")]
        [ResponseType(typeof(OrderItemViewModel))]
        [Route("order/reset/")]
        public IHttpActionResult PutOrderReset([FromBody]OrderViewModel orderVmdl)
        {
            try
            {
                var order = _bl.GetOrders(orderVmdl.OrderId);

                orderVmdl.Reset(order, _bl);
                _bl.SaveChanges();

                orderVmdl.LoadOrderItems(order).Refresh(order);

                return Ok(orderVmdl);
            }
            catch (SecurityException)
            {
                _log.WarnFormat("Security: '{0}' tried to update Order '{1}'", _bl.GetCurrentUid(), orderVmdl.OrderId);
                return Unauthorized();
            }

            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: '{0}'", ex);
                return InternalServerError();
            }
        }

        /// <summary>
        /// Decline Order
        /// </summary>
        /// <remarks>
        /// Decline an &#x60;Order&#x60;, sets its &#x60;OrderStatus&#x60; to declined
        /// </remarks>
        /// <param name="orderVmdl">&#x60;OrderViewmodel&#x60; of the &#x60;Order&#x60;</param>
        /// <response code="200"></response>
        /// <response code="401">Unauthorized attempt to decline an Order</response>
        /// <response code="500">An error occured, please read log files</response>
        [Authorize(Roles = "Admin, Verwalter")]
        [ResponseType(typeof(OrderItemViewModel))]
        [Route("order/decline/")]
        public IHttpActionResult PutOrderDecline([FromBody]OrderViewModel orderVmdl)
        {
            try
            {
                var order = _bl.GetOrders(orderVmdl.OrderId);

                orderVmdl.Decline(order, _bl);
                _bl.SaveChanges();
                orderVmdl.LoadOrderItems(order).Refresh(order);

                ////////////////Mail Part
                Task task = new Task(() => SendMail(orderVmdl));
                task.Start();

                return Ok(orderVmdl);
            }
            catch (SecurityException)
            {
                _log.WarnFormat("Security: '{0}' tried to update Order '{1}'", _bl.GetCurrentUid(), orderVmdl.OrderId);
                return Unauthorized();
            }

            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: '{0}'", ex);
                return InternalServerError();
            }
        }

        /// <summary>
        /// Return Order
        /// </summary>
        /// <remarks>
        /// Return an &#x60;Order&#x60;, sets its &#x60;OrderStatus&#x60; to finished
        /// </remarks>
        /// <param name="orderVmdl">&#x60;OrderViewmodel&#x60; of the &#x60;Order&#x60;</param>
        /// <response code="200"></response>
        /// <response code="401">Unauthorized attempt to return an Order</response>
        /// <response code="500">An error occured, please read log files</response>
        [Authorize(Roles = "Admin, Verwalter")]
        [ResponseType(typeof(OrderItemViewModel))]
        [Route("order/return/")]
        public IHttpActionResult PutOrderReturn([FromBody]OrderViewModel orderVmdl)
        {
            try
            {
                var order = _bl.GetOrders(orderVmdl.OrderId);

                orderVmdl.Return(order, _bl);
                _bl.SaveChanges();
                orderVmdl.LoadOrderItems(order).Refresh(order);

                return Ok(orderVmdl);
            }
            catch (SecurityException)
            {
                _log.WarnFormat("Security: '{0}' tried to update Order '{1}'", _bl.GetCurrentUid(), orderVmdl.OrderId);
                return Unauthorized();
            }

            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: '{0}'", ex);
                return InternalServerError();
            }
        }

        /// <summary>
        /// Abort Order
        /// </summary>
        /// <remarks>
        /// A User can abort an &#x60;Order&#x60; before it gets processed, sets its &#x60;OrderStatus&#x60; to aborted
        /// </remarks>
        /// <param name="orderVmdl">&#x60;OrderViewmodel&#x60; of the &#x60;Order&#x60;</param>
        /// <response code="200"></response>
        /// <response code="401">Unauthorized attempt to abort an Order</response>
        /// <response code="500">An error occured, please read log files</response>
        [Authorize]
        [ResponseType(typeof(OrderItemViewModel))]
        [Route("order/abort/")]
        public IHttpActionResult PutOrderAbort([FromBody]OrderViewModel orderVmdl)
        {
            try
            {
                var order = _bl.GetOrders(orderVmdl.OrderId);
                if (!order.Entleiher.Uid.Equals(_bl.GetCurrentUid()) || !order.OrderStatus.Slug.Equals("offen"))
                {
                    _log.WarnFormat("'{0}' failed to abort Order '{1}'", _bl.GetCurrentUid(), orderVmdl.OrderId);
                    return BadRequest("Der Auftrag kann nicht abgebrochen werden.");
                }
                orderVmdl.Abort(order, _bl);
                _bl.SaveChanges();
                _log.InfoFormat("'{0}' successfully aborted Order '{1}'", _bl.GetCurrentUid(), orderVmdl.OrderId);
                orderVmdl.Refresh(order);

                return Ok(orderVmdl);
            }
            catch (SecurityException)
            {
                _log.WarnFormat("Security: '{0}' tried to abort Order '{1}'", _bl.GetCurrentUid(), orderVmdl.OrderId);
                return Unauthorized();
            }

            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: '{0}'", ex);
                return InternalServerError();
            }
        }

        /// <summary>
        /// Search Order
        /// </summary>
        /// <remarks>
        /// Search &#x60;Orders&#x60;
        /// For search parameters look at &#x60;OrderSearchViewModel&#x60;
        /// </remarks>
        /// <param name="orderSearchVmdl">&#x60;OrderSearchViewmodel&#x60; with search parameters</param>
        /// <response code="200"></response>
        /// <response code="401">Unauthorized attempt to search an Order</response>
        /// <response code="500">An error occured, please read log files</response>
        [ResponseType(typeof(OrderItemViewModel))]
        [Route("search")]
        public IHttpActionResult PostSearchOrders([FromBody]OrderSearchViewModel orderSearchVmdl)
        {
            try
            {
                var result = orderSearchVmdl.Seach(_bl)
                    .Select(i => new OrderViewModel(i))
                    .ToList();
                var count = result.Count;

                return Ok(new OrderListViewModel(result.Skip(orderSearchVmdl.Offset).Take(orderSearchVmdl.Limit), orderSearchVmdl.Limit, count));
            }
            catch (SecurityException)
            {
                _log.WarnFormat("Security: '{0}' tried to view Orders as Admin/Verwalter'", _bl.GetCurrentUid());
                return Unauthorized();
            }

            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: '{0}'", ex);
                return InternalServerError();
            }
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

        private void SendMail(OrderViewModel vmdl)
        {
            try
            {
                Mail mail = new Mail(vmdl.OrderGuid);
                mail.MessageFormat(vmdl.OrderStatus.Slug);
                mail.Send();
            }
            catch(Exception ex)
            {
                _log.ErrorFormat("Exception: '{0}'", ex);
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
    }
}