using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
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

        /// <summary>
        /// Returns List of DeviceStatus
        /// </summary>
        /// <remarks>Returns a list of &#x60;DeviceStatus&#x60;</remarks>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
        [ResponseType(typeof(List<DeviceStatusViewModel>))]
        [Route("status")]

        public IHttpActionResult GetDeviceStatuses()
        {


            var vmdls = _bl.GetDeviceStatuses()
                .ToList()
                .Select(i => new DeviceStatusViewModel(i));

            return Ok(vmdls);
        }

        // GET: api/devices/
        /// <summary>
        /// Returns a List of all Devices
        /// </summary>
        /// <remarks>
        /// Returns a List of all &#x60;Devices&#x60;. 
        /// Can be paged with limit and offset.
        /// </remarks>
        /// <param name="limit">Limit</param>
        /// <param name="offset">Offset</param>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
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

                var deviceList = new DeviceListViewModel(devices.Skip(offset).Take(limit), limit, devices.Count);

                return Ok(deviceList);

            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: {0}", ex);
                return InternalServerError();
            }
        }

        // GET: api/devices/search
        /// <summary>
        /// Search for Devices
        /// </summary>
        /// <remarks>
        /// Search for &#x60;Devices&#x60;. Looks into Name and Brand
        /// Can be paged with limit and offset.
        /// </remarks>
        /// <param name="searchText">Search Query</param>
        /// <param name="limit">Limit</param>
        /// <param name="offset">Offset</param>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
        [ResponseType(typeof(DeviceListViewModel))]
        [Route("search")]
        public IHttpActionResult GetSearch(string searchText, int limit = 25, int offset = 0)
        {
            try
            {
                var result = searchText.ToLower().Split(new char[] { ' ', ',' }).ToList();
                var devices = _bl.GetDevices()
                    .ToList()
                    .Select(i => new DeviceViewModel(i).LoadMeta(i))
                    .ToList();
                result.ForEach(i =>
                {
                    devices = devices.Where(x => x.Name.ToLower().Contains(i) || x.Marke.ToLower().Contains(i))
                              .ToList();
                });

                var deviceList = new DeviceListViewModel(devices.Skip(offset).Take(limit), limit, devices.Count);

                return Ok(deviceList);

            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: {0}", ex);
                return InternalServerError();
            }
        }


        /// <summary>
        /// Returns Device by ID
        /// </summary>
        /// <remarks>Returns a &#x60;Device&#x60; by its ID</remarks>
        /// <param name="id">Unique identifier of a Device</param>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
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
                    _log.WarnFormat("Not Found: Devcie '{0}' not found", id);
                    return NotFound();
                }

                return Ok(vmdl);
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: {0}", ex);
                return InternalServerError();
            }

        }

        /// <summary>
        /// Returns Device by InvNum
        /// </summary>
        /// <remarks>Returns a &#x60;Device&#x60; by its unique InvNum</remarks>
        /// <param name="invNum">Unique identifier of a &#x60;Device&#x60;</param>
        /// <response code="200"></response>
        /// <response code="404">An error occured, Device not found</response>
        /// <response code="500">An error occured, please read log files</response>
        [ResponseType(typeof(DeviceViewModel))]
        [Route("invnum")]
        public IHttpActionResult GetDevice(string invNum)
        {
            try
            {
                invNum = invNum.Replace(" ", "+");
                var d = _bl.GetSingleDevice(invNum);
                var vmdl = new DeviceViewModel(d).LoadMeta(d);


                if (vmdl == null)
                {
                    _log.WarnFormat("Not Found: Device '{0}' not found", invNum);
                    return NotFound();
                }

                return Ok(vmdl);
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: {0}", ex);
                return InternalServerError();
            }

        }



        // POST: api/devices/filter/
        /// <summary>
        /// Filters the Devices with given parameters
        /// </summary>
        /// <remarks>
        /// Filters the &#x60;Devices&#x60; with the given parameters.
        /// Can be any Field of a &#x60;Device&#x60; (e.g: CPU, OS, GPU)
        /// </remarks>
        /// <param name="vmdl">Filter as FilterViewModel</param>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
        [ResponseType(typeof(DeviceListViewModel))]
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

                return Ok(new DeviceListViewModel(b, vmdl.Limit, count));
            }
            catch (SecurityException)
            {
                _log.WarnFormat("'{0}' tried to list Devices as Admin/Verwalter", _bl.GetCurrentUid());
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: {0}", ex);
                return InternalServerError();
            }
        }

        // GET: api/devices/types
        /// <summary>
        /// Returns all DeviceTypes
        /// </summary>
        /// <remarks>
        /// Returns a List of all &#x60;DeviceTypes&#x60; (e.g: PC, Notebook, TV).
        /// </remarks>
        /// <param name="showEmptyDeviceTypes">Boolean to show &#x60;DeviceTypes&#x60; which do not contain a &#x60;Device&#x60;</param>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
        [ResponseType(typeof(List<string>))]
        [Route("types")]
        public IHttpActionResult GetDeviceTypes(bool showEmptyDeviceTypes = true)
        {
            try
            {
                var deviceTypes = _bl.GetDeviceTypes()
                    .ToList()
                    .Select(i => new DeviceTypeViewModel(i).LoadFieldGroups(i))
                    .ToList();

                if (showEmptyDeviceTypes) return Ok(deviceTypes);

                var devices = _bl.GetDevices().GroupBy(i => i.Type.Slug).Select(i => i.Key).ToList();
                deviceTypes = deviceTypes.Where(i => devices.Contains(i.Slug)).ToList();

                return Ok(deviceTypes.OrderBy(i => i.Name));
            }
            catch
            {
                return InternalServerError();
            }

        }

        /// <summary>
        /// Creates a new Device
        /// </summary>
        /// <remarks>
        /// Creates a new &#x60;Device&#x60;.
        /// </remarks>
        /// <param name="vmdl">Device as &#x60;DeviceViewModel&#x60;</param>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
        [Authorize (Roles = "Admin, Verwalter")]
        [Route("")]
        [ResponseType(typeof(DeviceViewModel))]
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
                _log.WarnFormat("Security: '{0}' tried to create Device '{1}'", _bl.GetCurrentUid(), vmdl.InvNum);
                return Unauthorized();
            }

            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: {0}", ex);
                return InternalServerError();
            }
        }

        // POST: api/devices/types/
        /// <summary>
        /// Create New DeviceType
        /// </summary>
        /// <remarks>
        /// Creates a New &#x60;DeviceType&#x60;</remarks>
        /// <param name="vmdl">DeviceType as &#x60;DeviceTypeViewModel&#x60;</param>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
        [Authorize(Roles = "Admin, Verwalter")]
        [ResponseType(typeof(DeviceTypeViewModel))]
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
                _log.WarnFormat("Security: '{0}' tried to create DeviceType '{1}'", _bl.GetCurrentUid(), vmdl.Name);
                return Unauthorized();
            }

            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: '{0}'", ex);
                return InternalServerError();
            }
        }

        // DELETE: api/devices/id/{id}
        /// <summary>
        /// Deletes a Device
        /// </summary>
        /// <remarks>
        /// Deletes a &#x60;Device&#x60;.
        /// &#x60;Devices&#x60; are not removed from the database (Sets IsActive to 0).
        /// </remarks>
        /// <param name="id">Device ID</param>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
        [Authorize(Roles = "Admin, Verwalter")]
        [Route("id/{id}")]
        [ResponseType(typeof(OkResult))]
        public IHttpActionResult DeleteDevice(int id)
        {
            try
            {
                if (!_bl.DeviceExists(id))
                {
                    _log.WarnFormat("Not Found: Device '{0}' not found", id);
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
                _log.WarnFormat("Security: '{0}' tried to delete Device '{1}'", _bl.GetCurrentUid(), id);
                return Unauthorized();
            }

            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: {0}", ex);
                return InternalServerError();
            }
        }

        // DELETE: api/devices/types/{slug}
        /// <summary>
        /// Delete a DeviceType
        /// </summary>
        /// <remarks>
        /// Deletes a &#x60;DeviceType&#x60;.
        /// Only gets removed from the Database if not used by any &#x60;Devices&#x60;.
        /// (IsActive set to 0)
        /// </remarks>
        /// <param name="slug">Unique name for a &#x60;DeviceType&#x60;</param>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
        [Authorize(Roles = "Admin, Verwalter")]
        [Route("types/{slug}")]
        public IHttpActionResult DeleteDeviceType(string slug)
        {

            try
            {

                if (_bl.GetDeviceType(slug) == null)
                {
                    _log.WarnFormat("Not Found: DeviceType '{0}' not found", slug);
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
                _log.WarnFormat("Security: '{0}' tried to delete DeviceType '{1}'", _bl.GetCurrentUid(), slug);
                return Unauthorized();
            }

            catch(Exception ex)
            {
                _log.ErrorFormat("Exception: '{0}'", ex);
                return InternalServerError();
            }
        }

        // PUT: api/devices/id/{id}
        /// <summary>
        /// Update a Device
        /// </summary>
        /// <remarks>&#x60;Updates a Device&#x60;</remarks>
        /// <param name="id">Device Id</param>
        /// <param name="vmdl">Device as &#x60;DeviceViewModel&#x60;</param>
        /// <response code="200"></response>
        /// <response code="400">An error occured, id and vmdl.DeviceId have to be equal</response>
        /// <response code="500">An error occured, please read log files</response>
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



                var dev = _bl.GetSingleDevice(vmdl.DeviceId);
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
                    _log.WarnFormat("Not Found: Device '{0}' not found", id);
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (SecurityException)
            {
                _log.WarnFormat("Security: '{0}' tried to update Device '{1}'", vmdl.InvNum);
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: {0}", ex);
                return InternalServerError();
            }



            return Ok(vmdl);
        }

        // PUT: api/devices/types/{slug}
        /// <summary>
        /// Edit DeviceType
        /// </summary>
        /// <remarks>Edit a &#x60;DeviceType&#x60;</remarks>
        /// <param name="slug">Unique name for a &#x60;DeviceType&#x60;</param>
        /// <param name="vmdl">DeviceType as &#x60;DeviceType&#x60;</param>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
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
                vmdl.Refresh(dt);
                return Ok(vmdl);

            }
            catch (DbUpdateConcurrencyException)
            {
                if (_bl.GetDeviceType(slug) == null)
                {
                    _log.WarnFormat("Not Found: DeviceType '{0}' not found", slug);
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (SecurityException)
            {
                _log.WarnFormat("Security: '{0}' tried to update DeviceType '{1}'", _bl.GetCurrentUid(), slug);
                return Unauthorized();
            }

            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: {0}", ex);
                return InternalServerError();
            }

           

        }

        // GET: api/devices/accessories/
        /// <summary>
        /// Get Accessories
        /// </summary>
        /// <remarks>Get a List of all &#x60;Accessories&#x60;</remarks>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
        [ResponseType(typeof(List<AccessoryViewModel>))]
        [Route("accessories")]
        public IHttpActionResult GetAccessories()
        {
            try
            {
                var vmdls = _bl.GetAccessories()
                    .ToList()
                    .Select(i => new AccessoryViewModel(i))
                    .ToList();

                return Ok(vmdls);
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: {0}", ex);
                return InternalServerError();
            }
        }

        // GET: api/devices/accessories/{slug}
        /// <summary>
        /// Get a Single Accessory
        /// </summary>
        /// <remarks>Returns a Single &#x60;Accessory&#x60; by its Slug</remarks>
        /// <param name="slug">Internal name for a &#x60;Accessory&#x60; </param>
        /// <response code="200"></response>
        /// <response code="400">An error occured, &#x60;Accessory&#x60; not found</response>
        /// <response code="500">An error occured, please read log files</response>
        [ResponseType(typeof(DeviceViewModel))]
        [Route("accessories/{slug}")]
        public IHttpActionResult GetAccessory(string slug)
        {
            try
            {
                var obj = _bl.GetAccessory(slug);
                if (obj == null) return NotFound();

                return Ok(new AccessoryViewModel(obj));
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: {0}", ex);
                return InternalServerError();
            }
        }

        // POST: api/devices/accessories/
        /// <summary>
        /// Create Accessory
        /// </summary>
        /// <remarks>Creatse a new &#x60;Accessory&#x60;</remarks>
        /// <param name="vmdl">Accessory as &#x60;AccessoryViewModel&#x60;</param>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
        [ResponseType(typeof(DeviceViewModel))]
        [Route("accessories")]
        public IHttpActionResult PostAccessory([FromBody] AccessoryViewModel vmdl)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var obj = _bl.CreateAccessory();
                vmdl.ApplyChanges(obj, _bl);
                _bl.SaveChanges();
                vmdl.Refresh(obj);

                return Ok(vmdl);
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: {0}", ex);
                return InternalServerError();
            }
        }

        // PUT: api/devices/accessories/
        /// <summary>
        /// Edit an Accessory
        /// </summary>
        /// <remarks>Update an &#x60;Accessory&#x60;</remarks>
        /// <param name="slug">Unique name of an &#x60;Accessory&#x60;</param>
        /// <param name="vmdl">Accessory as &#x60;AccessoryViewModel&#x60;</param>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
        [ResponseType(typeof(DeviceViewModel))]
        [Route("accessories/{slug}")]
        public IHttpActionResult PutAccessory(string slug, [FromBody] AccessoryViewModel vmdl)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var obj = _bl.GetAccessory(slug);
                if (obj == null) return NotFound();

                _bl.UpdateAccessory(obj);
                vmdl.ApplyChanges(obj, _bl);
                _bl.SaveChanges();
                vmdl.Refresh(obj);

                return Ok(vmdl);
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: {0}", ex);
                return InternalServerError();
            }
        }

        // DELETE: api/devices/accessories/{slug}
        /// <summary>
        /// Delete an Accessory
        /// </summary>
        /// <remarks>Delete an &#x60;Accessory&#x60;</remarks>
        /// <param name="slug">Unique name of an &#x60;Accessory&#x60;</param>
        /// <response code="200"></response>
        /// <response code="404">An error occured, &#x60;Accessory&#x60; not found</response>
        /// <response code="500">An error occured, please read log files</response>
        [ResponseType(typeof(DeviceViewModel))]
        [Route("accessories/{slug}")]
        public IHttpActionResult DeleteAccessory(string slug)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var obj = _bl.GetAccessory(slug);
                if (obj == null) return NotFound();

                _bl.DeleteAccessory(obj);
                _bl.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: {0}", ex);
                return InternalServerError();
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