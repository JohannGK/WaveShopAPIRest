using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WaveShopAPIRest.Models
{
    public partial class ProductSelectedOrder
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string? Status { get; set; }
        public int IdProduct { get; set; }
        public int IdOrder { get; set; }
         [JsonIgnore]
        public virtual Order IdOrderNavigation { get; set; } = null!;
         [JsonIgnore]
        public virtual Product IdProductNavigation { get; set; } = null!;
    }
}
