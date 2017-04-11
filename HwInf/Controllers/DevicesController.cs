using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using HwInf.Common.DAL;
using HwInf.Common.BL;
using HwInf.Common.Models;
using HwInf.ViewModels;
using log4net;
using WebGrease.Css.Extensions;

namespace HwInf.Controllers
{
    [RoutePrefix("api/devices")]
    public class DevicesController : ApiController
    {
        private readonly HwInfContext _db = new HwInfContext();
        private readonly BL _bl;
        private readonly ILog _log = LogManager.GetLogger("Devices");

        public DevicesController()
        {
            _bl = new BL(_db);
        }

        // GET: api/devices/all
        /// <summary>
        /// Returns a list of all devices
        /// </summary>
        /// <param name="limit">Limit</param>
        /// <param name="offset">Offset</param>
        /// <returns></returns>
        [ResponseType(typeof(DeviceViewModel))]
        [Route("")]
        public IHttpActionResult GetAll(int limit = 25, int offset = 0)
        {

            var devices = _bl.GetDevices()
                .ToList()
                .Select(i => new DeviceViewModel(i).LoadMeta(i))
                .ToList();

            var deviceList = new DeviceListViewModel(devices.Skip(offset).Take(limit), offset, limit, _bl, devices.Count);

            return Ok(deviceList);
        }


        // GET: api/devices/all
        /// <summary>
        /// Returns a list of all devices
        /// </summary>
        /// <param name="limit">Limit</param>
        /// <param name="offset">Offset</param>
        /// <returns></returns>
        [ResponseType(typeof(DeviceViewModel))]
        [Authorize(Roles = "Admin, Verwalter")]
        [Route("admin")]
        public IHttpActionResult GetAllAdmin(int limit = 25, int offset = 0)
        {
            var vmdl = _bl.GetDevices(false)
                .ToList()
                .Select(i => new DeviceViewModel(i).LoadMeta(i))
                .ToList();

            if(!_bl.IsAdmin()) vmdl = vmdl.TakeWhile(i => i.Verwalter.Uid == User.Identity.Name).ToList();

            return Ok(new DeviceListViewModel(vmdl.Skip(offset).Take(limit), offset, limit , _bl, vmdl.Count ));
        }


        // GET: api/devices/{id}
        /// <summary>
        /// Returns device of given id
        /// </summary>
        /// <param name="id">Device ID</param>
        /// <returns></returns>
        [ResponseType(typeof(DeviceViewModel))]
        [Route("id/{id}")]
        public IHttpActionResult GetDevice(int id)
        {
            try
            {
                Device d = _bl.GetSingleDevice(id);
                var vmdl = new DeviceViewModel(d).LoadMeta(d);


                if (vmdl == null)
                {
                    return NotFound();
                }

                return Ok(vmdl);
            } catch
            {
                return InternalServerError();
            }
            
        } 
        
        // GET: api/devices/invnum/{invNum}
        /// <summary>
        /// Returns device of given InvNum
        /// </summary>
        /// <param name="invNum">Device InvNum</param>
        /// <returns></returns>
        [ResponseType(typeof(DeviceViewModel))]
        [Route("invnum/{invNum}")]
        public IHttpActionResult GetDevice(string invNum)
        {
            try
            {
                var d = _bl.GetSingleDevice(invNum);
                var vmdl = new DeviceViewModel(d).LoadMeta(d);


                if (vmdl == null)
                {
                    return NotFound();
                }

                return Ok(vmdl);
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
        /// <param name="limit">Limit</param>
        /// <param name="offset">Offset</param>
        /// 
        /// <returns></returns>

        [ResponseType(typeof(List<DeviceViewModel>))]
        [Route("{type}")]
        public IHttpActionResult GetFilter(string type, int limit = 25, int offset = 0)
        {
            var pq = Request.GetQueryNameValuePairs();
            var parameterQuery = pq.ToDictionary(p => p.Key, p => p.Value);
            parameterQuery.Remove("limit");
            parameterQuery.Remove("offset");

            var dt = _bl.GetDeviceType(type);

            var data = _bl.GetDevices(true, dt.Slug)
                .ToList() // execl SQL
                .Select(i => new DeviceViewModel(i).LoadMeta(i)) // Convert to viewmodel
                .ToList();

            var response = data.ToList();

            if (parameterQuery.Count() != 0)
            {
                response.Clear();

                var searchData = data.ToList();

                foreach (var p in parameterQuery)
                {
                    response.Clear();
                    var parameters = parameterQuery.Where(i => i.Key == p.Key).Select(i => i.Value).ToList();

                    foreach (var m in parameters)
                    {
                        response = new List<DeviceViewModel>(response.Union(searchData.Where(i => i.DeviceMeta.Any(k => k.Value.ToLower().Equals(m.ToLower()))))); 
                        response = new List<DeviceViewModel>(response.Union(searchData.Where(i => i.Marke.ToLower() == m.ToLower())));
                        response = new List<DeviceViewModel>(response.Union(searchData.Where(i => i.Name.ToLower().Contains(m.ToLower()))));
                    }

                    searchData = response.ToList();
                }

            }

            return Ok(new DeviceListViewModel(response.Skip(offset).Take(limit), limit, offset, _bl,response.Count));

        }

        // POST: api/devices/{type}/{filters?}
        /// <summary>
        /// Filters the devices with given parameters
        /// </summary>
        /// 
        /// <returns></returns>

        [ResponseType(typeof(List<Device>))]
        [Route("filter")]
        public IHttpActionResult PostFilter([FromBody] FilterViewModel vmdl)
        {

            var b = vmdl.FilteredList(_bl).ToList().Select(i => new DeviceViewModel(i).LoadMeta(i)).ToList();
            var count = b.Count;
            b = vmdl.Limit < 0 
                ? b.ToList() 
                : b.Skip(vmdl.Offset).Take(vmdl.Limit).ToList();

            return Ok(new DeviceListViewModel(b, vmdl.Offset, vmdl.Limit, _bl, count));

        }

        // GET: api/devices/types
        /// <summary>
        /// Returns all device types
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(List<string>))]
        [Route("types")]
        public IHttpActionResult GetDeviceTypes()
        {
            try
            {
                var deviceTypes = _bl.GetDeviceTypes()
                    .ToList()
                    .Select(i => new DeviceTypeViewModel(i))  // LoadComponents?
                    .ToList();


                return Ok(deviceTypes);
            }
            catch
            {
                return InternalServerError();
            }

        }

