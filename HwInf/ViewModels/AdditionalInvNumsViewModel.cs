using System.Collections.Generic;
using System.Linq;
using HwInf.Common.BL;

namespace HwInf.ViewModels
{
    public class AdditionalInvNumViewModel
    {
        public string InvNum { get; set; }

        public AdditionalInvNumViewModel()
        {
            
        }

        public AdditionalInvNumViewModel(string invNum)
        {
            InvNum = invNum;
        }
    }
}