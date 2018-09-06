using System;
using System.Linq;
using HwInf.BusinessLogic.Interfaces;
using HwInf.Services;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HwInf.Web.Controllers
{

   // [RoutePrefix("print")]
    public class PrintController : Controller
    {
        private readonly IBusinessLogicFacade _bl;
        private readonly IPdfService _pdfService;
        private readonly ILogger<PrintController> _log;

        public PrintController(IBusinessLogicFacade bl, IPdfService pdfService, ILogger<PrintController> log)
        {
            _bl = bl;
            _pdfService = pdfService;
            _log = log;
        }

        /// <summary>
        /// Get contract
        /// </summary>
        /// <remarks>  
        /// Creates a contract as PDF
        /// </remarks>
        /// <param name="orderGuid">&#x60;Order&#x60; guid</param>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
        [Route("{orderGuid}")]
        [HttpGet]
        public IActionResult GetPrint(Guid orderGuid)
        {
            try
            {
                var order = _bl.GetOrders(orderGuid);
                var damages = _bl.GetDamages().Where(d => d.Date <= DateTime.Now).ToList();
                var result = _pdfService.GenerateContrtactPdf(order, damages);

                return File(result, "application/Pdf");
            }
            catch (Exception ex)
            {
                _log.LogError("Exception: '{0}'", ex);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Get return-contract
        /// </summary>
        /// <remarks>  
        /// Creates a return-contract as PDF
        /// </remarks>
        /// <param name="orderGuid">&#x60;Order&#x60; guid</param>
        /// <response code="200"></response>
        /// <response code="500">An error occured, please read log files</response>
        [Route("return/{orderGuid}")]
        [HttpGet]
        public IActionResult GetReturn(Guid orderGuid)
        {
            try
            {
                var order = _bl.GetOrders(orderGuid);
                var damages = _bl.GetDamages().Where(d => d.Date <= DateTime.Now).ToList();
                var result = _pdfService.GenerateContractReturnPdf(order, damages);

                return File(result, "application/Pdf");
            }
            catch (Exception ex)
            {
                _log.LogError("Exception: '{0}'", ex);
                return StatusCode(500);
            }
        }
    }
}