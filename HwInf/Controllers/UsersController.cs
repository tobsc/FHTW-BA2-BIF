using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using HwInf.Common.BL;
using HwInf.Common.DAL;
using HwInf.Common.Models;
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


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool IsCurrentUser(string uid)
        {
            return User.Identity.Name == uid;
        }

        private bool IsAdmin()
        {
            return RequestContext.Principal.IsInRole("Admin");
        }
    }
}