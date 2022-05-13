using HotChocolate.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OrderService.Models;

namespace OrderService.GraphQL
{
    public class Mutation
    {
        [Authorize]
        public async Task<OrderOutput> SubmitOrderAsync(
           OrderInput input,
           [Service] IOptions<KafkaSettings> settings)
        {
            var dts = DateTime.Now.ToString();
            var key = "order-" + dts;

            // EF
            var order = new OrderInput
            {
                UserId = input.UserId,
                Product = input.Product,
                Quantity = input.Quantity,
                Price = input.Price
            };

            var val = JsonConvert.SerializeObject(order);

            var result = await KafkaHelper.SendMessage(settings.Value, "tugasfinal", key, val);

            OrderOutput resp = new OrderOutput
            {
                TransactionDate = dts,
                Message = "Order was submitted successfully"
            };

            if (!result)
                resp.Message = "Failed to submit data";

            return await Task.FromResult(resp);
        }
    }
}
