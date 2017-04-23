using System;
using System.Collections.Generic;
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
        private readonly ILog _log = LogManager.GetLogger(typeof(OrdersController).Name);

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

        [ResponseType(typeof(OrderViewModel))]
    
        [Route("filter")]
        public IHttpActionResult PostFilter([FromBody] OrderFilterViewModel vmdl)
        {
            var result = vmdl.FilteredList(_bl).Select(i => new OrderItemViewModel(i)).ToList();
            return Ok(result);
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

            var vmdls = SplitOrders(vmdl);

            vmdls.ForEach(i =>
            {
                var order = _bl.CreateOrder();
                i.ApplyChanges(order, _bl);
                i.LoadOrderItems(order).Refresh(order);
            });

            _bl.SaveChanges();
            vmdls.ForEach(i =>
            {
                _log.InfoFormat("Order '{0}' created by '{1}'", i.OrderGuid, User.Identity.Name);
            });
            

            return Ok(vmdls);
        }

        /// <summary>
        /// Change Status of Order Item
        /// </summary>
        /// <param name="id"></param>
        /// <param name="slug"></param>
        /// <returns></returns>
        [ResponseType(typeof(OrderItemViewModel))]
        [Route("orderitem/{id}/{slug}")]
        public IHttpActionResult PutOrderItem(int id, string slug)
        {
            var obj = _bl.GetOrderItem(id);
            var status = _bl.GetOrderStatus(slug);

            if (obj == null || status == null) return NotFound();
          
            _bl.UpdateOrderItem(obj);
            obj.OrderStatus = status;

            if (slug.Equals("abgeschlossen"))
            {
                obj.ReturnDate = DateTime.Now;
            }

            _bl.SaveChanges();

            _log.InfoFormat("Status of ordered Device '{0}' changed to '{1}' by '{2}'", obj.Device.InvNum, status.Name, User.Identity.Name);

            var vmdl = new OrderItemViewModel(obj);
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

        private List<OrderViewModel> SplitOrders(OrderViewModel vmdl)
        {

            // Load Devices
            vmdl.OrderItems
                .ToList()
                .ForEach(x => x.Device = new DeviceViewModel(_bl.GetSingleDevice(x.Device.InvNum)));

            // Group by Verwalter
            var devices = vmdl.OrderItems.GroupBy(i => i.Device.Verwalter).Select(x => x.ToList()).ToList();

            // Create vmdls
            var vmdls = new List<OrderViewModel>();
            devices.ForEach(i =>
            {
                var tmp = new OrderViewModel(vmdl) {OrderItems = i.ToList()};
                vmdls.Add(tmp);
            });

            return vmdls;

        }
    }
}