using System;
using System.Linq;
using System.Security;
using System.Web.Http;
using System.Web.Http.Description;
using HwInf.Common.Interfaces;
using HwInf.ViewModels;
using log4net;

namespace HwInf.Controllers
{
    [Authorize]
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private readonly IBusinessLayer _bl;
        private readonly ILog _log = LogManager.GetLogger(typeof(UsersController).Name);


        public UsersController(IBusinessLayer bl)
        {
            _bl = bl;
        }

        /// <summary>
        /// Get User information
        /// </summary>
        /// <remarks>
        /// Returns a logged in &#x60;Users&#x60; information its Uid
        /// </remarks>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
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
                _log.ErrorFormat("Exception: {0}", ex);
                return InternalServerError();
            }

        }

        /// <summary>
        /// Get Owners and Admins
        /// </summary>
        /// <remarks>
        /// Returns a List of all &#x60;Users&#x60; which are manage &#x60;Devices&#x60;
        /// </remarks>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
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
                _log.ErrorFormat("Exception: {0}", ex);
                return InternalServerError();
            }

        }

        /// <summary>
        /// Get Admins
        /// </summary>
        /// <remarks>
        /// Returns a List of all &#x60;Users&#x60; which are Admins;
        /// </remarks>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
        [Authorize(Roles = "Verwalter, Admin")]
        [ResponseType(typeof(UserViewModel))]
        [Route("admins")]
        public IHttpActionResult GetAdmins()
        {
            try
            {

                var vmdl = _bl.GetUsers()
                    .Where(i => i.Role.Name == "Admin")
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
        /// Get Users
        /// </summary>
        /// <remarks>
        /// Returns a List of all &#x60;Users&#x60;
        /// </remarks>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
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
                _log.ErrorFormat("Exception: {0}", ex);
                return InternalServerError();
            }

        }

        /// <summary>
        /// Update User
        /// </summary>
        /// <remarks>
        /// Updates a &#x60;User's&#x60; Phonenumber and/or assigned Room
        /// </remarks>
        /// <param name="userVmdl">&#x60;UserViewModel&#x60; of User</param>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
        [Route("update")]
        public IHttpActionResult PostUpdateUser([FromBody] UserViewModel userVmdl)
        {
            try
            {
                var obj = _bl.GetUsers(_bl.GetCurrentUid());

                userVmdl.ApplyChangesTelRoom(obj);
                userVmdl.Refresh(obj);
                _bl.SaveChanges();
                _log.InfoFormat("User '{0}' updated by '{1}'", userVmdl.Uid, User.Identity.Name);

                return Ok(userVmdl);
            }
            catch (SecurityException)
            {
                _log.WarnFormat("'{0}' tried to Update user '{1}'", _bl.GetCurrentUid(), userVmdl.Uid);
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: {0}", ex);
                return InternalServerError();
            }
        }

        /// <summary>
        /// Add Admin
        /// </summary>
        /// <remarks>
        /// Adds a &#x60;User&#x60; to the List of Admins
        /// </remarks>
        /// <param name="uid">Uid of &#x60;User&#x60;</param>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
        [Route("admin/{uid}")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult GetAddAdmin(string uid)
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
                _log.ErrorFormat("Exception: {0}", ex);
                return InternalServerError();
            }
        }

        /// <summary>
        /// Remove Admin
        /// </summary>
        /// <remarks>
        /// Removes a &#x60;User&#x60; from the List of Admins and sets his new role
        /// </remarks>
        /// <param name="uid">Uid of &#x60;User&#x60;</param>
        /// <param name="role">Role the &#x60;User&#x60; will be assigned to</param>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
        [Route("admin/remove/{uid}/{role}")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult GetRemoveAdmin(string uid, string role)
        {
            try
            {
                var p = _bl.GetUsers(uid);
                p.Role = _bl.GetRole(role);
                _bl.SaveChanges();
                _log.InfoFormat("User '{0}' set to '{1}' by '{2}'", p.Uid, role, User.Identity.Name);

                return Ok();
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: {0}", ex);
                return InternalServerError();
            }
        }
    }
}