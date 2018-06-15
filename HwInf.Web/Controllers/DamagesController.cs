using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security;
using HwInf.BusinessLogic.Interfaces;
using HwInf.Web.ViewModels;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HwInf.Web.Controllers
{
    [Authorize]
    [Route("api/damages")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class DamagesController : Controller
    {
        private readonly IBusinessLogicFacade _bl;
        private readonly ILog _log = LogManager.GetLogger(typeof(DamagesController));
        
        public DamagesController(IBusinessLogicFacade bl)
        {
            _bl = bl;
        }

        /// <summary>
        /// Get Damages
        /// </summary>
        /// <remarks>
        /// Returns all &#x60;Damages&#x60; which are not repaired yet.
        /// </remarks>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
        [Route("")]
        [HttpGet]
        public IActionResult GetDamages()
        {
            try
            {
                var damages = _bl.GetDamages()
                         .ToList()
                         .Select(i => new DamageViewModel(i))
                         .Where(i=> i.DamageStatus.Slug != "behoben")
                         .ToList();
                if (_bl.IsVerwalter)
                {
                    damages = damages.Where(i => i.Reporter.Uid == _bl.GetCurrentUid()).ToList();
                }

                return Ok(damages);

            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: {0}", ex);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Get Single Damage
        /// </summary>
        /// <remarks>Returns a single &#x60;Damage&#x60; by ID</remarks>
        /// <param name="id">Unique ID of a &#x60;Damage&#x60;</param>
        /// <response code="200"></response>
        /// <response code="404">An error occured, &#x60;Damage&#x60; not found</response>
        /// <response code="500">An error occured, please read log files</response>
        [Route("id/{id}")]
        [HttpGet]
        public IActionResult GetDamage(int id)
        {
            try
            {
                var damage = _bl.GetDamage(id);

                if (damage == null)
                {
                    _log.WarnFormat("Not Found: Order '{0}' not found", id);
                    return NotFound();
                }

                var vmdl = new DamageViewModel(damage);

                return Ok(vmdl);
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: {0}", ex);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Get Damages by InvNum of Device
        /// </summary>
        /// <remarks>
        /// Returns &#x60;Damages&#x60; attached to a single &#x60;Device&#x60;.
        /// </remarks>
        /// <param name="invNum">Unique identifier of a &#x60;Device&#x60;</param>
        /// <response code="200"></response>
        /// <response code="404">An error occured, &#x60;Damage&#x60; not found</response>
        /// <response code="500">An error occured, please read log files</response>
        [Route("invnum")]
        [HttpGet]
        public IActionResult GetDamages(string invNum)
        {
            try
            {
                invNum = invNum.Replace(" ", "+");
                var damages = _bl.GetDamages(invNum)
                         .ToList()
                         .Select(i => new DamageViewModel(i))
                         .Where(i=> i.DamageStatus.Slug != "behoben")
                         .ToList();
                if (_bl.IsVerwalter)
                {
                    damages = damages.Where(i => i.Reporter.Uid == _bl.GetCurrentUid()).ToList();
                }

                if (!damages.Any())
                {
                    _log.WarnFormat("Not Found: Damage by Device '{0}' not found", invNum);
                    return NotFound();
                }


                return Ok(damages.OrderByDescending(i=>i.DamageId));
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: {0}", ex);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Get Damages by DeviceId of Device
        /// </summary>
        /// <remarks>
        /// Returns &#x60;Damages&#x60; attached to a single &#x60;Device&#x60;.
        /// </remarks>
        /// <param name="deviceid">Unique identifier of a &#x60;Device&#x60;</param>
        /// <response code="200"></response>
        /// <response code="404">An error occured, &#x60;Damage&#x60; not found</response>
        /// <response code="500">An error occured, please read log files</response>
        [Route("deviceid")]
        [HttpGet]
        public IActionResult GetDamages(int deviceid)
        {
            try
            {
                
                var damages = _bl.GetDamages(deviceid)
                         .ToList()
                         .Select(i => new DamageViewModel(i))
                         .Where(i => i.DamageStatus.Slug != "behoben")
                         .ToList();
                if (_bl.IsVerwalter)
                {
                    damages = damages.Where(i => i.Reporter.Uid == _bl.GetCurrentUid()).ToList();
                }

                //2017-12-30 commented; if there are no damages then there should be no 404 error?
                //if (!damages.Any())
                //{
                //    _log.WarnFormat("Not Found: Damage by Device '{0}' not found", invNum);
                //    return NotFound();
                //}



                return Ok(damages.OrderByDescending(i => i.DamageId));
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: {0}", ex);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Get DamageStatus
        /// </summary>
        /// <remarks>Returns a list of &#x60;DamageStatus&#x60;</remarks>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
        [Route("damagestatus")]
        [HttpGet]
        public IActionResult GetDamageStatus()
        {
            try
            {
                var vmdl = _bl.GetDamageStatus()
                    .ToList();

                return Ok(vmdl);
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: {0}", ex);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Create Damage
        /// </summary>
        /// <remarks>Creates a new &#x60;Damage&#x60;. &#x60;Damages&#x60; are attached to &#x60;Devices&#x60; an show their current condition</remarks>
        /// <param name="vmdl">Damage as &#x60;DamageViewModel&#x60;</param>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
        [Route("")]
        [HttpPost]
        public IActionResult PostDamage([FromBody]DamageViewModel vmdl)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var damage = _bl.CreateDamage();
                vmdl.ApplyChanges(damage, _bl);

                
                _bl.SaveChanges();
               
                    _log.InfoFormat("Damage '{0}' created by '{1}'", vmdl.DamageId, User.Identity.Name);

                vmdl.Refresh(damage);
                return Ok(vmdl);

            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: {0}", ex);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Update Damage
        /// </summary>
        /// <remarks>Updates a &#x60;Damage&#x60;</remarks>
        /// <param name="id">Unique identifier of a &#x60;Damage&#x60;</param>
        /// <param name="vmdl">Updated Damage as &#x60;DamageViewModel&#x60;</param>
        /// <response code="200"></response>
        /// <response code="400">An error occured, id and vmdl.DamageId must be equal</response>
        /// <response code="500">An error occured, please read log files</response>
        [Route("id/{id}")]
        [HttpPut]
        public IActionResult PutDamage(int id, [FromBody]DamageViewModel vmdl)
        {
            if (id != vmdl.DamageId)
            {
                return BadRequest();
            }

            try
            {

                var damage = _bl.GetDamage(vmdl.DamageId);
                _bl.UpdateDamage(damage);
                vmdl.Update(damage, _bl);
                _bl.SaveChanges();

                _log.InfoFormat("Damage '{0}({1})' updated by '{2}'", vmdl.DamageId, vmdl.DamageStatus, User.Identity.Name);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_bl.DamageExists(id))
                {
                    _log.WarnFormat("Not Found: Damage '{0}' not found", id);
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (SecurityException)
            {
                _log.ErrorFormat("Security: '{0}' tried to update Device '{1}'", vmdl.DamageId);
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: {0}", ex);
                return StatusCode(500);
            }



            return Ok(vmdl);
        }
    }
}
