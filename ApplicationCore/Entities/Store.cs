using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    public class Store : Entity
    {
        public string Title { get; set; }
        public string SellerId { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public DateTime RegistrationDate { get; set; }

        public IEnumerable<Item> Items { get; set; } = new List<Item>();
    }
}
