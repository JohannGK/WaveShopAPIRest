using System;
using System.Collections.Generic;

namespace WaveShopAPIRest.Models
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Status { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; }
    }
}
