using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ApplicationCore.Entities;
using System.Linq;

namespace Infrastructure.Data
{

    public class StoreContext: DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options): base(options)
        {
        }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<MeasurementUnit> MeasurementUnits { get; set; }
        public DbSet<Characteristic> Characteristics { get; set; }
        public DbSet<CharacteristicValue> CharacteristicValues { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemImage> ItemImages { get; set; }
        public DbSet<ItemVariant> ItemVariants { get; set; }
        public DbSet<ItemProperty> ItemProperties { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }

        public void DetachAllEntities()
        {
            var changedEntriesCopy = this.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;
        }
        //public DbSet<StoreAdministrator> StoreAdministrators { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Brand>(ConfigureBrand);
            builder.Entity<Category>(ConfigureCategory);
            builder.Entity<MeasurementUnit>(ConfigureMeasurementUnit);
            builder.Entity<Characteristic>(ConfigureCharacteristic);
            builder.Entity<CharacteristicValue>(ConfigureCharacteristicValue);
            builder.Entity<Store>(ConfigureStore);
            builder.Entity<Item>(ConfigureItem);
            builder.Entity<ItemImage>(ConfigureItemImage);
            builder.Entity<ItemVariant>(ConfigureItemVariant);
            builder.Entity<ItemProperty>(ConfigureItemProperty);
            builder.Entity<Order>(ConfigureOrder);
            //builder.Entity<StoreAdministrator>(ConfigureStoreAdministrator);
        }
        private void ConfigureBrand(EntityTypeBuilder<Brand> builder)
        {
            builder.Property(t => t.Title).IsRequired();
            builder.ToTable(nameof(Brand));
            builder.HasKey(ci => ci.Id);
            builder.Property(cb => cb.Title)
                .IsRequired()
                .HasMaxLength(100);
        }
        private void ConfigureCategory(EntityTypeBuilder<Category> builder)
        {
            builder.Property(t => t.Title).IsRequired();
            builder.ToTable(nameof(Category));
            builder.HasKey(ci => ci.Id);
            builder.Property(cb => cb.Title)
                .IsRequired()
                .HasMaxLength(100);
        }
        private void ConfigureMeasurementUnit(EntityTypeBuilder<MeasurementUnit> builder)
        {
            builder.Property(t => t.Title).IsRequired();
            builder.ToTable(nameof(MeasurementUnit));
            builder.HasKey(ci => ci.Id);
            builder.Property(cb => cb.Title)
                .IsRequired()
                .HasMaxLength(100);
        }
        private void ConfigureCharacteristic(EntityTypeBuilder<Characteristic> builder)
        {
            builder.Property(t => t.Title).IsRequired();
            builder.ToTable(nameof(Characteristic));
        }
        private void ConfigureCharacteristicValue(EntityTypeBuilder<CharacteristicValue> builder)
        {
            builder.Property(t => t.Title).IsRequired();
            builder.ToTable(nameof(CharacteristicValue));
        }
        private void ConfigureStore(EntityTypeBuilder<Store> builder)
        {
            builder.Property(t => t.Title).IsRequired();
            builder.ToTable(nameof(Store));
            builder.HasKey(ci => ci.Id);
            builder.Property(cb => cb.Title)
                .IsRequired()
                .HasMaxLength(100);
        }
        private void ConfigureItem(EntityTypeBuilder<Item> builder)
        {
            builder.Property(t => t.Title).IsRequired();
            builder.ToTable(nameof(Item));
            builder.Property(ci => ci.Title)
                .IsRequired(true)
                .HasMaxLength(50);
            //builder.HasOne(ci => ci.MeasurementUnit)
            //    .WithMany()
            //    .HasForeignKey(ci => ci.MeasurementUnitId);
            //builder.HasOne(ci => ci.Brand)
            //    .WithMany()
            //    .HasForeignKey(ci => ci.BrandId);
            //builder.HasOne(ci => ci.Category)
            //    .WithMany()
            //    .HasForeignKey(ci => ci.CategoryId);
            //builder.HasOne(ci => ci.Store)
            //    .WithMany()
            //    .HasForeignKey(ci => ci.StoreId);
            builder.HasMany(i => i.Images)
                .WithOne(i => i.RelatedEntity)
                .HasForeignKey(i => i.RelatedId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);
            //builder.Property(i => i.In)
        }
        private void ConfigureItemImage(EntityTypeBuilder<ItemImage> builder)
        {
            builder.Property(t => t.Title).IsRequired(true);
            builder.Property(t => t.OwnerId).IsRequired(true);
            builder.ToTable(nameof(ItemImage));
        }
        private void ConfigureItemVariant(EntityTypeBuilder<ItemVariant> builder)
        {
            builder.Property(t => t.Title).IsRequired();
            builder.ToTable(nameof(ItemVariant));
        }
        private void ConfigureItemProperty(EntityTypeBuilder<ItemProperty> builder)
        {
            builder.ToTable(nameof(ItemProperty));
        }
        private void ConfigureOrder(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable(nameof(Order));
        }
    }
}
