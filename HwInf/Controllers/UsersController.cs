using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Security.Claims;
using System.Web.Http.Description;
using HwInf.Common.DAL;
using HwInf.ViewModels;
using System.Web.Security;
using System.Web;

namespace HwInf.Controllers
{
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private HwInfContext db = new HwInfContext();

        [Route("id/{id}")]
        public IHttpActionResult GetPersonById(int id)
        {
            var uid = db.Persons.Where(i => i.PersId == id).Select(i => i.uid).SingleOrDefault();
            
            if (IsCurrentUser(uid) || IsAdmin())
            {
                var p = db.Persons.Single(i => i.PersId == id);
                var vmdl = new UserViewModel(p);

                return Ok(vmdl);
            }

            return Unauthorized();
        }

        [Route("uid/{uid}")]
        public IHttpActionResult GetPersonByUid(string uid)
        {
            
            if (IsCurrentUser(uid) || IsAdmin())
            {
                var p = db.Persons.Single(i => i.uid == uid);
                var vmdl = new UserViewModel(p);

                return Ok(vmdl);
            }

            return Unauthorized();
           
        }

        [Route("update")]
        public IHttpActionResult PostUpdateUser([FromBody] UserViewModel vmdl)
        {

            Person p = db.Persons
                .Include(i => i.Room)
                .Include(i => i.Role)
                .Where(i => i.uid == vmdl.Uid)
                .FirstOrDefault<Person>();
            
            if(IsAdmin())
            {
                vmdl.ApplyChanges(p, db);
            } else if(IsCurrentUser(vmdl.Uid))
            {
                vmdl.ApplyChangesToTel(p, db);
            } else
            {
                return Unauthorized();
            }

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

        private bool PersonExists(int id)
        {
            return db.Persons.Count(e => e.PersId == id) > 0;
        }

        private bool IsCurrentUser(string uid)
        {
            return User.Identity.Name == uid ? true : false;
        }

        private bool IsAdmin()
        {
            return RequestContext.Principal.IsInRole("Admin") ? true : false;
        }
    }
}