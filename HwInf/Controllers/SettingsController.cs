using System.Diagnostics.CodeAnalysis;
using System.Runtime.Remoting;
using System.Web.Http;
using System.Web.Http.Description;
using HwInf.Common.BL;
using HwInf.Common.DAL;
using HwInf.ViewModels;
using log4net;

namespace HwInf.Controllers
{
    [Authorize(Roles = "Admin, Verwalter")]
    [RoutePrefix("api/settings")]
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public class SettingsController : ApiController
    {
        private readonly IDAL _dal;
        private readonly BL _bl;
        private readonly ILog _log = LogManager.GetLogger(typeof(SettingsController));


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
                    return NotFound();
                }
                var vmdl = new SettingViewModel(setting);

                return Ok(vmdl);
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

            return Ok(vmdl);
        }

        [ResponseType(typeof(SettingViewModel))]
        [Route("")]
        public IHttpActionResult PutSetting([FromBody] SettingViewModel vmdl)
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

            _bl.SaveChanges();

            return Ok(vmdl);
        }


        [Route("{key}")]
        public IHttpActionResult DeleteSetting(string key)
        {
            var setting = _bl.GetSetting(key);

            if (setting == null)
            {
                return NotFound();
            }
            
            _bl.DeleteSetting(setting);
            _bl.SaveChanges();
            return Ok();
        }
    }
}