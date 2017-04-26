using System;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Web.Http;
using System.Web.Http.Description;
using HwInf.Common.BL;
using HwInf.Common.DAL;
using HwInf.ViewModels;
using log4net;

namespace HwInf.Controllers
{
    [Authorize]
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private readonly IDAL _db;
        private readonly BL _bl;
        private readonly ILog _log = LogManager.GetLogger(typeof(UsersController).Name);

        public UsersController()
        {
            _db = new HwInfContext();
            _bl = new BL(_db);
        }

        public UsersController(IDAL db)
        {
            _db = db;
            _bl = new BL(db);
        }

        /// <summary>
        /// Returns UserViewModel of given Uid.
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(UserViewModel))]
        [Route("userdata")]
        public IHttpActionResult GetPersonByUid()
        {
            try
            {
                var p = _bl.GetUsers(_bl.GetCurrentUid());
                var vmdl = new UserViewModel(p);

                return Ok(vmdl);
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: {0}", ex.Message);
                return InternalServerError();
            }

        }

        /// <summary>
        /// Returns List of Users.
        /// </summary>
        /// <returns>LastName, Name, Uid</returns>
        [Authorize(Roles="Verwalter, Admin")]
        [ResponseType(typeof(UserViewModel))]
        [Route("owners")]
        public IHttpActionResult GetOwners()
        {
            try
            {
                var vmdl = _bl.GetUsers()
                    .Where(i => i.Role.Name != "User")
                    .ToList()
                    .Select(i => new UserViewModel(i))
                    .ToList();

                return Ok(vmdl);
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: {0}", ex.Message);
                return InternalServerError();
            }

        }

        /// <summary>
        /// Returns List of Users.
        /// </summary>
        /// <returns>LastName, Name, Uid</returns>
        [Authorize(Roles = "Verwalter, Admin")]
        [ResponseType(typeof(UserViewModel))]
        [Route("users")]
        public IHttpActionResult GetUsers()
        {
            try
            {

                var vmdl = _bl.GetUsers()
                    .ToList()
                    .Select(i => new UserViewModel(i))
                    .ToList();

                return Ok(vmdl);
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: {0}", ex.Message);
                return InternalServerError();
            }

        }

        /// <summary>
        /// Save Phonenumber
        /// </summary>
        /// <param name="vmdl"></param>
        /// <returns></returns>
        [Route("update")]
        public IHttpActionResult PostUpdateUser([FromBody] UserViewModel vmdl)
        {
            try
            {
                var obj = _bl.GetUsers(_bl.GetCurrentUid());

                vmdl.ApplyChangesTelRoom(obj);
                vmdl.Refresh(obj);
                _bl.SaveChanges();
                _log.InfoFormat("User '{0}' updated by '{1}'", vmdl.Uid, User.Identity.Name);

                return Ok(vmdl);
            }
            catch (SecurityException)
            {
                _log.ErrorFormat("'{0}' tried to Update user '{1}'", _bl.GetCurrentUid(), vmdl.Uid);
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: {0}", ex.Message);
                return InternalServerError();
            }
        }

        /// <summary>
        /// Add Admin
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [Route("admin")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult PutAddAdmin(string uid)
        {
            try
            {
                var p = _bl.GetUsers(uid);
                _bl.SetAdmin(p);
                _bl.SaveChanges();
                _log.InfoFormat("User '{0}' set to admin by '{1}'", p.Uid, User.Identity.Name);

                return Ok();
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: {0}", ex.Message);
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