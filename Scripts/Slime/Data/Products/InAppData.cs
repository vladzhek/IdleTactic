namespace Slime.Data.Products
{
    public class InAppData
    {
        public InAppData(float price, string localizedPrice, string currency = null)
        {
            Price = price;
            LocalizedPrice = localizedPrice;
            Currency = currency;
        }

        public float Price { get; }
        public string LocalizedPrice { get; }
        public string Currency { get; }
    }
}