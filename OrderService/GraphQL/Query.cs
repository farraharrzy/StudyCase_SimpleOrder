using HotChocolate.AspNetCore.Authorization;
using OrderService.Models;
using System.Security.Claims;

namespace OrderService.GraphQL
{
    public class Query
    {
        //List User's Order
        [Authorize]
        public IQueryable<Order> GetOrdersByToken([Service] StudyCase_SimpleOrderContext context, ClaimsPrincipal claimsPrincipal)
        {
            var userName = claimsPrincipal.Identity.Name;
            var user = context.Users.Where(o => o.Username == userName).FirstOrDefault();
            if (user != null)
            {
                var orders = context.Orders.Where(o => o.UserId == user.Id);
                return orders.AsQueryable();
            }
            return new List<Order>().AsQueryable();
        }

        //List All Orders
        [Authorize]
        public IQueryable<Order> GetOrders([Service] StudyCase_SimpleOrderContext context, ClaimsPrincipal claimsPrincipal)
        {
            var userName = claimsPrincipal.Identity.Name;

            // check manager role ?
            var managerRole = claimsPrincipal.Claims.Where(o => o.Type == ClaimTypes.Role && o.Value == "MANAGER").FirstOrDefault();
            var user = context.Users.Where(o => o.Username == userName).FirstOrDefault();
            if (user != null)
            {
                if (managerRole != null)
                    return context.Orders;
                var orders = context.Orders.Where(o => o.UserId == user.Id);
                return orders.AsQueryable();
            }
            return new List<Order>().AsQueryable();
        }
    }
}
