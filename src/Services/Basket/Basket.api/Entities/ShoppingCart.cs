using System.Collections.Generic;

namespace Basket.Api.Entities
{
    public class ShoppingCart
    {
        public string Name { get; set; }
        public IEnumerable<ShoppingCartItem> Items { get; set; }
        public decimal TotalPrice
        {
            get
            {
                var total = 0m;
                foreach (var item in this.Items)
                {
                    total += item.Quantity * item.Price;
                }
                return total;
            }
        }

        public ShoppingCart(string name)
        {
            this.Name = name;
            this.Items = new List<ShoppingCartItem>();
        }
    }
}