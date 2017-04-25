using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Security;
using HwInf.Common.DAL;
using HwInf.Common.BL;
using HwInf.Common.Models;
using HwInf.ViewModels;
using log4net;
using WebGrease.Css.Extensions;

namespace HwInf.Controllers
{
    [Authorize]
    [RoutePrefix("api/devices")]
    public class DevicesController : ApiController
    {
        private readonly IDAL _db;
        private readonly BL _bl;
        private readonly ILog _log = LogManager.GetLogger(typeof(DevicesController).Name);

        public DevicesController()
        {
            _db = new HwInfContext();
            _bl = new BL(_db);
        }

        public DevicesController(IDAL db)
        {
            _db = db;
            _bl = new BL(db);
        }

        // GET: api/devices/
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
            try
            {
                var devices = _bl.GetDevices()
                    .ToList()
                    .Select(i => new DeviceViewModel(i).LoadMeta(i))
                    .ToList();

                var deviceList = new DeviceListViewModel(devices.Skip(offset).Take(limit), offset, limit, _bl,
                    devices.Count);

                return Ok(deviceList);

            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: {0}", ex.Message);
                return InternalServerError();
            }
        }


        // GET: api/devices/id/{id}
        /// <summary>
        /// Returns device by
        /// </summary>
        /// <param name="id">Device ID</param>
        /// <returns></returns>
        [ResponseType(typeof(DeviceViewModel))]
        [Route("id/{id}")]
        public IHttpActionResult GetDevice(int id)
        {
            try
            {
                var d = _bl.GetSingleDevice(id);
                var vmdl = new DeviceViewModel(d).LoadMeta(d);


                if (vmdl == null)
                {
                    return NotFound();
                }

                return Ok(vmdl);
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: {0}", ex.Message);
                return InternalServerError();
            }

        } 
        
