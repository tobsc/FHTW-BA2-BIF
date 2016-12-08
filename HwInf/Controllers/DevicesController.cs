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
        /// Returns a list of all devices
        /// </summary>
        /// <returns></returns>
        [Route("")]
        public IHttpActionResult GetAll()
        {
            try
            {
                var devices = db.Devices.Include(x => x.Type);

                var json = devices
                    .Where(i => i.DeviceId > 0)
                    .Take(10000)
                    .ToList() // execl SQL
                    .Select(i => new DeviceViewModel(i).loadMeta(db)) // Convert to viewmodel
                    .ToList();

                return Ok(json);

            } catch
            {
                return InternalServerError() ;
            }
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
            try
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
            } catch
            {
                return InternalServerError();
            }
            
        }



        // GET: api/devices/{type}/{filters?}
        /// <summary>
        /// Filters the devices with given parameters
        /// </summary>
        /// <param name="type">Device Type</param>
        /// <returns></returns>

        // [Authorize]
        [Route("{type}")]
        public IHttpActionResult GetFilter(string type)
        {
            try
            {
                var parameterQuery = Request.GetQueryNameValuePairs();
                var devices = db.Devices.Include(x => x.Type);


                var data = devices
                    .Where(i => i.Type.Description.ToLower().Contains(type.ToLower()))
                    .Take(10000)
                    .ToList() // execl SQL
                    .Select(i => new DeviceViewModel(i).loadMeta(db)) // Convert to viewmodel
                    .ToList();

                var json = new List<DeviceViewModel>();
                json = data.ToList();

                if (parameterQuery.Count() != 0)
                {
                    json.Clear();

                    var searchData = data.ToList();

                    foreach (var p in parameterQuery)
                    {
                        json.Clear();
                        var parameters = parameterQuery.Where(i => i.Key == p.Key).Select(i => i.Value).ToList();

                        foreach (var m in parameters)
                        {
                            json = new List<DeviceViewModel>(json.Union(searchData.Where(i => i.DeviceMetaData.Values.Any(v => v.ToLower().Contains(m.ToLower()))).ToList()));
                            json = new List<DeviceViewModel>(json.Union(searchData.Where(i => i.Brand.ToLower() == m.ToLower())));
                        }

                        searchData = json.ToList();
                    }

                }
                return Ok(json);
            } catch
            {
                return InternalServerError();
            }
           
        }

        // GET: api/devices/types
        /// <summary>
        /// Returns all device types
        /// </summary>
        /// <returns></returns>
        [Route("types")]
        public IHttpActionResult GetDeviceComponents()
        {
            try
            {
                var deviceTypes = db.DeviceTypes;


                var typesList = deviceTypes
                    .Select(i => i.Description)
                    .ToList();

                return Ok(typesList);
            } catch
            {
                return InternalServerError();
            }
           
        }

        // GET: api/devices/filter/components/{type}
        /// <summary>
        /// Returns all components of a device with their values
        /// </summary>
        /// <param name="type">Device Type</param>
        /// <returns></returns>
        [Route("components/{type}")]
        public IHttpActionResult GetComponents(string type)
        {
            try
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
            brandList.Add("component", "Brand");
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

                return Ok(json);
            } catch
            {
                return InternalServerError();
            }
           
        }


        // POST: api/devices/create
        /// <summary>
        /// Creates a new device
        /// </summary>
        /// <param name="vmdl">Device View Model</param>
        /// <returns></returns>
        //[Authorize]
        [Route("create")]
        [ResponseType(typeof(Device))]
        public IHttpActionResult PostDevice([FromBody]DeviceViewModel vmdl)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (db.Devices.Count(i => i.InvNum == vmdl.InvNum) > 0)
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
            } catch
            {
                return InternalServerError();
            }
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