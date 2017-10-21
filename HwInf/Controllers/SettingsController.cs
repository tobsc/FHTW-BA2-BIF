using System;
using System.Diagnostics.CodeAnalysis;
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
        private readonly BL _bl;
        private readonly ILog _log = LogManager.GetLogger(typeof(SettingsController).Name);


        public SettingsController()
        {
            IDAL dal = new HwInfContext();
            _bl = new BL(dal);
        }

        public SettingsController(IDAL dal)
        {
            _bl = new BL(dal);
        }


        /// <summary>
        /// Get single Setting
        /// </summary>
        /// <remarks>
        /// Returns a single &#x60;Setting&#x60; by its key. 
        /// </remarks>
        /// <param name="key">Key of the &#x60;Setting&#x60;</param>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
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
                _log.ErrorFormat("Exception: '{0}'", ex);
                return InternalServerError();
            }
        }

        /// <summary>
        /// Get Settings
        /// </summary>
        /// <remarks>
        /// Returns a List of all &#x60;Settings&#x60; 
        /// </remarks>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
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


        /// <summary>
        /// Create single Setting
        /// </summary>
        /// <remarks>
        /// Create a single &#x60;Setting&#x60; 
        /// </remarks>
        /// <param name="settingVmdl"> &#x60;SettingViewModel&#x60; of the &#x60;Setting&#x60;</param>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
        [ResponseType(typeof(SettingViewModel))]
        [Route("")]
        public IHttpActionResult PostSetting([FromBody] SettingViewModel settingVmdl)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (string.IsNullOrWhiteSpace(settingVmdl.Key))
                {
                    return BadRequest("Bitte einen Key für die Einstellung angeben.");
                }

                if (string.IsNullOrWhiteSpace(settingVmdl.Value))
                {
                    return BadRequest("Bitte einen Value für die Einstellung angeben.");
                }

                if (_bl.GetSetting(settingVmdl.Key) != null)
                {
                    return BadRequest($"Die Einstellung mit dem key '{settingVmdl.Key}' existiert bereits.");
                }

                var obj = _bl.CreateSetting();

                settingVmdl.ApplyChanges(obj, _bl);

                _bl.SaveChanges();

                _log.InfoFormat("New Setting '{0}' created by '{1}'", settingVmdl.Key, User.Identity.Name);

                return Ok(settingVmdl);
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: '{0}'", ex);
                return InternalServerError();
            }
        }

        /// <summary>
        /// Update single Setting
        /// </summary>
        /// <remarks>
        /// Update a single &#x60;Setting&#x60; 
        /// </remarks>
        /// <param name="settingVmdl"> &#x60;SettingViewModel&#x60; of the &#x60;Setting&#x60;</param>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
        [ResponseType(typeof(SettingViewModel))]
        [Route("")]
        public IHttpActionResult PutSetting([FromBody] SettingViewModel settingVmdl)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (string.IsNullOrWhiteSpace(settingVmdl.Key))
                {
                    return BadRequest("Bitte einen Key für die Einstellung angeben.");
                }

                if (string.IsNullOrWhiteSpace(settingVmdl.Value))
                {
                    return BadRequest("Bitte einen Value für die Einstellung angeben.");
                }

                var obj = _bl.GetSetting(settingVmdl.Key);

                if (obj == null)
                {
                    _log.WarnFormat("Not Found: Setting '{0}' not found", settingVmdl.Key);
                    return NotFound();
                }

                _bl.UpdateSetting(obj);

                settingVmdl.ApplyChanges(obj, _bl);

                _bl.SaveChanges();

                _log.InfoFormat("Setting '{0}' updated by '{1}'", settingVmdl.Key, User.Identity.Name);

                return Ok(settingVmdl);
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: '{0}'", ex);
                return InternalServerError();
            }
        }


        /// <summary>
        /// Update multiple Settings
        /// </summary>
        /// <remarks>
        /// Update multiple &#x60;Settings&#x60; at once 
        /// </remarks>
        /// <param name="settingVmdlList">List of &#x60;SettingViewModels&#x60;</param>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
        [ResponseType(typeof(List<SettingViewModel>))]
        [Route("multiple")]
        public IHttpActionResult PutSettings([FromBody] List<SettingViewModel> settingVmdlList)
        {
            try
            {
                foreach (SettingViewModel vmdl in settingVmdlList)
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
                        _log.InfoFormat("Setting '{0}' not found", vmdl.Key);
                        return NotFound();
                    }

                    _bl.UpdateSetting(obj);

                    vmdl.ApplyChanges(obj, _bl);

                }
                _bl.SaveChanges();

                _log.InfoFormat("Setting '{0}' updated by '{1}'", settingVmdlList, User.Identity.Name);

                return Ok(settingVmdlList);
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: '{0}'", ex);
                return InternalServerError();
            }
        }

        /// <summary>
        /// Delete single Setting
        /// </summary>
        /// <remarks>
        /// Delete a single &#x60;Setting&#x60; 
        /// </remarks>
        /// <param name="settingKey">Key of the &#x60;Setting&#x60;</param>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
        [Route("{key}")]
        public IHttpActionResult DeleteSetting(string settingKey)
        {
            try
            {
                var setting = _bl.GetSetting(settingKey);

                if (setting == null)
                {
                    _log.WarnFormat("Not Found: Setting '{0}' not found", settingKey);
                    return NotFound();
                }

                _bl.DeleteSetting(setting);
                _bl.SaveChanges();
                _log.InfoFormat("Setting '{0}' deleted by '{1}'", setting.Key, User.Identity.Name);
                return Ok();
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: '{0}'", ex);
                return InternalServerError();
            }
        }


        /// <summary>
        /// Get Log
        /// </summary>
        /// <remarks>
        /// Returns the current LogFile as a string array
        /// </remarks>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
        [Authorize(Roles = "Admin, Verwalter")]
        [Route("logs")]
        public IHttpActionResult GetLog()
        {
            try
            {
                return Ok(_bl.GetLog());
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: '{0}'", ex);
                return InternalServerError();
            }
        }
    }
}