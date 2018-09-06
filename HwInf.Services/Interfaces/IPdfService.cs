using System.Collections.Generic;
using HwInf.Common.Models;

namespace HwInf.Services
{
    public interface IPdfService
    {
        byte[] GenerateContrtactPdf(Order order, List<Damage> damages);
        byte[] GenerateContractReturnPdf(Order order, List<Damage> damages);
    }
}