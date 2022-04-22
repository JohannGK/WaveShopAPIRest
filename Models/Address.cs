using System;
using System.Collections.Generic;

namespace WaveShopAPIRest.Models
{
    public partial class Address
    {
        public int Id { get; set; }
        public string Zip { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string State { get; set; } = null!;
        public string city { get; set; } = null!;
        public int IdUser { get; set; }

        public virtual User IdUserNavigation { get; set; } = null!;
    }
}
