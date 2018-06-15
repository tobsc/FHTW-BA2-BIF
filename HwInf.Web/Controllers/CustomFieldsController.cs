using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using HwInf.BusinessLogic.Interfaces;
using HwInf.Web.ViewModels;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoreLinq;

namespace HwInf.Web.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Controller managing Custom Fields
    /// </summary>
    [Authorize]
    [Route("api/customfields")]
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public class CustomFieldsController : Controller
    {
        private readonly IBusinessLogicFacade _bl;
        private readonly ILog _log = LogManager.GetLogger(typeof(CustomFieldsController));

        public CustomFieldsController(IBusinessLogicFacade bl)
        {
            _bl = bl;
        }

        /// <summary>
        /// Get FieldGroups
        /// </summary>
        /// <remarks>
        /// Returns a List of all &#x60;FieldGroups&#x60; and their &#x60;Fields&#x60;. 
        /// A &#x60;FieldGroup&#x60; represents a component of a &#x60;Device&#x60; (e.g: CPU, GPU, OS)
        /// </remarks>
        /// <response code="500">An error occured, please read log files</response>
        /// <response code="200"></response>
        [Route("fieldgroups")]
        [HttpGet]
        public IActionResult GetGroups()
        {
            var test = _bl.GetFieldGroups().ToList();
            var vmdl = _bl.GetFieldGroups()
                .ToList()
                .Select(i => new FieldGroupViewModel(i))
                .OrderBy(i => i.Name)
                .ToList();

            return Ok(vmdl);
        }


        /// <summary>
        /// Get FieldGroups of DeviceType
        /// </summary>
        /// <remarks>Returns a list of &#x60;Fieldgroups&#x60; belonging to a &#x60;DeviceType&#x60; (e.g. PC, Notebook, TV) </remarks>
        /// <response code="500">An error occured, please read log files</response>
        /// <response code="200"></response>
        /// <param name="typeSlug">Internal unique name of a &#x60;DeviceType&#x60;</param>
        [Route("fieldgroups/{typeSlug}")]
        [HttpGet]
        public IActionResult GetGroupsOfDeviceType(string typeSlug)
        {
            var dt = _bl.GetDeviceType(typeSlug);

            var vmdl = _bl.GetFieldGroups()
                .ToList()
                .Where(i => i.DeviceTypes == null || i.DeviceTypes.Contains(dt))
                .Select(i => new FieldGroupViewModel(i))
                .OrderBy(i => i.Name)
                .ToList();

            return Ok(vmdl);
        }

        /// <summary>
        /// Create FieldGroup
        /// </summary>
        /// <remarks>Add a new &#x60;FieldGroup&#x60;. A &#x60;FieldGroup&#x60; represents a component of a &#x60;Device&#x60; (e.g: CPU, GPU, OS)</remarks>
        /// <response code="500">An error occured, please read log files</response>
        /// <response code="200"></response>
        /// <param name="vmdl">New &#x60;FieldGroup&#x60; as &#x60;FieldGroupViewModel&#x60;</param>
        [Authorize(Roles = "Admin, Verwalter")]
        [Route("fieldgroups")]
        [HttpPost]
        public IActionResult PostGroup([FromBody]FieldGroupViewModel vmdl)
        {
            var obj = _bl.CreateFieldGroup();
            vmdl.ApplyChanges(obj, _bl);
            _bl.SaveChanges();
            vmdl.Refresh(obj);
            _log.InfoFormat("New FieldGroup '{0}' created by '{1}'", vmdl.Name, User.Identity.Name);
            return Ok(vmdl);
        }

        /// <summary>
        /// Add Field to FieldGroup
        /// </summary>
        /// <remarks>
        /// Add a new &#x60;Field&#x60; to a &#x60;FieldGroup&#x60;. 
        /// A &#x60;Fieldgroup&#x60; represents a component of a &#x60;Device&#x60; (e.g: CPU, GPU, OS).
        /// A &#x60;Field&#x60; represents a specific component(e.g: i5-4590, Windows 10)
        /// </remarks>
        /// <response code="500">An error occured, please read log files</response>
        /// <response code="200"></response>
        /// <param name="groupSlug">Unique name of a &#x60;FieldGroup&#x60;</param>
        /// <param name="vmdl">New Field as &#x60;FieldViewModel&#x60;</param>
        [Authorize(Roles = "Admin, Verwalter")]
        [Route("fields")]
        [HttpPost]
        public IActionResult PostField(string groupSlug, [FromBody]FieldViewModel vmdl)
        {
            var obj = _bl.GetFieldGroup(groupSlug);
            _bl.UpdateFieldGroup(obj);

            var field =_bl.CreateField();
            vmdl.ApplyChanges(field, _bl);
            obj.Fields.Add(field);

            _bl.SaveChanges();
            vmdl.Refresh(field);
            _log.InfoFormat("New Field '{0}' added to '{1}' by '{2}'", vmdl.Name, obj.Name, User.Identity.Name);
            return Ok(vmdl);
        }

        /// <summary>
        /// Add DeviceType to FieldGroup
        /// </summary>
        /// <remarks>
        /// Add a &#x60;DeviceType&#x60; (e.g: PC, TV) to a &#x60;FieldGroup&#x60;
        /// A &#x60;Fieldgroup&#x60; represents a component of a &#x60;Device&#x60; (e.g: CPU, GPU, OS).
        /// </remarks>
        /// <response code="500">An error occured, please read log files</response>
        /// <response code="200"></response>
        /// <param name="typeSlug">Unique name of a &#x60;DeviceType&#x60;</param>
        /// <param name="groupSlug">Unique name of a &#x60;FieldGroup&#x60;</param>
        [Authorize(Roles = "Admin, Verwalter")]
        [Route("fieldgroups/types")]
        [HttpPost]
        public IActionResult PostGroupType(string typeSlug, string groupSlug)
        {

            var fg = _bl.GetFieldGroup(groupSlug);
            var dt = _bl.GetDeviceType(typeSlug);

            _bl.UpdateFieldGroup(fg);
            fg.DeviceTypes.Add(dt);
            _bl.SaveChanges();
            _log.InfoFormat("DeviceType '{0}' added to '{1}' by '{2}'", dt.Name, fg.Name, User.Identity.Name);
            return Ok();
        }


        /// <summary>
        /// Update FieldGroup
        /// </summary>
        /// <remarks>Update a &#x60;FieldGroup&#x60;. A &#x60;Fieldgroup&#x60; represents a component of a &#x60;Device&#x60; (e.g: CPU, GPU, OS).</remarks>
        /// <param name="slug">Unique name of a &#x60;FieldGroup&#x60;</param>
        /// <param name="vmdl">Updated &#x60;FieldGroup&#x60; as &#x60;FieldGroupViewModel&#x60;</param>
        /// <response code="500">An error occured, please read log files</response>
        /// <response code="400">An error occured, slug and vmdl.slug have to be equivalent</response>
        /// <response code="404">An error occured, &#x60;FieldGroup&#x60; not found</response>
        /// <response code="200"></response>

        [Authorize(Roles = "Admin, Verwalter")]
        [HttpPut]
        [Route("fieldgroups/{slug}")]
        [HttpPut]
        public IActionResult PutFieldGroups(string slug, [FromBody]FieldGroupViewModel vmdl)
        {

            if (slug != vmdl.Slug)
            {
                return BadRequest();
            }

            try
            {
                var fg = _bl.GetFieldGroup(vmdl.Slug);
                _bl.UpdateFieldGroup(fg);
                var f = fg.Fields.ToList();
                f.ForEach(i => _bl.DeleteField(i));
                fg.Fields.Clear();
                _bl.SaveChanges();
                vmdl.ApplyChanges(fg, _bl);
                vmdl.Refresh(fg);

                var meta = _bl.GetDeviceMeta()
                    .Where(i => i.FieldGroupSlug.Equals(slug))
                    .ToList();
                meta.ForEach(i => i.FieldGroupSlug = vmdl.Slug);

                _bl.SaveChanges();
                _log.InfoFormat("FieldGroup '{0}' updated by '{1}'", vmdl.Name, User.Identity.Name);

                vmdl.Refresh(fg);
                return Ok(vmdl);

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_bl.FieldGroupExists(vmdl.Slug))
                {
                    _log.WarnFormat("Not Found: FieldGroup '{0}' not found", slug);
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// Get FieldGroups with used Field
        /// </summary>
        /// <remarks>
        /// Used for the filter.
        /// Only returns &#x60;Fields&#x60; of a &#x60;FieldGroup&#x60; which have a &#x60;Device&#x60; using them.
        /// </remarks>
        /// <response code="500">An error occured, please read log files</response>
        /// <response code="200"></response>
        [Route("filter/fieldgroups/")]
        [HttpGet]
        public IActionResult GetFieldGroupsUsedFields()
        {

            var vmdl = _bl.GetFieldGroups()
                .ToList()
                .Select(i => new FieldGroupViewModel(i))
                .ToList();


            var meta = _bl.GetFilteredDevices(null)
                .ToList()
                .Select(i => new DeviceViewModel(i).LoadMeta(i))
                .ToList()
                .SelectMany(i => i.DeviceMeta)
                .ToList()
                .Select(i => i.FieldSlug)
                .ToList();


            // Bad?
            var res = new List<FieldGroupViewModel>();

            vmdl.ForEach(i =>
            {
                var fieldGroup = new FieldGroupViewModel
                {
                    Name = i.Name,
                    Slug = i.Slug,
                    Fields = new List<FieldViewModel>(),
                    IsCountable = i.IsCountable
                };

                i.Fields.ForEach(y =>
                {
                    if (!meta.Contains(y.Slug)) return;
                    fieldGroup.Fields.Add(y);
                });

                res.Add(fieldGroup);
            });

            return Ok(res.OrderBy(i => i.Name).ToList());



        }

        /// <summary>
        /// Get FieldGroups with used Fields from DeviceType
        /// </summary>
        /// <remarks>
        /// Used for the filter.
        /// Only returns &#x60;Fields&#x60; of a &#x60;FieldGroup&#x60; which have a &#x60;Device&#x60; using them for a specific &#x60;DeviceType&#x60;.
        /// </remarks>
        /// <param name="typeSlug">Unique name of a &#x60;DeviceType&#x60;</param>
        /// <response code="500">An error occured, please read log files</response>
        /// <response code="200"></response>


        [Route("filter/fieldgroups/{typeslug}")]
        [HttpPost]
        public IActionResult GetFieldGroupsUsedFieldsType(string typeSlug)
        {
            var dt = _bl.GetDeviceType(typeSlug);

            var vmdl = _bl.GetFieldGroups()
                .ToList()
                .Where(i => i.DeviceTypes == null || i.DeviceTypes.Contains(dt))
                .Select(i => new FieldGroupViewModel(i))
                .ToList();


            var meta = _bl.GetFilteredDevices(null, typeSlug)
                .ToList()
                .Select(i => new DeviceViewModel(i).LoadMeta(i))
                .ToList()
                .SelectMany(i => i.DeviceMeta)
                .ToList()
                .Select(i => i.FieldSlug)
                .ToList();


            // Bad?
            var res = new List<FieldGroupViewModel>();

            vmdl.ForEach(i =>
            {
                var fieldGroup = new FieldGroupViewModel
                {
                    Name = i.Name,
                    Slug = i.Slug,
                    Fields = new List<FieldViewModel>(),
                    IsCountable = i.IsCountable
                };

                i.Fields.ForEach(y =>
                {
                    if (!meta.Contains(y.Slug)) return;
                    fieldGroup.Fields.Add(y);
                });

                res.Add(fieldGroup);
            });

            return Ok(res);
        }

        /// <summary>
        /// Delete FieldGroup
        /// </summary>
        /// <remarks>
        /// Delete a &#x60;FieldGroup&#x60;
        /// &#x60;FieldGroups&#x60; are not removed from the Database if they are used by any &#x60;Device&#x60;
        /// </remarks>
        /// <param name="slug">Unique name for a &#x60;FieldGroup&#x60;</param>
        /// <response code="500">An error occured, please read log files</response>
        /// <response code="200">&#x60;FieldGroup&#x60; deleted</response>
        [Authorize(Roles = "Admin, Verwalter")]
        [Route("fieldgroups/{slug}")]
        [HttpPost]
        public IActionResult DeleteFieldGroup(string slug)
        {
            try
            {
                var obj = _bl.GetFieldGroup(slug);
                _bl.DeleteFieldGroup(obj);
                _bl.SaveChanges();
                _log.InfoFormat("FieldGroup '{0}' deleted by '{1}'", obj.Name, User.Identity.Name);


            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_bl.FieldGroupExists(slug))
                {
                    _log.WarnFormat("Not Found: FieldGroup '{0}' not found", slug);
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }
    }

}