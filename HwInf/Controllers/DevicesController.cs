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
using HwInf.Models;
using HwInf.Common.DAL;

namespace HwInf.Controllers
{
    [RoutePrefix("api/devices")]
    public class DevicesController : ApiController
    {
        private HwInfContext db = new HwInfContext();

        // GET: api/Devices
        [Route("all")]
        public IEnumerable<DeviceViewModel> GetAll()
        {

            var devices = db.Devices.Include(x => x.Type);

            var json = devices
                .Where(i => i.DeviceId > 0)
                .Take(10000)
                .ToList() // execl SQL
                .Select(i => new DeviceViewModel(i).loadMeta(db)) // Convert to viewmodel
                .ToList();

            return json;
        }
       // [Authorize]
        [Route("filter/{type}/{filters?}")]
        public IEnumerable<DeviceViewModel> GetFilter(string type, string filters = "all")
        {

            var devices = db.Devices.Include(x => x.Type);

            var data = devices
                .Where(i => i.Type.Name.Contains(type))
                .Take(10000)
                .ToList() // execl SQL
                .Select(i => new DeviceViewModel(i).loadMeta(db)) // Convert to viewmodel
                .ToList();

            var json = new List<DeviceViewModel>();
            json = data.ToList();
            if (filters != "all")
            {
                var parameters = filters.Split('|');
                json.Clear();
                foreach (var m in parameters)
                {
                    json = new List<DeviceViewModel>(json.Union(data.Where(i => i.DeviceMetaData.Values.Any(v => v.ToLower().Contains(m))).ToList()));
                }
            }

            return json;
        }

        // GET: api/Devicee/5
        //[Authorize]
        [ResponseType(typeof(DBDevice))]
        public IHttpActionResult GetDevice(int id)
        {
            var devices = db.Devices.Include(x => x.Type);
            var json = devices
             .Where(i => i.DeviceId == id)
             .ToList() // execl SQL
             .Select(i => new DeviceViewModel(i).loadMeta(db)) // Convert to viewmodel
             .ToList();

            if (json == null)
            {
                return NotFound();
            }

            return Ok(json);
        }

        // PUT: api/Devicee/5
        [Authorize]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDevice(int id, DBDevice Dev)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Dev.DeviceId)
            {
                return BadRequest();
            }

            db.Entry(Dev).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeviceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST
        //[Authorize]
        [Route("create")]
        [ResponseType(typeof(DBDevice))]
        public IHttpActionResult PostDevice([FromBody]DeviceViewModel vmdl)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            vmdl.createDevice(db);

            return Ok(vmdl);
        }

        // DELETE: api/Devicee/5
        [ResponseType(typeof(DBDevice))]
        public IHttpActionResult DeleteDevice(int id)
        {
            DBDevice Dev = db.Devices.Find(id);
            if (Dev == null)
            {
                return NotFound();
            }

            db.Devices.Remove(Dev);
            db.SaveChanges();

            return Ok(Dev);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DeviceExists(int id)
        {
            return db.Devices.Count(e => e.DeviceId == id) > 0;
        }
    }
}