using ApplicationCore.Entities;
using ApplicationCore.ViewModels;
using System;

namespace OctopusStore.ViewModels
{
    public class StoreDetailViewModel : DetailViewModel<Store>
    {
        public string Address { get; set; }
        public string Description { get; set; }
        public string SellerId { get; set; }
        public DateTime RegistrationDate { get; set; }

        public StoreDetailViewModel(Store store)
            : base(store)
        {
            Title = store.Title;
            SellerId = store.SellerId;
            Address = store.Address;
            Description = store.Description;
            RegistrationDate = store.RegistrationDate;
        }

        public override Store ToModel()
        {
            return new Store()
            {
                Id = Id,
                Title = Title,
                Address = Address,
                SellerId = SellerId,
                Description = Description,
                RegistrationDate = RegistrationDate
            };
        }
        public override void UpdateModel(Store modelToUpdate)
        {
            modelToUpdate.Title = Title;
            modelToUpdate.SellerId = SellerId;
            modelToUpdate.Address = Address;
            modelToUpdate.Description = Description;
        }
    }
}