        // GET: api/devices/types/{type}
        /// <summary>
        /// Returns all components of a device type with all values
        /// </summary>
        /// <param name="type">Device Type Name</param>
        /// <returns></returns>
        [ResponseType(typeof(List<DeviceViewModel>))]
        [Route("types/{type}")]
        public IHttpActionResult GetComponents(string type)
        {
            var devices = _bl.GetDevices();
            var dt = _bl.GetDeviceType(type);
            var fg = _bl.GetFieldGroups();

            var components = fg.ToList()
                .Where(i => i.DeviceTypes.Contains(dt))
                .Select(i => new FieldGroupViewModel(i)).ToList();

            var response = new List<object>();

            var brands = devices
                .Where(i => i.Type.Name.ToLower() == type.ToLower())
                .Select(i => i.Brand)
                .Distinct()
                .ToList();

            brands.Sort();

            IDictionary<string, object> brandList = new Dictionary<string, object>();
            brandList.Add("component", "Marke");
            brandList.Add("values", brands);

            var f = new List<FieldViewModel>();
            f = brands
                .Select(i => new FieldViewModel{ Name = i, Slug = SlugGenerator.GenerateSlug(i, "field") })
                .ToList();

            var x = new FieldGroupViewModel {Name = "Marke", Slug = "brand", Fields = f};

            //response.Add(x);
            response.Add(components);


            return Ok(response);
        }

        /// <summary>
        /// Returns component values filtered by device type, component and user input
        /// </summary>
        /// <param name="type">Device Type</param>
        /// <param name="component">Device Component (e.g. Marke, Name, Prozessor, etc)</param>
        /// <param name="input">Input string</param>
        /// <returns></returns>
        [ResponseType(typeof(IQueryable<string>))]
        [Route("autofill/{type}/{component}/{input}")]
        public IHttpActionResult GetComponentValues(string type, string component, string input)
        {
            try
            {
                return Ok(new AutoFillViewModel().RefreshList(input, type, component, _bl));
            }
            catch
            {
                return InternalServerError();
            }

        }

        // POST: api/devices/create
        /// <summary>
        /// Creates a new device
        /// </summary>
        /// <param name="vmdl">Name, Marke, InvNum, TypeId, StatusId, RoomId, OwnerUid, DeviceMetaData</param>
        /// <returns></returns>
        //[Authorize]
        [Route("")]
        [ResponseType(typeof(Device))]
        public IHttpActionResult PostDevice([FromBody]DeviceViewModel vmdl)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            if (string.IsNullOrWhiteSpace(vmdl.Name))
            {
                return BadRequest("Bitte einen Namen für das Gerät angeben.");
            }

            if (string.IsNullOrWhiteSpace(vmdl.Marke))
            {
                return BadRequest("Bitte eine Marke für das Gerät angeben.");
            }


            if (_bl.GetDeviceType(vmdl.DeviceType.Slug) == null)
            {
                return BadRequest("Typ nicht vorhanden.");
            }

            if (_bl.GetUsers(vmdl.Verwalter.Uid) == null)
            {
                return BadRequest("Person nicht vorhanden.");
            }



            // Put all invNums into one List
            var invNums = new List<AdditionalInvNumViewModel>
            {
                new AdditionalInvNumViewModel {InvNum = vmdl.InvNum}
            };
            invNums.AddRange(vmdl.AdditionalInvNums);

            // Get Existing InvNums
            var existingInvNums = _bl.GetDevices(false).Select(i => i.InvNum)
                .ToList();


            var invNumsNoDupl = invNums.Select(i => i.InvNum).Distinct().ToList();

