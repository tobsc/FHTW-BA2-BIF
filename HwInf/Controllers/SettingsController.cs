using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Remoting;
using System.Web.Http;
using System.Web.Http.Description;
using HwInf.Common.BL;
using HwInf.Common.DAL;
using HwInf.ViewModels;
using log4net;
using System.Collections.Generic;
using System.Linq;

namespace HwInf.Controllers
{
    [Authorize]
    [RoutePrefix("api/settings")]
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public class SettingsController : ApiController
    {
        private readonly IDAL _dal;
        private readonly BL _bl;
        private readonly ILog _log = LogManager.GetLogger(typeof(SettingsController).Name);


        public SettingsController()
        {
            _dal = new HwInfContext();
            _bl = new BL(_dal);
        }

        public SettingsController(IDAL dal)
        {
            _dal = dal;
            _bl = new BL(_dal);
        }

        [ResponseType(typeof(SettingViewModel))]
        [Route("{key}")]
        public IHttpActionResult GetSetting(string key)
        {
            try
            {
                var setting = _bl.GetSetting(key);

                if (setting == null)
                {
                    _log.WarnFormat("Not Found: Setting '{0}' not found", key);
                    return NotFound();
                }
                var vmdl = new SettingViewModel(setting);

                return Ok(vmdl);
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: '{0}'", ex.Message);
                return InternalServerError();
            }
        }

        [ResponseType(typeof(List<SettingViewModel>))]
        [Route("")]
        public IHttpActionResult GetSetting()
        {
            try
            {
                var settings = _bl.GetSettings();

                if (settings == null)
                {
                    return NotFound();
                }
                var result = new List<SettingViewModel>();
                settings.ToList().Select(i => new SettingViewModel(i)).ToList().ForEach(i =>
                 {
                     result.Add(i);
                 });
               
                return Ok(result);
            }
            catch
            {
                return InternalServerError();
            }
        }

        [ResponseType(typeof(SettingViewModel))]
        [Route("")]
        public IHttpActionResult PostSetting([FromBody] SettingViewModel vmdl)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (string.IsNullOrWhiteSpace(vmdl.Key))
                {
                    return BadRequest("Bitte einen Key für die Einstellung angeben.");
                }

                if (string.IsNullOrWhiteSpace(vmdl.Value))
                {
                    return BadRequest("Bitte einen Value für die Einstellung angeben.");
                }

                if (_bl.GetSetting(vmdl.Key) != null)
                {
                    return BadRequest($"Die Einstellung mit dem key '{vmdl.Key}' existiert bereits.");
                }

                var obj = _bl.CreateSetting();

                vmdl.ApplyChanges(obj, _bl);

                _bl.SaveChanges();

                _log.InfoFormat("New Setting '{0}' created by '{1}'", vmdl.Key, User.Identity.Name);

                return Ok(vmdl);
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: '{0}'", ex.Message);
                return InternalServerError();
            }
        }

        [ResponseType(typeof(SettingViewModel))]
        [Route("")]
        public IHttpActionResult PutSetting([FromBody] SettingViewModel vmdl)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (string.IsNullOrWhiteSpace(vmdl.Key))
                {
                    return BadRequest("Bitte einen Key für die Einstellung angeben.");
                }

                if (string.IsNullOrWhiteSpace(vmdl.Value))
                {
                    return BadRequest("Bitte einen Value für die Einstellung angeben.");
                }

                var obj = _bl.GetSetting(vmdl.Key);

                if (obj == null)
                {
                    _log.WarnFormat("Not Found: Setting '{0}' not found", vmdl.Key);
                    return NotFound();
                }

                _bl.UpdateSetting(obj);

                vmdl.ApplyChanges(obj, _bl);

                _bl.SaveChanges();

                _log.InfoFormat("Setting '{0}' updated by '{1}'", vmdl.Key, User.Identity.Name);

                return Ok(vmdl);
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: '{0}'", ex.Message);
                return InternalServerError();
            }
        }

        [ResponseType(typeof(List<SettingViewModel>))]
        [Route("multiple")]
        public IHttpActionResult PutSettings([FromBody] List<SettingViewModel> vmdllist)
        {
            try
            {
                foreach (SettingViewModel vmdl in vmdllist)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    if (string.IsNullOrWhiteSpace(vmdl.Key))
                    {
                        return BadRequest("Bitte einen Key für die Einstellung angeben.");
                    }

                    if (string.IsNullOrWhiteSpace(vmdl.Value))
                    {
                        return BadRequest("Bitte einen Value für die Einstellung angeben.");
                    }

                    var obj = _bl.GetSetting(vmdl.Key);

                    if (obj == null) return NotFound();

                    _bl.UpdateSetting(obj);

                    vmdl.ApplyChanges(obj, _bl);

                }
                _bl.SaveChanges();

                _log.InfoFormat("Setting '{0}' updated by '{1}'", vmdllist.ToString(), User.Identity.Name);

                return Ok(vmdllist);
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: '{0}'", ex.Message);
                return InternalServerError();
            }
        }


        [Route("{key}")]
        public IHttpActionResult DeleteSetting(string key)
        {
            try
            {
                var setting = _bl.GetSetting(key);

                if (setting == null)
                {
                    _log.WarnFormat("Not Found: Setting '{0}' not found", key);
                    return NotFound();
                }

                _bl.DeleteSetting(setting);
                _bl.SaveChanges();
                _log.InfoFormat("Setting '{0}' deleted by '{1}'", setting.Key, User.Identity.Name);
                return Ok();
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: '{0}'", ex.Message);
                return InternalServerError();
            }
        }

        /// <summary>
        /// Return Log 
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [Route("log")]
        public IHttpActionResult GetLog()
        {
            try
            {
                return Ok(_bl.GetLog());
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: '{0}'", ex.Message);
                return InternalServerError();
            }
        }
    }
}