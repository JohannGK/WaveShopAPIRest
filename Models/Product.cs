using System;
using System.Collections.Generic;

namespace WaveShopAPIRest.Models
{
    public partial class Product
    {
        public Product()
        {
            ProductSelectedCarts = new HashSet<ProductSelectedCart>();
            ProductSelectedOrders = new HashSet<ProductSelectedOrder>();
            IdUsers = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? PhotoAddress { get; set; }
        public string? VideoAddress { get; set; }
        public int StockQuantity { get; set; }
        public double UnitPrice { get; set; }
        public string Status { get; set; } = null!;
        public DateTime Published { get; set; }
        public string Country { get; set; } = null!;
        public string Location { get; set; } = null!;
        public int IdCategory { get; set; }
        public int IdVendor { get; set; }
        public int LikesNumber { get; set; }
        public int DislikesNumber { get; set; }
        public int ShoppedTimes { get; set; }
        public int CommentsNumber { get; set; }

        public virtual Category IdCategoryNavigation { get; set; } = null!;
        public virtual ICollection<ProductSelectedCart> ProductSelectedCarts { get; set; }
        public virtual ICollection<ProductSelectedOrder> ProductSelectedOrders { get; set; }

        public virtual ICollection<User> IdUsers { get; set; }
    }
}
