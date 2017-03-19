using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using HwInf.Common.BL;
using HwInf.Common.DAL;
using HwInf.ViewModels;

namespace HwInf.Controllers
{
    [Authorize]
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private readonly HwInfContext _db = new HwInfContext();
        private readonly BL _bl;


        public UsersController()
        {
            _bl = new BL(_db);
        }

        /// <summary>
        /// Returns UserViewModel of given Uid.
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(UserViewModel))]
        [Route("userdata")]
        public IHttpActionResult GetPersonByUid()
        {
            var p = _bl.GetUsers(User.Identity.Name);
            var vmdl = new UserViewModel(p);

            return Ok(vmdl);
           
        }

        /// <summary>
        /// Returns List of Users.
        /// </summary>
        /// <returns>LastName, Name, Uid</returns>
        [Authorize(Roles="Verwalter, Admin")]
        [ResponseType(typeof(UserViewModel))]
        [Route("owners")]
        public IHttpActionResult GetOwners()
        {
            var vmdl = _bl.GetUsers()
                .Where(i => i.Role.Name != "User")
                .ToList()
                .Select(i => new UserViewModel(i))
                .ToList();

            return Ok(vmdl);

        }

        /// <summary>
        /// Save Phonenumber
        /// </summary>
        /// <param name="vmdl"></param>
        /// <returns></returns>
        [Route("update")]
        public IHttpActionResult PostUpdateUser([FromBody] UserViewModel vmdl)
        {

            var obj = _bl.GetUsers(User.Identity.Name);

            vmdl.ApplyChangesTelRoom(obj);
            vmdl.Refresh(obj);
            _bl.SaveChanges();

            return Ok(vmdl);
        }

        /// <summary>
        /// Add Admin
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [Route("admin")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult PutAddAdmin(string uid)
        {

            var p = _bl.GetUsers(uid);
            _bl.SetAdmin(p);
            _bl.SaveChanges();  
       


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