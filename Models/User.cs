﻿using System;
using System.Collections.Generic;

namespace WaveShopAPIRest.Models
{
    public partial class User
    {
        public User()
        {
            Addresses = new HashSet<Address>();
            Orders = new HashSet<Order>();
            ShoppingCarts = new HashSet<ShoppingCart>();
            IdProducts = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime BirthDay { get; set; }
        public int Age { get; set; }
        public string UerType { get; set; } = null!;
        public string Reputation { get; set; } = null!;
        public DateTime LastLogin { get; set; }
        public DateTime LastUpdate { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }

        public virtual ICollection<Product> IdProducts { get; set; }
    }
}
