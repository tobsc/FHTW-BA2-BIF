using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using HwInf.BusinessLogic.Interfaces;
using HwInf.Web.ViewModels;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HwInf.Web.Controllers
{
    [Authorize]
    [Route("api/settings")]
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public class SettingsController : Controller
    {
        private readonly IBusinessLogicFacade _bl;
        private readonly ILog _log = LogManager.GetLogger(typeof(SettingsController));


        public SettingsController(IBusinessLogicFacade bl)
        {
            _bl = bl;
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
        [Route("{key}")]
        [HttpGet]
        public IActionResult GetSetting(string key)
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
                return StatusCode(500);
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
        [Route("")]
        [HttpGet]
        public IActionResult GetSetting()
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
                return StatusCode(500);
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
        [Route("")]
        [HttpPost]
        public IActionResult PostSetting([FromBody] SettingViewModel settingVmdl)
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
                return StatusCode(500);
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
        [Route("")]
        [HttpPut]
        public IActionResult PutSetting([FromBody] SettingViewModel settingVmdl)
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
                return StatusCode(500);
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
        [Route("multiple")]
        [HttpPut]
        public IActionResult PutSettings([FromBody] List<SettingViewModel> settingVmdlList)
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
                return StatusCode(500);
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
        [HttpDelete]
        public IActionResult DeleteSetting(string settingKey)
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
                return StatusCode(500);
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
        [HttpGet]
        public IActionResult GetLog()
        {
            try
            {
                return Ok(_bl.GetLog());
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Exception: '{0}'", ex);
                return StatusCode(500);
            }
        }
    }
}