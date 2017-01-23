using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using HwInf.Common.DAL;

namespace HwInf.Controllers
{
    [RoutePrefix("api/test")]
    public class TestController : ApiController
    {
        
        private HwInfContext db = new HwInfContext();

        // GET: api/Test
        [Route("create/{number}/{type}/{brand}/{meta}")]
        public IHttpActionResult GetCreateDevices(int number,int type, string brand, int meta)
        {

            for(int i = 0; i<number; i++)
            {
                Device dev = new Device();
                dev.Brand = brand;
                dev.Name = "Device"+i;
                dev.InvNum = "T000"+i;
                dev.Status = db.DeviceStatus.Single(x => x.StatusId == 1);
                dev.Type = db.DeviceTypes.Single(x => x.TypeId == type);
                dev.CreateDate = DateTime.Now;
                dev.Person = db.Persons.Single(x => x.uid == "if15b032");
                dev.Room = "A0.00";

                for(int j = 0; j<meta; j++)
                {
                    DeviceMeta m = new DeviceMeta();
                    m.Component.DeviceType = dev.Type;
                    m.Device = dev;
                    m.Component.Name = "Key" + j;
                    m.MetaValue = "Value" + j;

                    db.DeviceMeta.Add(m);
                }

                db.Devices.Add(dev);
                db.SaveChanges();
            }



            return Ok();
        }

        [Route("createInitial")]
        public IHttpActionResult GetInitial()
        {

            Role ro = new Role();
            ro.Name = "Admin";
            db.Roles.Add(ro);
            db.SaveChanges();

            Person p = new Person();
            p.Name = "Tobias";
            p.LastName = "Schlachter";
            p.uid = "if15b032";
            p.Email = "tobias.schlachter@technikum-wien.at";
            p.Role = db.Roles.Single(x => x.RoleId == 1);


            db.Persons.Add(p);

            var type = new List<DeviceType>
            {
                new DeviceType { Description = "Notebook" },
                new DeviceType { Description = "PC" },
                new DeviceType { Description = "Monitor" }

            };

            type.ForEach(x => db.DeviceTypes.Add(x));

            DeviceStatus s = new DeviceStatus();
            s.Description = "Verfügbar";

            db.DeviceStatus.Add(s);

            db.SaveChanges();


            return Ok();
        }

        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DeviceExists(int id)
        {
            return db.Devices.Count(e => e.DeviceId == id) > 0;
        }
    }
}