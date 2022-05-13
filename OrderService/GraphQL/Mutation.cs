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
            var key = "Submit-Order-" + DateTime.Now.ToString();
            var val = JsonConvert.SerializeObject(input);
            var result = await KafkaHelper.SendMessage(settings.Value, "tugasfinal", key, val);

            var ret = new OrderOutput(result, "Success to Submit Order");
            if (!result)
                ret = new OrderOutput(result, "Failed to Submit Order");

            return await Task.FromResult(ret);
        }
    }
}
