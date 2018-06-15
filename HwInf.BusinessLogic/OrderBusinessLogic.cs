using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security;
using HwInf.BusinessLogic.Interfaces;
using HwInf.Common.Models;
using HwInf.DataAccess.Interfaces;

namespace HwInf.BusinessLogic
{
    public class OrderBusinessLogic : IOrderBusinessLogic
    {
        private readonly IDataAccessLayer _dal;
        private readonly IBusinessLogicPrincipal _principal;

        public OrderBusinessLogic(IDataAccessLayer dal, IBusinessLogicPrincipal principal)
        {
            _dal = dal;
            _principal = principal;
        }
        public IEnumerable<Order> GetOrders()
        {
            return _dal.Orders;
        }

        public Order GetOrders(int orderId)
        {
            if (!_principal.IsAllowed)
            {
                return _dal.Orders
                    .SingleOrDefault(i => i.OrderId.Equals(orderId) && i.Entleiher.Uid.Equals(_principal.CurrentUid));
            }

            return _dal.Orders
                .SingleOrDefault(i => i.OrderId.Equals(orderId));
        }

        public Order GetOrders(Guid guid)
        {
            return _dal.Orders.SingleOrDefault(i => i.OrderGuid.Equals(guid));
        }

        public IEnumerable<OrderStatus> GetOrderStatus()
        {
            return _dal.OrderStatus;
        }

        public OrderStatus GetOrderStatus(string slug)
        {
            return _dal.OrderStatus.FirstOrDefault(i => i.Slug.Equals(slug));
        }

        public IEnumerable<OrderItem> GetOrderItems()
        {
            return _dal.OrderItems;
        }

        public OrderItem GetOrderItem(int id)
        {
            return _dal.OrderItems.FirstOrDefault(i => i.ItemId.Equals(id));
        }

        public Order CreateOrder()
        {
            return _dal.CreateOrder();
        }

        public OrderItem CreateOrderItem()
        {
            return _dal.CreateOrderItem();
        }

        public void UpdateOrderItem(OrderItem obj)
        {
            if (!_principal.IsAllowed) throw new SecurityException();
            _dal.UpdateObject(obj);
        }


        /// <summary>
        /// Returns a collection of orders
        /// </summary>
        /// <param name="statusSlugs">Slugs of order stati</param>
        /// <param name="order">order DESC or ASC</param>
        /// <param name="orderBy">order by property</param>
        /// <param name="orderByFallback">2nd order by property</param>
        /// <param name="isAdminView">true: show orders filtered by verwalter/admin, false: show orders filtered by entleiher</param>
        /// <returns></returns>
        public ICollection<Order> GetFilteredOrders(
            ICollection<string> statusSlugs,
            string order,
            string orderBy,
            string orderByFallback,
            bool isAdminView
        )
        {
            if (isAdminView && !_principal.IsAllowed)
            {
                throw new SecurityException();
            }

            var result = GetOrders().ToList();

            if (statusSlugs.Any())
            {
                result = result.Where(i => statusSlugs.Contains(i.OrderStatus.Slug)).ToList();
            }

            // Order ASC or DESC and filter by provided property
            result = order.Equals("ASC", StringComparison.OrdinalIgnoreCase)
                ? result.OrderBy(i => i.GetType().GetProperty(orderBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)?.GetValue(i, null))
                    .ThenBy(i => i.GetType().GetProperty(orderByFallback, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)?.GetValue(i, null))
                    .ToList()
                : result.OrderByDescending(i => i.GetType().GetProperty(orderBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)?.GetValue(i, null))
                    .ThenByDescending(i => i.GetType().GetProperty(orderByFallback, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)?.GetValue(i, null))
                    .ToList();

            return !isAdminView ?
                result.Where(i => i.Entleiher.Uid.Equals(_principal.CurrentUid)).ToList()
                : _principal.IsAdmin ?
                    result
                    : result.Where(i => i.Verwalter.Uid.Equals(_principal.CurrentUid)).ToList();
        }

        public ICollection<Order> SearchOrders(
            string searchQuery,
            string order,
            string orderBy,
            string orderByFallback,
            bool isAdminView)
        {

            if (isAdminView && !_principal.IsAllowed)
            {
                throw new SecurityException();
            }

            searchQuery = searchQuery.ToLower();
            var result = GetOrders().ToList();
            if (isAdminView)
            {
                if (_principal.IsVerwalter) result = result
                        .Where(i => i.Verwalter.Uid.Equals(_principal.CurrentUid))
                        .ToList();

                result = result
                    .Where(i => i.Entleiher.Uid.ToLower().Contains(searchQuery)
                                || i.Entleiher.Name.ToLower().Contains(searchQuery)
                                || i.Entleiher.LastName.ToLower().Contains(searchQuery)
                                || i.OrderId.ToString().ToLower().Contains(searchQuery)
                                || i.OrderItems
                                    .ToList()
                                    .Any(x => x.Device.Name.ToLower().Contains(searchQuery)
                                              || x.Device.InvNum.ToLower().Contains(searchQuery)))
                    .ToList();
            }
            else
            {
                result = result
                    .Where(i => i.Entleiher.Uid.Equals(_principal.CurrentUid))
                    .Where(i => i.Verwalter.Uid.ToLower().Contains(searchQuery)
                        || i.Verwalter.Name.ToLower().Contains(searchQuery)
                        || i.Verwalter.LastName.ToLower().Contains(searchQuery)
                        || i.OrderId.ToString().ToLower().Contains(searchQuery)
                        || i.OrderItems
                            .ToList()
                            .Any(x => x.Device.Name.ToLower().Contains(searchQuery)
                                        || x.Device.InvNum.ToLower().Contains(searchQuery)))
                    .ToList();

            }

            result = order.Equals("ASC")
                ? result.OrderBy(i => i.GetType().GetProperty(orderBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)?.GetValue(i, null))
                    .ThenBy(i => i.GetType().GetProperty(orderByFallback, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)?.GetValue(i, null))
                    .ToList()
                : result.OrderByDescending(i => i.GetType().GetProperty(orderBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)?.GetValue(i, null))
                    .ThenByDescending(i => i.GetType().GetProperty(orderByFallback, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)?.GetValue(i, null))
                    .ToList();

            return result;
        }

    }
}