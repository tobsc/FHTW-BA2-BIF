using System.IdentityModel.Tokens.Jwt;
using HwInf.Common.Models;

namespace HwInf.BusinessLogic.Interfaces
{
    public interface IBusinessLogic
    {
        bool IsAdmin { get; }
        bool IsVerwalter { get; }
        string[] GetLog();
        bool IsAdminUid(string uid);
        string GetCurrentUid();
        JwtSecurityToken CreateToken(Person p);
    }
}