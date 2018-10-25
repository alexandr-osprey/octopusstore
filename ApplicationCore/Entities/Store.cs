using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    /// <summary>
    /// Store for seller
    /// </summary>
    public class Store: Entity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public DateTime RegistrationDate { get; set; }

        public IEnumerable<Item> Items { get; set; } = new List<Item>();
    }
}
