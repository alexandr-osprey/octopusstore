using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Identity;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Specifications;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class StoreService: Service<Store>, IStoreService
    {
        protected IItemService ItemService { get; }

        public StoreService(
           StoreContext context,
            IIdentityService identityService,
            IScopedParameters scopedParameters,
            IItemService itemService,
            IAuthorizationParameters<Store> authoriationParameters,
            IAppLogger<Service<Store>> logger)
           : base(context, identityService, scopedParameters, authoriationParameters, logger)
        {
            ItemService = itemService;
        }

        public override async Task<Store> CreateAsync(Store entity)
        {
            await base.CreateAsync(entity);
            await IdentityService.AddClaim(entity.OwnerId, new Claim(CustomClaimTypes.StoreAdministrator, entity.Id.ToString()));
            return entity;
        }

        protected override async Task ValidateCustomUniquinessWithException(Store store)
        {
            await base.ValidateCustomUniquinessWithException(store);
            if (await Context.ExistsBySpecAsync(Logger, new Specification<Store>(s => s.OwnerId == ScopedParameters.CurrentUserId)))
                throw new EntityAlreadyExistsException("User already has a store");
        }

        override public async Task<int> DeleteAsync(Specification<Store> spec)
        {
            spec.AddInclude((s => s.Items));
            spec.Description += ", includes Items";
            return await base.DeleteAsync(spec);
        }

        public override async Task DeleteRelatedEntitiesAsync(Store store)
        {
            var spec = new Specification<Item>(i => store.Items.Contains(i))
            {
                Description = $"Item with StoreId={store.Id}"
            };
            await ItemService.DeleteAsync(spec);
            await IdentityService.RemoveFromUsersAsync(new Claim(store.OwnerId, store.Id.ToString()));
            await base.DeleteRelatedEntitiesAsync(store);
        }

        protected override async Task ValidateWithExceptionAsync(EntityEntry<Store> entry)
        {
            await base.ValidateWithExceptionAsync(entry);
            var store = entry.Entity;
            if (string.IsNullOrWhiteSpace(store.Title))
                throw new EntityValidationException("Incorrect title");
            if (string.IsNullOrWhiteSpace(store.Description))
                throw new EntityValidationException("Incorrect description");
            if (string.IsNullOrWhiteSpace(store.Address))
                throw new EntityValidationException("Incorrect address");
            var entityEntry = Context.Entry(store);
            IsPropertyModified(entityEntry, s => s.RegistrationDate, false);
        }


        protected override async Task ModifyBeforeSaveAsync(EntityEntry<Store> entry)
        {
            await base.ModifyBeforeSaveAsync(entry);
            if (entry.State == Microsoft.EntityFrameworkCore.EntityState.Added)
            {
                var store = entry.Entity;
                store.RegistrationDate = DateTime.Now;
            }

        }
    }
}
