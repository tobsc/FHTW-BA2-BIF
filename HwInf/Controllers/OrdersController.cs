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
        /// <param name="act">NOT IMPLEMENTED.</param>
        /// <returns></returns>
        [ResponseType(typeof(OrderViewModel))]
        [Route("id/{id}/{act?}")]
        public IHttpActionResult GetById(int id, string act = null)
        {
            var uid = db.Orders.Where(i => i.OrderId == id).Select(i => i.Person.uid).SingleOrDefault();


            if(!IsCurrentUser(uid) && !IsAdmin())
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
        [Route("uid/{uid}")]
        public IHttpActionResult GetByUid(string uid)
        {

            if (!IsCurrentUser(uid) && !IsAdmin())
            {
                return Unauthorized();
            }

            var orders = db.Orders
                .Include(i => i.Person)
                .Include(i => i.Owner);

            var vmdl = orders
                .Where(i => i.Person.uid == uid)
                .ToList()
                .Select(i => new OrderViewModel(i).loadOrderItems(db))
                .ToList();


            return Ok(vmdl);

        }

        /// <summary>
        /// Creates a new order.
        /// </summary>
        /// <param name="vmdl">OrderViewModel</param>
        /// <returns></returns>
                [RouteAttribute("create")]
        public IHttpActionResult PostOrder([FromBody] OrderViewModel vmdl)
        {
            if (!IsCurrentUser(vmdl.PersonUid) && !IsAdmin())
            {
                return Unauthorized();
            }

            Order o = new Order();
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

        private bool IsCurrentUser(string uid)
        {
            return User.Identity.Name == uid ? true : false;
        }

        private bool IsAdmin()
        {
            return RequestContext.Principal.IsInRole("Admin") ? true : false;
        }
    }
}