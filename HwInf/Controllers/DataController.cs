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

namespace HwInf.Controllers
{
    public class DataController : ApiController
    {
        private HwInfContext db = new HwInfContext();

      
        public IHttpActionResult GetStatus()
        {
            var status = db.Status.ToList();

            return Ok(status);
        }

        public IHttpActionResult GetRooms()
        {
            var rooms = db.Rooms.ToList();

            return Ok(rooms);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}