using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using HwInf.Common.BL;

namespace HwInf.ViewModels
{
    public class OrderListViewModel
    {
        public int MaxPages { get; set; }
        public int Results { get; set; }

        public IEnumerable<OrderViewModel> Orders { get; set; }


        public OrderListViewModel()
        {

        }

        public OrderListViewModel(IEnumerable<OrderViewModel> obj, int limit, int count)
        {
            Orders = obj.ToList();
            Results = count;
            MaxPages = limit == 0 ? 0 : (int)Math.Ceiling((double)count / limit);
        }


    }
}