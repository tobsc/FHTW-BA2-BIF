using HwInf.Common.Models;
using HwInf.Services.PdfService;
using Microsoft.AspNetCore.Mvc;
using Controller = Microsoft.AspNetCore.Mvc.Controller;

namespace HwInf.PdfServiceTestClient.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly PDFCreator _pdfCreator;
        public ValuesController(PDFCreator pdfCreator)
        {
            _pdfCreator = pdfCreator;
        }
        // GET api/values
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public IActionResult Get()
        {
            var o = new Order
            {
                Entleiher = new Person()
                {
                    Name = "Max",
                    LastName = "Muster",
                    Uid = "if15b032",
                    Email = "if15b032@technikum-wien.at",
                    Tel = "1234568",
                    Studiengang = "BIF"
                }
            };
            var result = _pdfCreator.GenerateContrtactPdf(o, null);
            return File(result, "application/Pdf");
        }
    }
}
