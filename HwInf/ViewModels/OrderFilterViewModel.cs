using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Compilation;
using HwInf.Common.BL;
using HwInf.Common.Models;
using MigraDoc.DocumentObjectModel.Shapes.Charts;

namespace HwInf.ViewModels
{
    public class OrderFilterViewModel
    {
        public string Order { get; set; } = "ASC";
        public string OrderBy { get; set; } = "OrderStatus";
        public string OrderByFallback { get; set; } = "From";
        public ICollection<string> StatusQuery { get; set; } = new List<string>();
        public ICollection<string> UidQuery { get; set; } = new List<string>();
        public bool IsIncoming { get; set; } = false;

        public ICollection<OrderItem> FilteredList(BL bl)
        {
            return bl.GetFilteredOrderItems(StatusQuery, UidQuery, Order, OrderBy, OrderByFallback, IsIncoming);
        }
    }
}