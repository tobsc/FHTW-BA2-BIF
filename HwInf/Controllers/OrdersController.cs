using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using HwInf.Common.DAL;
using HwInf.ViewModels;

namespace HwInf.Controllers
{
    [Authorize]
    [RoutePrefix("api/orders")]
    public class OrdersController : ApiController
    {
        private HwInfContext db = new HwInfContext();


        /// <summary>
        /// Returns Order by given id.
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <returns></returns>
        [ResponseType(typeof(OrderViewModel))]
        [Route("id/{id}")]
        public IHttpActionResult GetById(int id)
        {
            var uid = db.Orders.Where(i => i.OrderId == id).Select(i => i.Person.uid).SingleOrDefault();


            if(!IsAllowed(uid) && !IsAdmin())
            {
                return Unauthorized();
            }

                var orders = db.Orders
                    .Include(i => i.Person)
                    .Include(i => i.Owner);

                var vmdl = orders
                    .Where(i => i.OrderId == id)
                    .ToList()
                    .Select(i => new OrderViewModel(i).loadOrderItems(db))
                    .ToList();


                return Ok(vmdl);

        }

        /// <summary>
        /// Returns all orders from a user.
        /// </summary>
        /// <param name="uid">User ID</param>
        /// <returns></returns>
        [ResponseType(typeof(OrderViewModel))]
        [Route("")]
        public IHttpActionResult GetByUid(string uid = "")
        {

            if (!String.IsNullOrWhiteSpace(uid) && !IsAdmin())
            {
                return Unauthorized();
            }

            if(String.IsNullOrWhiteSpace(uid))
            {
                uid = User.Identity.Name;
            }


            var orders = db.Orders
                .Include(i => i.Person)
                .Include(i => i.Owner);

            List<OrderViewModel> vmdl = new List<OrderViewModel>();

            if (!String.IsNullOrWhiteSpace(uid) && IsAdmin())
            {
                vmdl = orders
                    .Where(i => i.Owner.uid == uid)
                    .ToList()
                    .Select(i => new OrderViewModel(i).loadOrderItems(db))
                    .ToList();
            } else
            {
                vmdl = orders
                    .Where(i => i.Person.uid == uid)
                    .ToList()
                    .Select(i => new OrderViewModel(i).loadOrderItems(db))
                    .ToList();
            }

            return Ok(vmdl);

        }

        /// <summary>
        /// Change Order Status
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <param name="act">Action: accept, decline, return</param>
        /// <returns></returns>
        [Route("id/{id}/{act}")]
        public IHttpActionResult GetChangeOrderStatus(int id, string act)
        {
            var uid = db.Orders.Where(i => i.OrderId == id).Select(i => i.Owner.uid).SingleOrDefault();

            if (!IsAllowed(uid) && !IsAdmin())
            {
                return Unauthorized();
            }

            if(!act.Equals("accept") && !act.Equals("decline") && !act.Equals("return"))
            {
                return BadRequest("Wrong action!");
            }
  
            var o = db.Orders.Single(i => i.OrderId == id);

            var order = new OrderViewModel(o);

            order.changeStatus(o, db, act);


            db.SaveChanges();


            return Ok();

        }


        [Route("rooms")]
        public IHttpActionResult GetRooms()
        {
            return Ok(db.Rooms.ToList());
        }


        /// <summary>
        /// Creates a new order.
        /// </summary>
        /// <param name="vmdl">From, To, PersonUid, OwnerUid, OrderItems</param>
        /// <example>test</example>
        /// <returns></returns>
        [Route("create")]
        public IHttpActionResult PostOrder([FromBody] OrderViewModel vmdl)
        {
            if (!IsAllowed(vmdl.PersonUid) && !IsAdmin())
            {
                return Unauthorized();
            }

            if(vmdl.containsDuplicates())
            {
                return BadRequest("Ein Gerät kann nur einmal ausgewählt werden.");
            }

            if(vmdl.containsLentItems(db))
            {
                return BadRequest("Nicht alle Geräte sind zum Ausleihen verfügbar.");
            }

            Order o = new Order();
            vmdl.Status = db.Status.Where(i => i.Description == "Offen").Select(i => i.Description).FirstOrDefault();
            vmdl.Date = DateTime.Now;
            vmdl.ApplyChanges(o, db); 
            db.Orders.Add(o);
            db.SaveChanges();

            var orderItems = vmdl.createOrderItems(o, db);
            orderItems.ForEach(i => db.OrderItems.Add(i));
            db.SaveChanges();            

            return Ok();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderExists(int id)
        {
            return db.Orders.Count(e => e.OrderId == id) > 0;
        }

        private bool IsAllowed(string uid)
        {
            return User.Identity.Name == uid ? true : false;
        }

        private bool IsAdmin()
        {
            return RequestContext.Principal.IsInRole("Admin") ? true : false;
        }
    }
}