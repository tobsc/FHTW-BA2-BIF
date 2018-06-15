using System.Collections.Generic;
using HwInf.Common.Models;

namespace HwInf.PDFService.ViewModels
{
    public class ContractViewModel
    {
        public Order Order { get; set; }
        public List<Damage> Damages { get; set; }
        public void Refresh(Order order, List<Damage> damages)
        {
            Order = order;
            Damages = damages;
        }
    }
}