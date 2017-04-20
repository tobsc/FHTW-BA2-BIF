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
    [Authorize]
    [RoutePrefix("api/orders")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class OrdersController : ApiController
    {
        private readonly IDAL _db;
        private readonly BL _bl;
        private readonly ILog _log = LogManager.GetLogger(typeof(OrdersController));

        public OrdersController()
        {
            _db = new HwInfContext();
            _bl = new BL(_db);
        }

        public OrdersController(IDAL db)
        {
            _db = db;
            _bl = new BL(db);
        }

        /// <summary>
        /// Get Orders
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(OrderViewModel))]
        [Route("")]
        public IHttpActionResult GetOrders()
        {
            var orders = _bl.GetOrders()
                .ToList()
                .Select(i => new OrderViewModel(i).LoadOrderItems(i))
                .ToList();

            return Ok(orders);
        }

        /// <summary>
        /// Get Single Order
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(OrderViewModel))]
        [Route("id/{id}")]
        public IHttpActionResult GetOrders(int id)
        {
            var order = _bl.GetOrders(id);

            if(order == null) return NotFound();

            var vmdl =  new OrderViewModel(order).LoadOrderItems(order);

            return Ok(vmdl);
        }

        /// <summary>
        /// Get Single Order by Guid
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [ResponseType(typeof(OrderViewModel))]
        [Route("guid/{guid}")]
        public IHttpActionResult GetOrdersGuid(Guid guid)
        {
            var order = _bl.GetOrders(guid);

            if (order == null) return NotFound();

            var vmdl = new OrderViewModel(order).LoadOrderItems(order);

            return Ok(vmdl);
        }

        /// <summary>
        /// Create Order
        /// </summary>
        /// <param name="vmdl"></param>
        /// <returns></returns>
        [ResponseType(typeof(OrderViewModel))]
        [Route("")]
        public IHttpActionResult PostOrder([FromBody]OrderViewModel vmdl)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order = _bl.CreateOrder();
            vmdl.ApplyChanges(order, _bl);
            vmdl.LoadOrderItems(order).Refresh(order);

            _bl.SaveChanges();

            return Ok(vmdl);
        }

        /// <summary>
        /// Starts the RunTime Text Template and creates the contract as pdf
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <returns></returns>
        [Route("print/{id}")]
        public IHttpActionResult GetPrint(int id)
        {

            var uid = _bl.GetOrders(id).Entleiher.Uid;
            if (uid != _bl.GetCurrentUid() && !_bl.IsAdmin())
            {
                return Unauthorized();
            }


            var rpt = new Contract(id, uid);
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
            pdf.Save(AppDomain.CurrentDomain.BaseDirectory+"\\Ausleihvertrag.pdf");


            HttpResponseMessage result;
            var localFilePath = AppDomain.CurrentDomain.BaseDirectory + "\\Hello.pdf";

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


            return Ok(result);

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