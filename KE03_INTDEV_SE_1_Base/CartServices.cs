
public class CartItem
{
    public int ProductId { get; set; }
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string ImgSrc { get; set; } = "";
}

public class CartService
{
    public event Action? OnChange;
    private List<CartItem> items = new();

    public IReadOnlyList<CartItem> Items => items;

    public void AddToCart(CartItem item)
    {
        var existing = items.FirstOrDefault(i => i.ProductId == item.ProductId);
        if (existing != null)
        {
            existing.Quantity += item.Quantity;
        }
        else
        {
            items.Add(item);
        }
        OnChange?.Invoke();
    }

    public int TotalCount => items.Sum(i => i.Quantity);
    public decimal TotalPrice => items.Sum(i => i.Price * i.Quantity);
}
