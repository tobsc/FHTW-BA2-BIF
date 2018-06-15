using System.Security.Claims;
using System.Security.Principal;

namespace HwInf.BusinessLogic
{

    public interface IBusinessLogicPrincipal
    {
        bool IsAdmin { get; }
        bool IsVerwalter { get; }
        bool IsAllowed { get; }
        string CurrentUid { get; }
    }
    public class BusinessLogicPrincipal : IBusinessLogicPrincipal
    {
        private static IPrincipal _principal;

        public BusinessLogicPrincipal(IPrincipal principal)
        {
            _principal = principal;
        }
        public bool IsAdmin => _principal.IsInRole("Admin");
        public bool IsVerwalter => _principal.IsInRole("Verwalter");
        public bool IsAllowed => IsAdmin || IsVerwalter;
        public string CurrentUid => (_principal.Identity as ClaimsIdentity)?.FindFirst("Uid").Value;
    }
}