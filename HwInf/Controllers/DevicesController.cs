﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
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

namespace HwInf.Controllers
{
    [Authorize]
    [RoutePrefix("api/devices")]
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public class DevicesController : ApiController
    {
        private readonly HwInfContext _db = new HwInfContext();
        private readonly BL _bl;

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
            var vmdl = _bl.GetDevices(limit, offset)
                .ToList()
                .Select(i => new DeviceViewModel(i).LoadMeta(i))
                .ToList();

            return Ok(vmdl);
        }

        // GET: api/devices/all
        /// <summary>
        /// Returns a list of all devices
        /// </summary>
        /// <param name="limit">Limit</param>
        /// <param name="offset">Offset</param>
        /// <returns></returns>
        [ResponseType(typeof(DeviceViewModel))]
        [Authorize(Roles = "Admin, Owner")]
        [Route("admin")]
        public IHttpActionResult GetAllAdmin(int limit = 25, int offset = 0)
        {
            var vmdl = _bl.GetDevices(limit, offset, false, 0, true)
                .ToList()
                .Select(i => new DeviceViewModel(i).LoadMeta(i))
                .ToList();

            if(!_bl.IsAdmin()) vmdl = vmdl.TakeWhile(i => i.Verwalter.Uid == User.Identity.Name).ToList();

            return Ok(vmdl.Skip(offset).Take(limit));
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

            var data = _bl.GetDevices(limit, offset, true, dt.TypeId, true)
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
            return Ok(response.Skip(offset).Take(limit));

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

            if (String.IsNullOrWhiteSpace(vmdl.Name))
            {
                return BadRequest("Bitte einen Namen für das Gerät angeben.");
            }

            if (String.IsNullOrWhiteSpace(vmdl.Marke))
            {
                return BadRequest("Bitte eine Marke für das Gerät angeben.");
            }


            if (_bl.GetDeviceType(vmdl.DeviceType.Slug) == null)
            {
                return BadRequest("Typ nicht vorhanden.");
            }

            if (_bl.GetDevices(0, 0, true, vmdl.DeviceType.DeviceTypeId, true)
                .Count(i => i.InvNum.ToLower().Equals(vmdl.InvNum.ToLower())) > 0)
            {
                return BadRequest("Es existiert bereits ein Gerät mit dieser Inventarnummer.");
            }


       

            if (_bl.GetUsers(vmdl.Verwalter.Uid) == null)
            {
                return BadRequest("Person nicht vorhanden.");
            }

            var dev = _bl.CreateDevice();
            vmdl.ApplyChanges(dev, _bl);
            _bl.SaveChanges();

            vmdl.Refresh(dev);

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
        [Route("delete/{id}")]
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

        // PUT: api/Devicee/5
        /// <summary>
        /// Update a Devices
        /// </summary>
        /// <param name="id"></param>
        /// <param name="vmdl"></param>
        /// <returns></returns>
        //[Authorize]
        [HttpPut]
        [Route("")]
        public IHttpActionResult PutDevice(int id, DeviceViewModel vmdl)
        {


            if (id != vmdl.DeviceId)
            {
                return BadRequest();
            }

            try
            {
                var dev = _bl.GetSingleDevice(vmdl.DeviceId);
                _bl.UpdateDevice(dev);
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