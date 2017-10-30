using HwInf.ViewModels;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security;
using System.Web.Http;
using System.Web.Http.Description;
using HwInf.Common.Interfaces;

namespace HwInf.Controllers
{
    [Authorize]
    [RoutePrefix("api/damages")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class DamagesController : ApiController
    {
        private readonly IBusinessLayer _bl;
        private readonly ILog _log = LogManager.GetLogger(typeof(DamagesController).Name);



        public DamagesController(IBusinessLayer bl)
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
        [ResponseType(typeof(List<DamageViewModel>))]
        [Route("")]
        public IHttpActionResult GetDamages()
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
                return InternalServerError();
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
        [ResponseType(typeof(DamageViewModel))]
        [Route("id/{id}")]
        public IHttpActionResult GetDamage(int id)
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
                return InternalServerError();
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
        [ResponseType(typeof(DamageViewModel))]
        [Route("invnum")]
        public IHttpActionResult GetDamages(string invNum)
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
                return InternalServerError();
            }
        }

        /// <summary>
        /// Get DamageStatus
        /// </summary>
        /// <remarks>Returns a list of &#x60;DamageStatus&#x60;</remarks>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
        [ResponseType(typeof(List<DamageStatusViewModel>))]
        [Route("damagestatus")]
        public IHttpActionResult GetDamageStatus()
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
                return InternalServerError();
            }
        }

        /// <summary>
        /// Create Damage
        /// </summary>
        /// <remarks>Creates a new &#x60;Damage&#x60;. &#x60;Damages&#x60; are attached to &#x60;Devices&#x60; an show their current condition</remarks>
        /// <param name="vmdl">Damage as &#x60;DamageViewModel&#x60;</param>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
        [ResponseType(typeof(DamageViewModel))]
        [Route("")]
        public IHttpActionResult PostDamage([FromBody]DamageViewModel vmdl)
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
                return InternalServerError();
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
        [ResponseType(typeof(DamageViewModel))]
        [Route("id/{id}")]
        public IHttpActionResult PutDamage(int id, DamageViewModel vmdl)
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
                return InternalServerError();
            }



            return Ok(vmdl);
        }
    }
}
