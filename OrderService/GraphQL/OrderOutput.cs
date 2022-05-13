namespace OrderService.GraphQL
{
    public record OrderOutput
    (
        bool status,
        string? message
    );
}
