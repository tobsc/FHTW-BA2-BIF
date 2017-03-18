using System;
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
    [RoutePrefix("api/customfields")]
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public class CustomFieldsController : ApiController
    {
        private readonly HwInfContext _db = new HwInfContext();
        private readonly BL _bl;

        public CustomFieldsController()
        {
            _bl = new BL(_db);
        }

        // GET: api/admin/devices/groups
        /// <summary>
        /// Returns all Field Groups
        /// </summary>
        /// <param name="input">String</param>
        /// <param name="entity">Entity Name (deviceType, fieldGroup)</param>
        /// <returns></returns>
        [Route("slug")]
        public IHttpActionResult GetSlug(string input, string entity)
        {
            return Ok(SlugGenerator.GenerateSlug(input, entity));
        }

        // GET: api/admin/devices/groups
        /// <summary>
        /// Returns all Field Groups
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(IQueryable<FieldGroupViewModel>))]
        [Route("fieldgroups")]
        public IHttpActionResult GetGroups()
        {

            var vmdl = _bl.GetFieldGroups()
                .ToList()
                .Select(i => new FieldGroupViewModel(i));

            return Ok(vmdl);
        }


        // POST: api/admin/devices/types
        /// <summary>
        /// Return FieldGroups of a DeviceType
        /// </summary>
        /// <returns>List of FieldGroups</returns>
        [ResponseType(typeof(FieldGroupViewModel))]
        [Route("fieldgroups/{typeSlug}")]
        public IHttpActionResult GetGroupsOfDeviceType(string typeSlug)
        {
            var dt = _bl.GetDeviceType(typeSlug);

            var vmdl = _bl.GetFieldGroups()
                .ToList()
                .Where(i => i.DeviceTypes.Contains(dt))
                .Select(i => new FieldGroupViewModel(i));

            return Ok(vmdl);
        }

        // POST: api/admin/devices/groups
        /// <summary>
        /// Add new FieldGroup
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(DeviceViewModel))]
        [Route("fieldgroups")]
        public IHttpActionResult PostGroup(FieldGroupViewModel vmdl)
        {
            var obj = _bl.CreateFieldGroup();
            vmdl.ApplyChanges(obj, _bl);
            _bl.SaveChanges();
            return Ok(vmdl);
        }

        // POST: api/admin/devices/groups/fields
        /// <summary>
        /// Add new Field to FieldGroup
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(DeviceViewModel))]
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

            return Ok(vmdl);
        }

        // POST: api/admin/devices/groups/types
        /// <summary>
        /// Add DeviceType to FieldGroup
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(DeviceViewModel))]
        [Route("devices/groups/types")]
        public IHttpActionResult PostGroupType(string typeSlug, string groupSlug)
        {

            var fg = _bl.GetFieldGroups(groupSlug);
            var dt = _bl.GetDeviceType(typeSlug);

            _bl.UpdateFieldGroup(fg);
            fg.DeviceTypes.Add(dt);
            _bl.SaveChanges();

            return Ok();
        }

        // POST: api/admin/devices/types
        /// <summary>
        /// Create New DeviceType
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(DeviceViewModel))]
        [Route("device/types")]
        public IHttpActionResult PostDeviceType(DeviceTypeViewModel vmdl)
        {

            var dt = _bl.CreateDeviceType();

            vmdl.ApplyChanges(dt, _bl);
            _bl.SaveChanges();

            vmdl.Refresh(dt);
            return Ok(vmdl);
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