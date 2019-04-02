using ApplicationCore.Extensions;
using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    /// <summary>
    /// Store for seller
    /// </summary>
    public class Store: Entity, ShallowClonable<Store>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public DateTime RegistrationDate { get; set; }

        public virtual IEnumerable<Item> Items { get; set; } = new List<Item>();

        public Store(): base()
        {
        }

        public bool Equals(Store other) => base.Equals(other)
            && Title.EqualsCI(other.Title)
            && Description.EqualsCI(other.Description)
            && Address.EqualsCI(other.Address)
            && RegistrationDate == other.RegistrationDate;
        public override bool Equals(object obj) => Equals(obj as Store);
        public override int GetHashCode() => base.GetHashCode();

        protected Store(Store store): base(store)
        {
            Title = store.Title;
            Description = store.Description;
            Address = store.Address;
            RegistrationDate = store.RegistrationDate;
        }

        public Store ShallowClone()
        {
            return (Store)MemberwiseClone();
        }
    }
}
