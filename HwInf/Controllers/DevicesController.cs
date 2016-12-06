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

        // GET: api/devices/all
        /// <summary>
        /// Returns a List of all Devices
        /// </summary>
        /// <returns></returns>
        [Route("")]
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


        // GET: api/devices/{id}
        /// <summary>
        /// Returns device of given id
        /// </summary>
        /// <param name="id">Device ID</param>
        /// <returns></returns>
        //[Authorize]
        [Route("id/{id}")]
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



        // GET: api/devices/{type}/{filters?}
        /// <summary>
        /// Filters the Devices with given Parameters
        /// </summary>
        /// <param name="type">Device Type</param>
        /// <param name="filters">Filter Values</param>
        /// <returns></returns>

        // [Authorize]
        [Route("{type}/{filters?}")]
        public IEnumerable<DeviceViewModel> GetFilter(string type, string filters = "all")
        {

            var devices = db.Devices.Include(x => x.Type);

            var data = devices
                .Where(i => i.Type.Description.ToLower().Contains(type.ToLower()))
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
                    json = new List<DeviceViewModel>(json.Union(data.Where(i => i.DeviceMetaData.Values.Any(v => v.ToLower().Contains(m.ToLower()))).ToList()));
                    json = new List<DeviceViewModel>(json.Union(data.Where(i => i.Brand.ToLower() == m.ToLower()))); 
                }
            }

            return json;
        }

        // GET: api/devices/types
        /// <summary>
        /// Returns all device types
        /// </summary>
        /// <returns></returns>
        [Route("types")]
        public IEnumerable<string> GetDeviceComponents()
        {
            var deviceTypes = db.DeviceTypes;


            var typesList = deviceTypes
                .Select(i => i.Description)
                .ToList();

            return typesList;
        }

        // GET: api/devices/filter/components/{type}
        /// <summary>
        /// Returns all components of a device type
        /// </summary>
        /// <param name="type">Device Type</param>
        /// <returns></returns>
        [Route("components/{type}")]
        public IEnumerable<object> GetComponents(string type)
        {
            var devices = db.Devices.Include(x => x.Type);
            var meta = db.DeviceMeta.Include(x => x.DeviceType);
            List<object> json = new List<object>();

            var brands = devices
                .Where(i => i.Type.Description.ToLower() == type.ToLower())
                .Select(i => i.Brand)
                .Distinct()
                .ToList();

            brands.Sort();

            IDictionary<string, object> brandList = new Dictionary<string, object>();
            brandList.Add("component", "Marke");
            brandList.Add("values", brands);

            json.Add(brandList);

            var deviceComponents = meta
                    .Where(i => i.DeviceType.Description.ToLower() == type.ToLower())
                    .Select(i => i.MetaKey)
                    .Distinct()
                    .ToList();

            deviceComponents.Sort();

            
            foreach (var c in deviceComponents)
            {
                var componentValues = meta
                    .Where(i => i.DeviceType.Description.ToLower() == type.ToLower())
                    .Where(i => i.MetaKey.ToLower() == c.ToLower())
                    .OrderBy(i => i.MetaValue)
                    .Select(i => i.MetaValue)
                    .Distinct()
                    .ToList();

                componentValues.Sort();
                IDictionary<string, object> componentList = new Dictionary<string, object>();
                componentList.Add("component", c);
                componentList.Add("values", componentValues);
                json.Add(componentList);

            }

            return json;
        }

        // GET: api/devices/types/{type}/{component}
        /// <summary>
        /// Returns all values of a component of a device
        /// </summary>
        /// <param name="type">Device Type</param>
        /// <param name="component">Device Compontent</param>
        /// <returns></returns>
        [Route("components/{type}/{component}")]
        public IDictionary<string, object> GetComponentValues(string type, string component)
        {
            List<string> componentValues = new List<string>();
            if (component.ToLower() == "marke")
            {
                var devices = db.Devices.Include(x => x.Type);
                componentValues = devices
                    .Where(i => i.Type.Description.ToLower() == type.ToLower())
                    .OrderBy(i => i.Brand)
                    .Select(i => i.Brand)
                    .Distinct()
                    .ToList();

                componentValues.Sort();
            } else
            {
                var meta = db.DeviceMeta.Include(x => x.DeviceType);
                componentValues = meta
                    .Where(i => i.DeviceType.Description.ToLower() == type.ToLower())
                    .Where(i => i.MetaKey.ToLower() == component.ToLower())
                    .OrderBy(i => i.MetaValue)
                    .Select(i => i.MetaValue)
                    .Distinct()
                    .ToList();

                componentValues.Sort();
            }

            IDictionary<string, object> json = new Dictionary<string, object>();
          
            json.Add("component", component);
            json.Add("values", componentValues); 

            return json;
        }



        // POST: api/devices/create
        /// <summary>
        /// Creates a new Device in Database
        /// </summary>
        /// <param name="vmdl">Device View Model</param>
        /// <returns></returns>
        //[Authorize]
        [Route("create")]
        [ResponseType(typeof(Device))]
        public IHttpActionResult PostDevice([FromBody]DeviceViewModel vmdl)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(db.Devices.Count(i => i.InvNum == vmdl.InvNum) > 0)
            {
                return BadRequest("Device already exists!");
            }

            Device dev = new Device();
            dev.Description = vmdl.Name;
            dev.InvNum = vmdl.InvNum;
            dev.Status.Description = vmdl.Status;
            dev.Type = db.DeviceTypes.Single(i => i.TypeId == vmdl.TypeId);

            db.Devices.Add(dev);

            foreach (var m in vmdl.DeviceMetaData)
            {
                db.DeviceMeta.Add(new DeviceMeta
                {
                    MetaKey = m.Key,
                    MetaValue = m.Value,
                    Device = dev,
                    DeviceType = dev.Type
                });
            }

            db.SaveChanges();

            return Ok(vmdl);
        }


        // PUT: api/Devicee/5
        /// <summary>
        /// NOT IMPLEMENTED!
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Dev"></param>
        /// <returns></returns>
        //[Authorize]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDevice(int id, Device Dev)
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

        // DELETE: api/devicee/{id}
        /// <summary>
        /// Deletes the device with the given id
        /// </summary>
        /// <param name="id">Device ID</param>
        /// <returns></returns>
        [ResponseType(typeof(Device))]
        public IHttpActionResult DeleteDevice(int id)
        {
            Device Dev = db.Devices.Find(id);
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