using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using HwInf.Common.DAL;
using HwInf.ViewModels;
using HwInf.Common;
using System.IO;
using MigraDoc.DocumentObjectModel.IO;
using MigraDoc.Rendering;
using System.Net.Http.Headers;
using HwInf.Common.BL;
using log4net;


namespace HwInf.Controllers
{

    [RoutePrefix("api/print")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class PrintController : ApiController
    {
        private readonly IDAL _db;
        private readonly BL _bl;
        private readonly ILog _log = LogManager.GetLogger(typeof(PrintController).Name);

        public PrintController()
        {
            _db = new HwInfContext();
            _bl = new BL(_db);
        }

        public PrintController(IDAL db)
        {
            _db = db;
            _bl = new BL(db);
        }

        /// <summary>
        /// Starts the RunTime Text Template and creates the contract as pdf
        /// </summary>
        /// <param name="guid">Order guid</param>
        /// <returns></returns>
        [Route("{guid}")]
        public HttpResponseMessage GetPrint(Guid guid)
        {

            var order = _bl.GetOrders(guid);


            var rpt = new Contract(order.OrderId, order.Entleiher.Uid);
            // Report -> String
            var text = rpt.TransformText();


            // Stream für den DdlReader erzeugen
            MemoryStream stream = CreateMDDLStream(text);
            var errors = new DdlReaderErrors();
            DdlReader rd = new DdlReader(stream, errors);

            // MDDL einlesen
            var doc = rd.ReadDocument();

            // MigraDoc Dokument in ein PDF Rendern
            PdfDocumentRenderer pdf = new PdfDocumentRenderer(true, PdfSharp.Pdf.PdfFontEmbedding.None);
            pdf.Document = doc;
            pdf.RenderDocument();
            // Speichern
            pdf.Save(AppDomain.CurrentDomain.BaseDirectory + "\\Ausleihvertrag.pdf");


            HttpResponseMessage result;
            var localFilePath = AppDomain.CurrentDomain.BaseDirectory + "\\Ausleihvertrag.pdf";

            if (!File.Exists(localFilePath))
            {
                result = Request.CreateResponse(HttpStatusCode.Gone);
            }
            else
            {
                // Serve the file to the client
                result = Request.CreateResponse(HttpStatusCode.OK);
                //result.Content = new StreamContent(new FileStream(localFilePath, FileMode.Open, FileAccess.Read));
                Byte[] bytes = File.ReadAllBytes(localFilePath);
                result.Content = new ByteArrayContent(bytes);
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                result.Content.Headers.ContentDisposition.FileName = "Ausleih_Vertrag.pdf";
            }


            return result;

        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }


        private static MemoryStream CreateMDDLStream(string text)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter sw = new StreamWriter(stream);

            sw.Write(text);

            sw.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
    }
}