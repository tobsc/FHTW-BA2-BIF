using System;
using System.Collections.Generic;
using HwInf.Common.Models;

namespace HwInf.BusinessLogic.Interfaces
{
    public interface IOrderBusinessLogic
    {
        IEnumerable<Order> GetOrders();
        Order GetOrders(int orderId);
        Order GetOrders(Guid guid);
        IEnumerable<OrderStatus> GetOrderStatus();
        OrderStatus GetOrderStatus(string slug);
        IEnumerable<OrderItem> GetOrderItems();
        OrderItem GetOrderItem(int id);
        Order CreateOrder();
        OrderItem CreateOrderItem();
        void UpdateOrderItem(OrderItem obj);

        ICollection<Order> GetFilteredOrders(
            ICollection<string> statusSlugs,
            string order,
            string orderBy,
            string orderByFallback,
            bool isAdminView
        );

        ICollection<Order> SearchOrders(
            string searchQuery,
            string order,
            string orderBy,
            string orderByFallback,
            bool isAdminView);
    }
}