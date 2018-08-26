using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using HwInf.BusinessLogic.Interfaces;
using HwInf.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoreLinq;

namespace HwInf.Web.Controllers
{
    [Authorize]
    [Route("api/devices")]
    public class DevicesController : Controller
    {
        private readonly IBusinessLogicFacade _bl;
        private readonly ILogger<DevicesController> _log;

        public DevicesController(IBusinessLogicFacade bl, ILogger<DevicesController> logger)
        {
            _bl = bl;
            _log = logger;
        }

        /// <summary>
        /// Get DeviceStatus
        /// </summary>
        /// <remarks>Returns a list of &#x60;DeviceStatus&#x60;</remarks>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
        [Route("status")]
        [HttpGet]
        public IActionResult GetDeviceStatuses()
        {


            var vmdls = _bl.GetDeviceStatuses()
                .ToList()
                .Select(i => new DeviceStatusViewModel(i))
                .OrderBy(i => i.Description);

            return Ok(vmdls);
        }

        /// <summary>
        /// Get all Devices
        /// </summary>
        /// <remarks>
        /// Returns a List of all &#x60;Devices&#x60;. 
        /// Can be paged with limit and offset.
        /// </remarks>
        /// <param name="limit">Limit</param>
        /// <param name="offset">Offset</param>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
        [Route("")]
        [HttpGet]
        public IActionResult GetAll(int limit = 25, int offset = 0)
        {
            try
            {
                var devices = _bl.GetDevices()
                    .ToList()
                    .Select(i =>
                    {
                        var newDeviceViewModel = new DeviceViewModel(i).LoadMeta(i);
                        newDeviceViewModel.Stock = _bl.GetDevices().ToList().Where(j => j.Status.Description == "Verfügbar")
                            .Count(j => j.DeviceGroupSlug == newDeviceViewModel.DeviceGroupSlug);
                        return newDeviceViewModel;
                    })
                    .ToList();

                var deviceList = new DeviceListViewModel(devices.Skip(offset).Take(limit), limit, devices.Count);

                return Ok(deviceList);

            }
            catch (Exception ex)
            {
                _log.LogError("Exception: {0}", ex);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Search Devices
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
        [Route("search")]
        [HttpGet]
        public IActionResult GetSearch(string searchText, int limit = 25, int offset = 0)
        {
            try
            {
                var result = searchText.ToLower().Split(new char[] { ' ', ',' }).ToList();
                var devices = _bl.GetFilteredDevicesUser(null)
                    .ToList()
                    .Select(i =>
                    {
                        var newDeviceViewModel = new DeviceViewModel(i).LoadMeta(i);
                        newDeviceViewModel.Stock = _bl.GetDevices().ToList().Where(j => j.Status.Description == "Verfügbar")
                            .Count(j => j.DeviceGroupSlug == newDeviceViewModel.DeviceGroupSlug);
                        return newDeviceViewModel;
                    })
                    .ToList();
                result.ForEach(i =>
                {
                    devices = devices.Where(x => x.Name.ToLower().Contains(i) || x.Marke.ToLower().Contains(i) || x.InvNum.Contains(i))
                              .ToList();
                });

                var deviceList = new DeviceListViewModel(devices.Skip(offset).Take(limit), limit, devices.Count);

                return Ok(deviceList);

            }
            catch (Exception ex)
            {
                _log.LogError("Exception: {0}", ex);
                return StatusCode(500);
            }
        }


        /// <summary>
        /// Get Device by ID
        /// </summary>
        /// <remarks>Returns a &#x60;Device&#x60; by its ID</remarks>
        /// <param name="id">Unique identifier of a Device</param>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
        [Route("id/{id}")]
        [HttpGet]
        public IActionResult GetDevice(int id)
        {
            try
            {
                var d = _bl.GetSingleDevice(id);
                var vmdl = new DeviceViewModel(d).LoadMeta(d);
                

                if (vmdl == null)
                {
                    _log.LogWarning("Not Found: Devcie '{0}' not found", id);
                    return NotFound();
                }

                vmdl.Stock = _bl.GetDevices().ToList().Where(j => j.Status.Description == "Verfügbar")
                    .Count(j => j.DeviceGroupSlug == vmdl.DeviceGroupSlug);

                return Ok(vmdl);
            }
            catch (Exception ex)
            {
                _log.LogError("Exception: {0}", ex);
                return StatusCode(500);
            }

        }

        /// <summary>
        /// Get Device by InvNum
        /// </summary>
        /// <remarks>Returns a &#x60;Device&#x60; by its unique InvNum</remarks>
        /// <param name="invNum">Unique identifier of a &#x60;Device&#x60;</param>
        /// <response code="200"></response>
        /// <response code="404">An error occured, Device not found</response>
        /// <response code="500">An error occured, please read log files</response>
        [Route("invnum")]
        [HttpGet]
        public IActionResult GetDevice(string invNum)
        {
            try
            {
                invNum = invNum.Replace(" ", "+");
                var d = _bl.GetSingleDevice(invNum);
                var vmdl = new DeviceViewModel(d).LoadMeta(d);


                if (vmdl == null)
                {
                    _log.LogWarning("Not Found: Device '{0}' not found", invNum);
                    return NotFound();
                }

                vmdl.Stock = _bl.GetDevices().Where(j => j.Status.Description == "Verfügbar")
                    .Count(j => j.DeviceGroupSlug == vmdl.DeviceGroupSlug);


                return Ok(vmdl);
            }
            catch (Exception ex)
            {
                _log.LogError("Exception: {0}", ex);
                return StatusCode(500);
            }

        }



        /// <summary>
        /// Filter Devices
        /// </summary>
        /// <remarks>
        /// Filters the &#x60;Devices&#x60; with the given parameters.
        /// Can be any Field of a &#x60;Device&#x60; (e.g: CPU, OS, GPU)
        /// </remarks>
        /// <param name="vmdl">Filter as FilterViewModel</param>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
        [Route("filter")]
        [HttpPost]
        public IActionResult PostFilter([FromBody] FilterViewModel vmdl)
        {
            try
            {
                vmdl.OrderBy = vmdl.OrderBy ?? "Name";
                vmdl.Order = vmdl.Order ?? "ASC";

                var b = vmdl.FilteredList(_bl)
                    .ToList()
                    .Select(i =>
                    {
                        var newDeviceViewModel = new DeviceViewModel(i).LoadMeta(i);
                        newDeviceViewModel.Stock = _bl.GetDevices().ToList().Where(j => j.Status.Description == "Verfügbar")
                            .Count(j => j.DeviceGroupSlug == newDeviceViewModel.DeviceGroupSlug);
                        return newDeviceViewModel;
                    })
                    .ToList();
                var count = b.Count;
                b = vmdl.Limit < 0
                    ? b.ToList()
                    : b.Skip(vmdl.Offset).Take(vmdl.Limit).ToList();

                return Ok(new DeviceListViewModel(b, vmdl.Limit, count));
            }
            catch (SecurityException)
            {
                _log.LogWarning("'{0}' tried to list Devices as Admin/Verwalter", _bl.GetCurrentUid());
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _log.LogError("Exception: {0}", ex);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Filter Devices
        /// </summary>
        /// <remarks>
        /// Filters the &#x60;Devices&#x60; with the given parameters.
        /// Can be any Field of a &#x60;Device&#x60; (e.g: CPU, OS, GPU)
        /// </remarks>
        /// <param name="vmdl">Filter as FilterViewModel</param>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
        [Route("filteruser")]
        [HttpPost]
        public IActionResult PostFilterUser([FromBody] FilterViewModel vmdl)
        {
            try
            {
                vmdl.OrderBy = vmdl.OrderBy ?? "Name";
                vmdl.Order = vmdl.Order ?? "ASC";

                var b = vmdl.FilteredListUser(_bl)
                    .ToList()
                    .Select(i =>
                    {
                        var newDeviceViewModel = new DeviceViewModel(i).LoadMeta(i);
                        newDeviceViewModel.Stock = _bl.GetDevices().ToList().Where(j => j.Status.Description == "Verfügbar")
                            .Count(j => j.DeviceGroupSlug == newDeviceViewModel.DeviceGroupSlug);
                        return newDeviceViewModel;
                    })
                    .ToList();
                var count = b.Count;
                b = vmdl.Limit < 0
                    ? b.ToList()
                    : b.Skip(vmdl.Offset).Take(vmdl.Limit).ToList();

                return Ok(new DeviceListViewModel(b, vmdl.Limit, count));
            }
            catch (SecurityException)
            {
                _log.LogWarning("'{0}' tried to list Devices as Admin/Verwalter", _bl.GetCurrentUid());
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _log.LogError("Exception: {0}", ex);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Get DeviceTypes
        /// </summary>
        /// <remarks>
        /// Returns a List of all &#x60;DeviceTypes&#x60; (e.g: PC, Notebook, TV).
        /// </remarks>
        /// <param name="showEmptyDeviceTypes">Boolean to show &#x60;DeviceTypes&#x60; which do not contain a &#x60;Device&#x60;</param>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
        [Route("types")]
        [HttpGet]
        public IActionResult GetDeviceTypes(bool showEmptyDeviceTypes = true)
        {
            try
            {
                var deviceTypes = _bl.GetDeviceTypes()
                    .ToList()
                    .Select(i => new DeviceTypeViewModel(i).LoadFieldGroups(i))
                    .ToList();

                if (showEmptyDeviceTypes) return Ok(deviceTypes.OrderBy(i => i.Name).ToList());

                var devices = _bl.GetDevices().GroupBy(i => i.Type.Slug).Select(i => i.Key).ToList();
                deviceTypes = deviceTypes.Where(i => devices.Contains(i.Slug)).ToList();

                return Ok(deviceTypes.OrderBy(i => i.Name).ToList());
            }
            catch(Exception ex)
            {
                return StatusCode(500);
            }

        }

        /// <summary>
        /// Create Device
        /// </summary>
        /// <remarks>
        /// Creates a new &#x60;Device&#x60;.
        /// </remarks>
        /// <param name="vmdl">Device as &#x60;DeviceViewModel&#x60;</param>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
        [Authorize (Roles = "Admin, Verwalter")]
        [HttpPost]
        [Route("")]
        public IActionResult PostDevice([FromBody]DeviceViewModel vmdl)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = string.IsNullOrWhiteSpace(vmdl.InvNum) 
                    ? CreateDeviceNoInvNum(vmdl) 
                    : CreateDeviceWithInvNum(vmdl);

                return Ok(response);

            }
            catch (SecurityException)
            {
                _log.LogWarning("Security: '{0}' tried to create Device '{1}'", _bl.GetCurrentUid(), vmdl.InvNum);
                return Unauthorized();
            }

            catch (ArgumentException ex)
            {
                _log.LogError("Exception: {0}", ex);
                return BadRequest(ex.Message);
            }

            catch (Exception ex)
            {
                _log.LogError("Exception: {0}", ex);
                return StatusCode(500);
            }
        }

        private List<DeviceViewModel> CreateDeviceNoInvNum(DeviceViewModel vmdl)
        {
            var response = new List<DeviceViewModel>();

            // Check for new fields and add them
            vmdl.DeviceMeta.ForEach(i =>
            {
                var fg = _bl.GetFieldGroup(i.FieldGroupSlug);
                if (fg.Fields.Count(j => j.Name == i.Field) == 0)
                {
                    _bl.UpdateFieldGroup(fg);
                    var field = _bl.CreateField();
                    var fvmdl = new FieldViewModel { Name = i.Field };
                    fvmdl.ApplyChanges(field, _bl);
                    fg.Fields.Add(field);
                }
            });

            for (var i = 0; i < vmdl.Quantity; i++)
            {
                var d = _bl.CreateDevice();
                d.CreateDate = DateTime.Now;
                vmdl.DeviceGroupSlug = vmdl.Name + "-" + _bl.GetCurrentUid();
                vmdl.ApplyChanges(d, _bl);
                response.Add(new DeviceViewModel(d).LoadMeta(d));
            }

            _bl.SaveChanges();

            return response;
        }

        private List<DeviceViewModel> CreateDeviceWithInvNum(DeviceViewModel vmdl)
        {
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
                throw new ArgumentException("Es existiert bereits ein Gerät mit dieser Inventarnummer.");
            }

            // Check for new fields and add them
            vmdl.DeviceMeta?.ForEach(i =>
            {
                var fg = _bl.GetFieldGroup(i.FieldGroupSlug);
                if (fg.Fields.Count(j => j.Name == i.Field) == 0)
                {
                    _bl.UpdateFieldGroup(fg);
                    var field = _bl.CreateField();
                    var fvmdl = new FieldViewModel { Name = i.Field };
                    fvmdl.ApplyChanges(field, _bl);
                    fg.Fields.Add(field);
                }
            });

            var response = new List<DeviceViewModel>();

            invNums?
                .Select(i => i.InvNum)?
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

            _log.LogInformation("Device '{0}({1})' added by '{2}'", vmdl.InvNum, vmdl.Name, User.Identity);
            if (vmdl.AdditionalInvNums != null)
            {
                            foreach (var n in vmdl.AdditionalInvNums)
            {
                _log.LogInformation("Device '{0}({1})' added by '{2}'", n.InvNum, vmdl.Name, User.Identity.Name);
            }
            }

            return response;
        }

        /// <summary>
        /// Create DeviceType
        /// </summary>
        /// <remarks>
        /// Creates a New &#x60;DeviceType&#x60;</remarks>
        /// <param name="vmdl">DeviceType as &#x60;DeviceTypeViewModel&#x60;</param>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
        [Authorize(Roles = "Admin, Verwalter")]
        [Route("types")]
        [HttpPost]
        public IActionResult PostDeviceType([FromBody]DeviceTypeViewModel vmdl)
        {
            try
            {
                var dt = _bl.CreateDeviceType();

                vmdl.ApplyChanges(dt, _bl);
                _bl.SaveChanges();
                _log.LogInformation("DeviceType '{0}' created by '{1}'", vmdl.Name, User.Identity.Name);

                vmdl.Refresh(dt);
                return Ok(vmdl);

            }
            catch (SecurityException)
            {
                _log.LogWarning("Security: '{0}' tried to create DeviceType '{1}'", _bl.GetCurrentUid(), vmdl.Name);
                return Unauthorized();
            }

            catch (Exception ex)
            {
                _log.LogError("Exception: '{0}'", ex);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Delete Device
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
        [HttpDelete]
        public IActionResult DeleteDevice(int id)
        {
            try
            {
                if (!_bl.DeviceExists(id))
                {
                    _log.LogWarning("Not Found: Device '{0}' not found", id);
                    return NotFound();
                }

                var d = _bl.GetSingleDevice(id);
                _bl.DeleteDevice(d);
                _bl.SaveChanges();
                _log.LogInformation("Device '{0}({1})' deleted by '{2}'", d.InvNum, d.Name, User.Identity.Name);

                return Ok(d);

            }
            catch (SecurityException)
            {
                _log.LogWarning("Security: '{0}' tried to delete Device '{1}'", _bl.GetCurrentUid(), id);
                return Unauthorized();
            }

            catch (Exception ex)
            {
                _log.LogError("Exception: {0}", ex);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Delete DeviceType
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
        [HttpDelete]
        public IActionResult DeleteDeviceType(string slug)
        {

            try
            {

                if (_bl.GetDeviceType(slug) == null)
                {
                    _log.LogWarning("Not Found: DeviceType '{0}' not found", slug);
                    return NotFound();
                }
                else
                {
                    var dt = _bl.GetDeviceType(slug);
                    _bl.DeleteDeviceType(dt);
                    _bl.SaveChanges();
                    _log.LogInformation("DeviceType '{0}' deleted by '{1}'", dt.Name, _bl.GetCurrentUid());
                }

                return Ok();

            }
            catch (SecurityException)
            {
                _log.LogWarning("Security: '{0}' tried to delete DeviceType '{1}'", _bl.GetCurrentUid(), slug);
                return Unauthorized();
            }

            catch(Exception ex)
            {
                _log.LogError("Exception: '{0}'", ex);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Update Device
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
        [HttpPut]
        public IActionResult PutDevice(int id, [FromBody]DeviceViewModel vmdl)
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
                    var fg = _bl.GetFieldGroup(i.FieldGroupSlug);
                    if (fg.Fields.Count(j => j.Name == i.Field) == 0)
                    {
                        _bl.UpdateFieldGroup(fg);
                        var field = _bl.CreateField();
                        field.FieldGroup = fg;
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

                _log.LogInformation("Device '{0}({1})' updated by '{2}'", vmdl.InvNum, vmdl.Name, User.Identity.Name);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_bl.DeviceExists(id))
                {
                    _log.LogWarning("Not Found: Device '{0}' not found", id);
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (SecurityException)
            {
                _log.LogWarning("Security: '{0}' tried to update Device '{1}'", User.Identity.Name, vmdl.InvNum);
            }
            catch (Exception ex)
            {
                _log.LogError("Exception: {0}", ex);
                return StatusCode(500);
            }



            return Ok(vmdl);
        }

        /// <summary>
        /// Edit DeviceType
        /// </summary>
        /// <remarks>Edit a &#x60;DeviceType&#x60;</remarks>
        /// <param name="slug">Unique name for a &#x60;DeviceType&#x60;</param>
        /// <param name="vmdl">DeviceType as &#x60;DeviceType&#x60;</param>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
        [Authorize(Roles="Admin, Verwalter")]
        [Route("types/{slug}")]
        [HttpPut]
        public IActionResult PutDeviceType(string slug, [FromBody]DeviceTypeViewModel vmdl)
        {
            try
            {
                var dt = _bl.GetDeviceType(slug);
                _bl.UpdateDeviceType(dt);
                vmdl.ApplyChanges(dt, _bl);
                _bl.SaveChanges();

                _log.LogInformation("DeviceType '{0}' updated by '{1}'", vmdl.Name, User.Identity.Name);
                vmdl.Refresh(dt);
                return Ok(vmdl);

            }
            catch (DbUpdateConcurrencyException)
            {
                if (_bl.GetDeviceType(slug) == null)
                {
                    _log.LogWarning("Not Found: DeviceType '{0}' not found", slug);
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (SecurityException)
            {
                _log.LogWarning("Security: '{0}' tried to update DeviceType '{1}'", _bl.GetCurrentUid(), slug);
                return Unauthorized();
            }

            catch (Exception ex)
            {
                _log.LogError("Exception: {0}", ex);
                return StatusCode(500);
            }

           

        }

        /// <summary>
        /// Get Accessories
        /// </summary>
        /// <remarks>Get a List of all &#x60;Accessories&#x60;</remarks>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
        [Route("accessories")]
        [HttpGet]
        public IActionResult GetAccessories()
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
                _log.LogError("Exception: {0}", ex);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Get Single Accessory
        /// </summary>
        /// <remarks>Returns a Single &#x60;Accessory&#x60; by its Slug</remarks>
        /// <param name="slug">Internal name for a &#x60;Accessory&#x60; </param>
        /// <response code="200"></response>
        /// <response code="400">An error occured, &#x60;Accessory&#x60; not found</response>
        /// <response code="500">An error occured, please read log files</response>
        [Route("accessories/{slug}")]
        [HttpGet]
        public IActionResult GetAccessory(string slug)
        {
            try
            {
                var obj = _bl.GetAccessory(slug);
                if (obj == null) return NotFound();

                return Ok(new AccessoryViewModel(obj));
            }
            catch (Exception ex)
            {
                _log.LogError("Exception: {0}", ex);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Create Accessory
        /// </summary>
        /// <remarks>Creatse a new &#x60;Accessory&#x60;</remarks>
        /// <param name="vmdl">Accessory as &#x60;AccessoryViewModel&#x60;</param>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
        [Route("accessories")]
        [HttpPost]
        public IActionResult PostAccessory([FromBody] AccessoryViewModel vmdl)
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
                _log.LogError("Exception: {0}", ex);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Edit Accessory
        /// </summary>
        /// <remarks>Update an &#x60;Accessory&#x60;</remarks>
        /// <param name="slug">Unique name of an &#x60;Accessory&#x60;</param>
        /// <param name="vmdl">Accessory as &#x60;AccessoryViewModel&#x60;</param>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
        [Route("accessories/{slug}")]
        [HttpPut]
        public IActionResult PutAccessory(string slug, [FromBody] AccessoryViewModel vmdl)
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
                _log.LogError("Exception: {0}", ex);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Delete Accessory
        /// </summary>
        /// <remarks>Delete an &#x60;Accessory&#x60;</remarks>
        /// <param name="slug">Unique name of an &#x60;Accessory&#x60;</param>
        /// <response code="200"></response>
        /// <response code="404">An error occured, &#x60;Accessory&#x60; not found</response>
        /// <response code="500">An error occured, please read log files</response>
        [Route("accessories/{slug}")]
        [HttpDelete]
        public IActionResult DeleteAccessory(string slug)
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
                _log.LogError("Exception: {0}", ex);
                return StatusCode(500);
            }
        }
    }

}