            // Check if new InvNums do not exist
            if (invNums.Select(i => i.InvNum).Intersect(existingInvNums).Any() || invNumsNoDupl.Count() != invNums.Count())
            {
                return BadRequest("Es existiert bereits ein Gerät mit dieser Inventarnummer.");
            }



            // Check for new fields and add them
            vmdl.DeviceMeta.ForEach(i =>
            {
                var fg = _bl.GetFieldGroups(i.FieldGroupSlug);
                if (fg.Fields.Count(j => j.Name == i.Field) == 0)
                {
                    _bl.UpdateFieldGroup(fg);
                    var field = _bl.CreateField();
                    var fvmdl = new FieldViewModel { Name = i.Field };
                    fvmdl.ApplyChanges(field, _bl);
                    fg.Fields.Add(field);
                }
            });



            invNums
                .Select(i => i.InvNum)
                .ForEach(i =>
                {
                    var d = _bl.CreateDevice();
                    d.CreateDate = DateTime.Now;
                    vmdl.InvNum = i;
                    vmdl.ApplyChanges(d, _bl);
                    vmdl.Refresh(d);
                });

            _bl.SaveChanges();

            _log.InfoFormat("Device '{0}({1})' added by '{2}'", vmdl.InvNum, vmdl.Name, User.Identity.Name);
            foreach (var n in vmdl.AdditionalInvNums)
            {
                _log.InfoFormat("Device '{0}({1})' added by '{2}'", n.InvNum, vmdl.Name, User.Identity.Name);
            }

            return Ok(vmdl);
        }

        // POST: api/admin/devices/types
        /// <summary>
        /// Create New DeviceType
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(DeviceViewModel))]
        [Route("types")]
        public IHttpActionResult PostDeviceType(DeviceTypeViewModel vmdl)
        {

            var dt = _bl.CreateDeviceType();

            vmdl.ApplyChanges(dt, _bl);
            _bl.SaveChanges();

            vmdl.Refresh(dt);
            return Ok(vmdl);
        }

        // DELETE: api/devicee/{id}
        /// <summary>
        /// Deletes (Sets IsActive to 0) the device with the given id
        /// </summary>
        /// <param name="id">Device ID</param>
        /// <returns></returns>
        //[Authorize(Roles = "Admin")]
        [Route("id/{id}")]
        public IHttpActionResult DeleteDevice(int id)
        {
            if (!_bl.DeviceExists(id))
            {
                return NotFound();
            }
            else
            {
                _bl.DeleteDevice(id);
                _bl.SaveChanges();
            }

            return Ok();
        }

        // DELETE: 
        /// <summary>
        /// Delete Device Type
        /// </summary>
        /// <param name="slug">Device ID</param>
        /// <returns></returns>
        //[Authorize(Roles = "Admin")]
        [Route("types/{slug}")]
        public IHttpActionResult DeleteDeviceType(string slug)
        {
            if (_bl.GetDeviceType(slug) == null)
            {
                return NotFound();
            }
            else
            {
                var dt = _bl.GetDeviceType(slug);
                _bl.DeleteDeviceType(dt);
                _bl.SaveChanges();
            }

            return Ok();
        }

        // PUT: api/Devicee/5
        /// <summary>
        /// Update a Devices
        /// </summary>
        /// <param name="id"></param>
        /// <param name="vmdl"></param>
        /// <returns></returns>
        //[Authorize]
        [HttpPut]
        [Route("id/{id}")]
        public IHttpActionResult PutDevice(int id, DeviceViewModel vmdl)
        {


            if (id != vmdl.DeviceId)
            {
                return BadRequest();
            }

            try
            {

                // Check for new fields and add them
                vmdl.DeviceMeta.ForEach(i =>
                {
                    var fg = _bl.GetFieldGroups(i.FieldGroupSlug);
                    if (fg.Fields.Count(j => j.Name == i.Field) == 0)
                    {
                        _bl.UpdateFieldGroup(fg);
                        var field = _bl.CreateField();
                        var fvmdl = new FieldViewModel { Name = i.Field };
                        fvmdl.ApplyChanges(field, _bl);
                        fg.Fields.Add(field);
                    }
                });



                var dev = _bl.GetSingleDevice(vmdl.DeviceId);
                _bl.UpdateDevice(dev);
                var dm = dev.DeviceMeta.ToList();
                dm.ForEach(i => _bl.DeleteMeta(i));
                dev.DeviceMeta.Clear();
                vmdl.ApplyChanges(dev, _bl);
                _bl.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_bl.DeviceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(vmdl);
        }

        // PUT: api/devices/types
        /// <summary>
        /// Edit DeviceType
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(DeviceViewModel))]
        [Route("types/{slug}")]
        public IHttpActionResult PutDeviceType(string slug, DeviceTypeViewModel vmdl)
        {
            try
            {
                var dt = _bl.GetDeviceType(slug);
                _bl.UpdateDeviceType(dt);

                vmdl.ApplyChanges(dt, _bl);
                _bl.SaveChanges();

            }
            catch (DbUpdateConcurrencyException)
            {
                if (_bl.GetDeviceType(slug) == null)
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