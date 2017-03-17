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
    [RoutePrefix("api/admin")]
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public class AdminController : ApiController
    {
        private readonly HwInfContext _db = new HwInfContext();
        private readonly BL _bl;

        public AdminController()
        {
            _bl = new BL(_db);
        }

        // GET: api/admin/devices/groups
        /// <summary>
        /// Returns all Field Groups
        /// <param name="input"></param>
        /// </summary>
        /// <returns></returns>
        [Route("devices/slug")]
        public IHttpActionResult GetSlug(string input)
        {
            return Ok(SlugGenerator.GenerateSlug(input));
        }

        // GET: api/admin/devices/groups
        /// <summary>
        /// Returns all Field Groups
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(IQueryable<FieldGroupViewModel>))]
        [Route("devices/groups")]
        public IHttpActionResult GetGroups()
        {

            var vmdl = _bl.GetFieldGroups()
                .ToList()
                .Select(i => new FieldGroupViewModel(i));

            return Ok(vmdl);
        }

        // POST: api/admin/devices/groups
        /// <summary>
        /// Add new Field Group
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(DeviceViewModel))]
        [Route("devices/groups")]
        public IHttpActionResult PostGroup(FieldGroupViewModel vmdl)
        {


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