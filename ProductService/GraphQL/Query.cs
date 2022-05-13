using HotChocolate.AspNetCore.Authorization;
using ProductService.Models;
using UserService.Models;

namespace UserService.GraphQL
{
    public class Query
    {
        public IQueryable<Product> GetProducts([Service] StudyCase_SimpleOrderContext context) =>
            context.Products;
    }
}