        // GET: api/devices/invnum/{invNum}
        /// <summary>
        /// Returns device by InvNum
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
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: {0}", ex.Message);
                return InternalServerError();
            }

        }


      
        // POST: api/devices/filter/
        /// <summary>
        /// Filters the devices with given parameters
        /// </summary>
        /// <param name="vmdl">FilterViewModel</param>
        /// <returns></returns>
        [ResponseType(typeof(List<Device>))]
        [Route("filter")]
        public IHttpActionResult PostFilter([FromBody] FilterViewModel vmdl)
        {
            try
            {
                vmdl.OrderBy = vmdl.OrderBy ?? "Name";
                vmdl.Order = vmdl.Order ?? "ASC";

                var b = vmdl.FilteredList(_bl).ToList().Select(i => new DeviceViewModel(i).LoadMeta(i)).ToList();
                var count = b.Count;
                b = vmdl.Limit < 0
                    ? b.ToList()
                    : b.Skip(vmdl.Offset).Take(vmdl.Limit).ToList();

                return Ok(new DeviceListViewModel(b, vmdl.Offset, vmdl.Limit, _bl, count));
            }
            catch (SecurityException)
            {
                _log.ErrorFormat("'{0}' tried to list Devices as Admin/Verwalter", _bl.GetCurrentUid());
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: {0}", ex.Message);
                return InternalServerError();
            }
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

        // POST: api/devices/create
        /// <summary>
        /// Creates a new device
        /// </summary>
        /// <param name="vmdl">Name, Marke, InvNum, TypeId, StatusId, RoomId, OwnerUid, DeviceMetaData</param>
        /// <returns></returns>
        [Authorize (Roles = "Admin, Verwalter")]
        [Route("")]
        [ResponseType(typeof(Device))]
        public IHttpActionResult PostDevice([FromBody]DeviceViewModel vmdl)
        {
            try
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
                if (vmdl.AdditionalInvNums != null)
                    invNums.AddRange(vmdl.AdditionalInvNums);

                // Get Existing InvNums
                var existingInvNums = _bl.GetDevices(false).Select(i => i.InvNum)
                    .ToList();


                var invNumsNoDupl = invNums.Select(i => i.InvNum).Distinct().ToList();

                // Check if new InvNums do not exist
                if (invNums.Select(i => i.InvNum).Intersect(existingInvNums).Any() ||
                    invNumsNoDupl.Count() != invNums.Count())
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
                        var fvmdl = new FieldViewModel {Name = i.Field};
                        fvmdl.ApplyChanges(field, _bl);
                        fg.Fields.Add(field);
                    }
                });

                var response = new List<DeviceViewModel>();

                invNums
                    .Select(i => i.InvNum)
                    .ForEach(i =>
                    {
                        var d = _bl.CreateDevice();
                        d.CreateDate = DateTime.Now;
                        vmdl.InvNum = i;
                        vmdl.ApplyChanges(d, _bl);
                        vmdl.Refresh(d);
                        response.Add(new DeviceViewModel(d).LoadMeta(d));

                    });

                _bl.SaveChanges();

                _log.InfoFormat("Device '{0}({1})' added by '{2}'", vmdl.InvNum, vmdl.Name, User.Identity.Name);
                if (vmdl.AdditionalInvNums == null) return Ok(vmdl);
                foreach (var n in vmdl.AdditionalInvNums)
                {
                    _log.InfoFormat("Device '{0}({1})' added by '{2}'", n.InvNum, vmdl.Name, User.Identity.Name);
                }

                return Ok(response);

            }
            catch (SecurityException)
            {
                _log.ErrorFormat("Security: '{0}' tried to create Device '{1}'", _bl.GetCurrentUid(), vmdl.InvNum);
                return Unauthorized();
            }

            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: {0}", ex.Message);
                return InternalServerError();
            }
        }

        // POST: api/devices/types/
        /// <summary>
        /// Create New DeviceType
        /// </summary>
        /// <param name="vmdl">DeviceTypeViewModel</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Verwalter")]
        [ResponseType(typeof(DeviceViewModel))]
        [Route("types")]
        public IHttpActionResult PostDeviceType(DeviceTypeViewModel vmdl)
        {
            try
            {
                var dt = _bl.CreateDeviceType();

                vmdl.ApplyChanges(dt, _bl);
                _bl.SaveChanges();

                _log.InfoFormat("DeviceType '{0}' created by '{1}'", vmdl.Name, User.Identity.Name);

                vmdl.Refresh(dt);
                return Ok(vmdl);

            }
            catch (SecurityException)
            {
                _log.ErrorFormat("Security: '{0}' tried to create DeviceType '{1}'", _bl.GetCurrentUid(), vmdl.Name);
                return Unauthorized();
            }

            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: '{0}'", ex.Message);
                return InternalServerError();
            }
        }

        // DELETE: api/devices/id/{id}
        /// <summary>
        /// Deletes (Sets IsActive to 0) a device
        /// </summary>
        /// <param name="id">Device ID</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Verwalter")]
        [Route("id/{id}")]
        public IHttpActionResult DeleteDevice(int id)
        {
            try
            {
                if (!_bl.DeviceExists(id))
                {
                    return NotFound();
                }

                var d = _bl.GetSingleDevice(id);
                _bl.DeleteDevice(d);
                _bl.SaveChanges();
                _log.InfoFormat("Device '{0}({1})' deleted by '{2}'", d.InvNum, d.Name, User.Identity.Name);

                return Ok();

            }
            catch (SecurityException)
            {
                _log.ErrorFormat("Security: '{0}' tried to delete Device '{1}'", _bl.GetCurrentUid(), id);
                return Unauthorized();
            }

            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: {0}", ex.Message);
                return InternalServerError();
            }
        }

        // DELETE: api/devices/types/{slug}
        /// <summary>
        /// Delete Device Type
        /// </summary>
        /// <param name="slug">DeviceType Slug</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Verwalter")]
        [Route("types/{slug}")]
        public IHttpActionResult DeleteDeviceType(string slug)
        {

            try
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
                    _log.InfoFormat("DeviceType '{0}' deleted by '{1}'", dt.Name, _bl.GetCurrentUid());
                }

                return Ok();

            }
            catch (SecurityException)
            {
                _log.ErrorFormat("Security: '{0}' tried to delete DeviceType '{1}'", _bl.GetCurrentUid(), slug);
                return Unauthorized();
            }

            catch(Exception ex)
            {
                _log.ErrorFormat("Exception: '{0}'", ex.Message);
                return InternalServerError();
            }
        }

        // PUT: api/devices/id/{id}
        /// <summary>
        /// Update a Devices
        /// </summary>
        /// <param name="id">Device Id</param>
        /// <param name="vmdl">DeviceViewModel</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Verwalter")]
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
                        var fvmdl = new FieldViewModel {Name = i.Field};
                        fvmdl.ApplyChanges(field, _bl);
                        fg.Fields.Add(field);
                    }
                });



                var dev = _bl.GetSingleDevice(vmdl.InvNum);
                _bl.UpdateDevice(dev);
                var dm = dev.DeviceMeta.ToList();
                dm.ForEach(i => _bl.DeleteMeta(i));
                dev.DeviceMeta.Clear();
                vmdl.ApplyChanges(dev, _bl);
                _bl.SaveChanges();

                _log.InfoFormat("Device '{0}({1})' updated by '{2}'", vmdl.InvNum, vmdl.Name, User.Identity.Name);
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
            catch (SecurityException)
            {
                _log.ErrorFormat("Security: '{0}' tried to update Device '{1}'", vmdl.InvNum);
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: {0}", ex.Message);
                return InternalServerError();
            }



            return Ok(vmdl);
        }

        // PUT: api/devices/types/{slug}
        /// <summary>
        /// Edit DeviceType
        /// </summary>
        /// <param name="slug">DeviceType slug</param>
        /// <param name="vmdl">DeviceTypeViewModel</param>
        /// <returns></returns>
        [Authorize(Roles="Admin, Verwalter")]
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

                _log.InfoFormat("DeviceType '{0}' updated by '{1}'", vmdl.Name, User.Identity.Name);

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
            catch (SecurityException)
            {
                _log.ErrorFormat("Security: '{0}' tried to update DeviceType '{1}'", _bl.GetCurrentUid(), slug);
                return Unauthorized();
            }

            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: {0}", ex.Message);
                return InternalServerError();
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