using System;
using System.Collections.Generic;

namespace WaveShopAPIRest.Models
{
    public partial class ProductSelectedCart
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string? Status { get; set; }
        public int IdProduct { get; set; }
        public int IdShoppingCart { get; set; }

        public virtual Product IdProductNavigation { get; set; } = null!;
        public virtual ShoppingCart IdShoppingCartNavigation { get; set; } = null!;
    }
}
