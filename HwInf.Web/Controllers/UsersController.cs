using System;
using System.Linq;
using System.Security;
using HwInf.BusinessLogic.Interfaces;
using HwInf.Common.Models;
using HwInf.Web.ViewModels;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HwInf.Web.Controllers
{
    [Authorize]
    [Route("api/users")]
    public class UsersController : Controller
    {
        private readonly IBusinessLogicFacade _bl;
        private readonly ILogger<UsersController> _log;


        public UsersController(IBusinessLogicFacade bl, ILogger<UsersController> log)
        {
            _bl = bl;
            _log = log;
        }

        /// <summary>
        /// Get User information
        /// </summary>
        /// <remarks>
        /// Returns a logged in &#x60;Users&#x60; information its Uid
        /// </remarks>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
        [Route("userdata")]
        [HttpGet]
        public IActionResult GetPersonByUid()
        {
            try
            {
                var p = _bl.GetUsers(_bl.GetCurrentUid());
                var vmdl = new UserViewModel(p);

                return Ok(vmdl);
            }
            catch (Exception ex)
            {
                _log.LogError("Exception: {0}", ex);
                return StatusCode(500);
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
        [Route("owners")]
        [HttpGet]
        public IActionResult GetOwners()
        {
            try
            {
                var vmdl = _bl.GetUsers()
                    .Where(i => i.Role.Name != "User")
                    .ToList()
                    .Select(i => new UserViewModel(i))
                    .OrderBy(i => i.LastName)
                    .ThenBy(i => i.Name)
                    .ToList();

                return Ok(vmdl);
            }
            catch (Exception ex)
            {
                _log.LogError("Exception: {0}", ex);
                return StatusCode(500);
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
        [Route("admins")]
        [HttpGet]
        public IActionResult GetAdmins()
        {
            try
            {

                var vmdl = _bl.GetUsers()
                    .Where(i => i.Role.Name == "Admin")
                    .ToList()
                    .Select(i => new UserViewModel(i))
                    .OrderBy(i => i.LastName)
                    .ThenBy(i => i.Name)
                    .ToList();

                return Ok(vmdl);
            }
            catch (Exception ex)
            {
                _log.LogError("Exception: {0}", ex.Message);
                return StatusCode(500);
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
        [Route("users")]
        [HttpGet]
        public IActionResult GetUsers()
        {
            try
            {

                var vmdl = _bl.GetUsers()
                    .ToList()
                    .Select(i => new UserViewModel(i))
                    .OrderBy(i => i.LastName)
                    .ThenBy(i => i.Name)
                    .ToList();

                return Ok(vmdl);
            }
            catch (Exception ex)
            {
                _log.LogError("Exception: {0}", ex);
                return StatusCode(500);
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
        [HttpPut]
        public IActionResult PostUpdateUser([FromBody] UserViewModel userVmdl)
        {
            try
            {
                Person user;
                if (_bl.IsAdmin || _bl.IsVerwalter)
                {
                    user = _bl.GetUsers(_bl.GetCurrentUid());
                }
                else
                {
                    user = _bl.GetUsers(userVmdl.Uid);
                }
                

                userVmdl.ApplyChangesTelRoom(user);
                userVmdl.Refresh(user);
                _bl.SaveChanges();
                _log.LogInformation("User '{0}' updated by '{1}'", userVmdl.Uid, User.Identity.Name);

                return Ok(userVmdl);
            }
            catch (SecurityException)
            {
                _log.LogWarning("'{0}' tried to Update user '{1}'", _bl.GetCurrentUid(), userVmdl.Uid);
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _log.LogError("Exception: {0}", ex);
                return StatusCode(500);
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
        [HttpGet]
        public IActionResult GetAddAdmin(string uid)
        {
            try
            {
                var p = _bl.GetUsers(uid);
                _bl.SetAdmin(p);
                _bl.SaveChanges();
                _log.LogInformation("User '{0}' set to admin by '{1}'", p.Uid, User.Identity.Name);

                return Ok();
            }
            catch (Exception ex)
            {
                _log.LogError("Exception: {0}", ex);
                return StatusCode(500);
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
        [HttpGet]
        public IActionResult GetRemoveAdmin(string uid, string role)
        {
            try
            {
                var p = _bl.GetUsers(uid);
                p.Role = _bl.GetRole(role);
                _bl.SaveChanges();
                _log.LogInformation("User '{0}' set to '{1}' by '{2}'", p.Uid, role, User.Identity.Name);

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