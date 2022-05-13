namespace OrderService.GraphQL
{
    public class OrderInput
    {
        public string Product { set; get; }
        public int Quantity { set; get; }
        public float Price { set; get; }
    }
}
