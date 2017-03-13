using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using HwInf.Common.Models;

namespace HwInf.Controllers
{
    [Authorize]
    [RoutePrefix("api/orders")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class OrdersController : ApiController
    {
        private HwInfContext db = new HwInfContext();


        /// <summary>
        /// Returns Order by given id.
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <returns></returns>
        [ResponseType(typeof(OrderViewModel))]
        [Route("id/{id}")]
        public IHttpActionResult GetById(int id)
        {
            var uid = db.Orders.Where(i => i.OrderId == id).Select(i => i.Person.uid).SingleOrDefault();


            if(!IsAllowed(uid) && !IsAdmin())
            {
                return Unauthorized();
            }

                var orders = db.Orders
                    .Include(i => i.Person)
                    .Include(i => i.Owner);

                var vmdl = orders
                    .Where(i => i.OrderId == id)
                    .ToList()
                    .Select(i => new OrderViewModel(i).LoadOrderItems(db))
                    .ToList();


                return Ok(vmdl);

        }

        /// <summary>
        /// Returns all orders from a user.
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(OrderViewModel))]
        [Route("outgoing")]
        public IHttpActionResult GetByUid()
        {

            var uid = User.Identity.Name;


            var orders = db.Orders
                .Include(i => i.Person)
                .Include(i => i.Owner);

            var vmdl = orders
                .Where(i => i.Person.uid == uid)
                .ToList()
                .Select(i => new OrderViewModel(i).LoadOrderItems(db))
                .ToList();

            return Ok(vmdl);

        }

        /// <summary>
        /// Returns all orders.
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(OrderViewModel))]
        [Authorize(Roles = "Admin,Verwalter")]
        [Route("incoming")]
        public IHttpActionResult GetOwnerOrders()
        {
            var orders = db.Orders
                .Include(i => i.Person)
                .Include(i => i.Owner);

            List<OrderViewModel> vmdl;

            if (IsAdmin())
            {
                vmdl = orders
                    .ToList()
                    .Select(i => new OrderViewModel(i).LoadOrderItems(db))
                    .ToList();
            } else
            {
                string uid = User.Identity.Name;

                vmdl = orders
                    .Where(i => i.Owner.uid == uid)
                    .ToList()
                    .Select(i => new OrderViewModel(i).LoadOrderItems(db))
                    .ToList();
            }


            return Ok(vmdl);

        }

        /// <summary>
        /// Change Order Status
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <param name="act">Action: accept, decline, return</param>
        /// <returns></returns>
        [Route("id/{id}/{act}")]
        public IHttpActionResult GetChangeOrderStatus(int id, string act)
        {
            var uid = db.Orders.Where(i => i.OrderId == id).Select(i => i.Owner.uid).SingleOrDefault();

            if (!IsAllowed(uid) && !IsAdmin())
            {
                return Unauthorized();
            }

            if(!act.Equals("accept") && !act.Equals("decline") && !act.Equals("return"))
            {
                return BadRequest("Wrong action!");
            }
  
            var o = db.Orders.Single(i => i.OrderId == id);

            var order = new OrderViewModel(o);

            order.ChangeStatus(o, db, act);


            db.SaveChanges();


            return Ok(act + " erfolgreich!");

        }

        /// <summary>
        /// Return OrderStatus
        /// </summary>
        /// <returns></returns>
        [Route("status")]
        public IHttpActionResult GetStatus()
        {
            return Ok(db.OrderStatus.ToList());
        }


        /// <summary>
        /// Creates a new order.
        /// </summary>
        /// <param name="vmdl">From, To, OrderItems</param>
        /// <example>test</example>
        /// <returns></returns>
        [Route("create")]
        public IHttpActionResult PostOrder([FromBody] OrderViewModel vmdl)
        {
            if (!IsAllowed(vmdl.PersonUid) && !IsAdmin())
            {
                return Unauthorized();
            }

            if(vmdl.ContainsDuplicates())
            {
                return BadRequest("Ein Gerät kann nur einmal ausgewählt werden.");
            }

            if (vmdl.ContainsLentItems(db))
            {
                return BadRequest("Nicht alle Geräte sind zum Ausleihen verfügbar.");
            }

            var allOrders = CreateAllOrders(vmdl);
            vmdl.PersonUid = User.Identity.Name;

            allOrders.ForEach(i => db.Orders.Add(i));

            db.SaveChanges();    

            return Ok("Anfrage war erfolgreich!");
        }

        /// <summary>
        /// Starts the RunTime Text Template and creates the contract as pdf
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <returns></returns>
        [Route("print/{id}")]
        public HttpResponseMessage GetPrint(int id)
        {
            var uid = db.Orders.Where(i => i.OrderId == id).Select(i => i.Person.uid).SingleOrDefault();


            if (!IsAllowed(uid) && !IsAdmin())
            {
                return null;
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
            pdf.Save(AppDomain.CurrentDomain.BaseDirectory+"\\Hello.pdf");

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


            return result;

        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool IsAllowed(string uid)
        {
            return User.Identity.Name == uid;
        }

        private bool IsAdmin()
        {
            return RequestContext.Principal.IsInRole("Admin");
        }

        private List<Order> CreateAllOrders(OrderViewModel vmdl)
        {

            var dev = new List<Device>();
            foreach(var oi in vmdl.OrderItems)
            {
                dev.Add(db.Devices.Single(i => i.DeviceId == oi));
            }


            var orders = new List<Order>();

            var groups = dev.GroupBy(i => i.Person.uid);

            foreach(var g in groups)
            {
                Order o = new Order();
                o.Date = DateTime.Now.Date;
                o.From = vmdl.From;
                o.To = vmdl.To;
                o.Person = db.Persons.Single(i => i.uid == User.Identity.Name);
                o.Owner = g.Select(i => i.Person).FirstOrDefault();
                o.Status = db.OrderStatus.Single(i => i.Description == "Offen");

                orders.Add(o);

                foreach(var x in g)
                {
                    OrderItem oi = new OrderItem();
                    oi.Device = x;
                    oi.Order = o;

                    db.OrderItems.Add(oi);
                }
            }


            return orders;
           
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