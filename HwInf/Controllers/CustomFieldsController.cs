using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
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
    /// <inheritdoc />
    /// <summary>
    /// Controller managing Custom Fields
    /// </summary>
    [Authorize]
    [RoutePrefix("api/customfields")]
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public class CustomFieldsController : ApiController
    {
        private readonly IDAL _db;
        private readonly BL _bl;
        private readonly ILog _log = LogManager.GetLogger(typeof(CustomFieldsController).Name);

        public CustomFieldsController()
        {
            _db = new HwInfContext();
            _bl = new BL(_db);
        }

        public CustomFieldsController(IDAL db)
        {
            _db = db;
            _bl = new BL(db);
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
        [ResponseType(typeof(List<FieldGroupViewModel>))]
        [Route("fieldgroups")]
        public IHttpActionResult GetGroups()
        {

            var vmdl = _bl.GetFieldGroups()
                .ToList()
                .Select(i => new FieldGroupViewModel(i))
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
        [ResponseType(typeof(List<FieldGroupViewModel>))]
        [Route("fieldgroups/{typeSlug}")]
        public IHttpActionResult GetGroupsOfDeviceType(string typeSlug)
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
        [ResponseType(typeof(FieldGroupViewModel))]
        [Route("fieldgroups")]
        public IHttpActionResult PostGroup(FieldGroupViewModel vmdl)
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
        [ResponseType(typeof(FieldViewModel))]
        [Route("fields")]
        public IHttpActionResult PostField(string groupSlug, FieldViewModel vmdl)
        {
            var obj = _bl.GetFieldGroups(groupSlug);
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
        [ResponseType(typeof(OkResult))]
        [Route("fieldgroups/types")]
        public IHttpActionResult PostGroupType(string typeSlug, string groupSlug)
        {

            var fg = _bl.GetFieldGroups(groupSlug);
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
        [ResponseType(typeof(FieldGroupViewModel))]
        [Route("fieldgroups/{slug}")]
        public IHttpActionResult PutFieldGroups(string slug, FieldGroupViewModel vmdl)
        {

            if (slug != vmdl.Slug)
            {
                return BadRequest();
            }

            try
            {
                var fg = _bl.GetFieldGroups(vmdl.Slug);
                _bl.UpdateFieldGroup(fg);
                var f = fg.Fields.ToList();
                f.ForEach(i => _bl.DeleteField(i));
                fg.Fields.Clear();
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
        [ResponseType(typeof(FieldGroupViewModel))]
        [Route("filter/fieldgroups/")]
        public IHttpActionResult GetFieldGroupsUsedFields()
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

            return Ok(res.OrderBy(i => i.Name));



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


        [ResponseType(typeof(FieldGroupViewModel))]
        [Route("filter/fieldgroups/{typeslug}")]
        public IHttpActionResult GetFieldGroupsUsedFieldsType(string typeSlug)
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
        [ResponseType(typeof(FieldGroupViewModel))]
        [Route("fieldgroups/{slug}")]
        public IHttpActionResult DeleteFieldGroup(string slug)
        {
            try
            {
                var obj = _bl.GetFieldGroups(slug);
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