using HwInf.Common.BL;
using HwInf.Common.DAL;
using HwInf.ViewModels;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Web.Http;
using System.Web.Http.Description;

namespace HwInf.Controllers
{
    [Authorize]
    [RoutePrefix("api/damages")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class DamagesController : ApiController
    {
        private readonly IDAL _db;
        private readonly BL _bl;
        private readonly ILog _log = LogManager.GetLogger(typeof(DamagesController).Name);

        public DamagesController()
        {
            _db = new HwInfContext();
            _bl = new BL(_db);
        }

        public DamagesController(IDAL db)
        {
            _db = db;
            _bl = new BL(db);
        }

        /// <summary>
        /// Get Damages
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(DamageViewModel))]
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
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// <param name="invNum"></param>
        /// <returns></returns>
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

                if (damages == null)
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
        /// <returns></returns>
        [ResponseType(typeof(DamageStatusViewModel))]
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
        /// <param name="vmdl"></param>
        /// <returns></returns>
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
        /// Update a Damage
        /// </summary>
        /// <param name="id">Damage Id</param>
        /// <param name="vmdl">DamageViewModel</param>
        /// <returns></returns>
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
