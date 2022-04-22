﻿using System;
using System.Collections.Generic;

namespace WaveShopAPIRest.Models
{
    public partial class ShoppingCart
    {
        public ShoppingCart()
        {
            ProductSelectedCarts = new HashSet<ProductSelectedCart>();
        }

        public int id { get; set; }
        public int productsQuantity { get; set; }
        public double subtotal { get; set; }
        public DateTime LastUpdate { get; set; }
        public int IdUser { get; set; }

        public virtual User IdUserNavigation { get; set; } = null!;
        public virtual ICollection<ProductSelectedCart> ProductSelectedCarts { get; set; }
    }
}
