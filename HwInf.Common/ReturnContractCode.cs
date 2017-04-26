using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HwInf.Common
{
    partial class ReturnContract
    {
        private string uid;
        private int orderId;
        public ReturnContract(int orderId, string uid)
        {
            this.uid = uid;
            this.orderId = orderId;
        }
    }
}
