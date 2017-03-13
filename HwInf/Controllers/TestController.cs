using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using HwInf.Common.DAL;
using HwInf.Common.Models;

namespace HwInf.Controllers
{
    [RoutePrefix("api/test")]
    public class TestController : ApiController
    {
        
        private readonly HwInfContext _db = new HwInfContext();

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
                dev.Status = _db.DeviceStatus.Single(x => x.StatusId == 1);
                dev.Type = _db.DeviceTypes.Single(x => x.TypeId == type);
                dev.CreateDate = DateTime.Now;
                dev.Person = _db.Persons.Single(x => x.uid == "if15b032");
                dev.Room = "A0.00";

                for(int j = 0; j<meta; j++)
                {
                    DeviceMeta m = new DeviceMeta();
                    m.Component.Name = "Key" + j;
                    m.MetaValue = "Value" + j;

                    _db.DeviceMeta.Add(m);
                }

                _db.Devices.Add(dev);
                _db.SaveChanges();
            }



            return Ok();
        }

        [Route("createInitial")]
        public IHttpActionResult GetInitial()
        {

            Role ro = new Role();
            ro.Name = "Admin";
            _db.Roles.Add(ro);
            _db.SaveChanges();

            Person p = new Person();
            p.Name = "Tobias";
            p.LastName = "Schlachter";
            p.uid = "if15b032";
            p.Email = "tobias.schlachter@technikum-wien.at";
            p.Role = _db.Roles.Single(x => x.RoleId == 1);


            _db.Persons.Add(p);

            var type = new List<DeviceType>
            {
                new DeviceType { Description = "Notebook" },
                new DeviceType { Description = "PC" },
                new DeviceType { Description = "Monitor" }

            };

            type.ForEach(x => _db.DeviceTypes.Add(x));

            DeviceStatus s = new DeviceStatus();
            s.Description = "Verfügbar";

            _db.DeviceStatus.Add(s);

            _db.SaveChanges();


            return Ok();
        }

        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}