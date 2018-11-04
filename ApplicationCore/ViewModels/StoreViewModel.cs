using ApplicationCore.Entities;
using ApplicationCore.ViewModels;
using System;

namespace ApplicationCore.ViewModels
{
    public class StoreViewModel: EntityViewModel<Store>
    {
        public string Title { get; set; }
        public string OwnerId { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public DateTime RegistrationDate { get; set; }

        public StoreViewModel(): base()
        {
        } 

        public StoreViewModel(Store store): base(store)
        {
            Title = store.Title;
            OwnerId = store.OwnerId;
            Address = store.Address;
            Description = store.Description;
        }

        public override Store ToModel()
        {
            return new Store()
            {
                Id = Id,
                Title = Title,
                Address = Address,
                OwnerId = OwnerId,
                Description = Description,
                RegistrationDate = RegistrationDate,
            };
        }
        public override Store UpdateModel(Store modelToUpdate)
        {
            modelToUpdate.Title = Title;
            modelToUpdate.Address = Address;
            modelToUpdate.Description = Description;
            return modelToUpdate;
        }
    }
}
