using ApplicationCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Identity
{
    /// <summary>
    /// DbContext used for storing Application Users
    /// </summary>
    public class AppIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        

        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<RefreshToken>(ConfigureTokenPair);
            
        }
        private void ConfigureTokenPair(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(t => t.Token);
            builder.Property(t => t.Token).IsRequired();
            builder.Property(t => t.OwnerId).IsRequired();
        }
        
    }
}
