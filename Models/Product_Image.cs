using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WaveShopAPIRest.Models
{
    public partial class Product_Image
    {
        public int Id { get; set; }
        public string Url { get; set; } = null!;
        public DateTime LastUpdate { get; set; }
        public int? IdProduct { get; set; }

        [JsonIgnore]
        public virtual Product? IdProductNavigation { get; set; }
    }
}
