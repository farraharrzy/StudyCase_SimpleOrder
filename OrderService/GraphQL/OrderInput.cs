namespace OrderService.GraphQL
{
    public class OrderInput
    {
        public int? UserId { get; set; }
        public string Product { set; get; }
        public int Quantity { set; get; }
        public float Price { set; get; }
    }
}
