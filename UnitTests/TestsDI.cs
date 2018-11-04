//using Infrastructure.Data;
//using Infrastructure.Identity;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using Xunit;

//namespace UnitTests
//{
//    public class TestsDI
//    {
//        protected static ServiceCollection Services { get; } = new ServiceCollection();
//        protected static ServiceProvider ServiceProvider { get; } = Services.BuildServiceProvider();
//        protected static DbContextOptions<StoreContext> StoreContextOptions { get; } =
//            new DbContextOptionsBuilder<StoreContext>()
//                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
//        protected static DbContextOptions<AppIdentityDbContext> IdentityContextOptions { get; } =
//            new DbContextOptionsBuilder<AppIdentityDbContext>()
//                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
//        //protected static ServiceProvider ServiceProvider = Services.BuildServiceProvider();


//        public TestsDI()
//        {
//            Services.AddSingleton(StoreContextOptions);
//            Services.AddDbContext<StoreContext>();
//            Services.AddDbContext<AppIdentityDbContext>();
//            Services.AddSingleton(IdentityContextOptions);
//            Services.AddIdentity<ApplicationUser, IdentityRole>()
//                .AddEntityFrameworkStores<AppIdentityDbContext>()
//                .AddDefaultTokenProviders();
//            Services.AddScoped<UserManager<ApplicationUser>, UserManager<ApplicationUser>>();
//        }

//        [Fact]
//        public void Test1()
//        {
            
//            var sp = Services.BuildServiceProvider();
//            var context1 = sp.GetRequiredService<StoreContext>();
//            var context2 = sp.GetRequiredService<StoreContext>();
           
//            var um = sp.GetRequiredService<UserManager<ApplicationUser>>();
//            Assert.True(context1 == context2);
//        }

//        [Fact]
//        public void Test12()
//        {
//            var context1 = ServiceProvider.GetRequiredService<StoreContext>();
//            var context2 = ServiceProvider.GetRequiredService<StoreContext>();

//            var um = ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
//            Assert.True(context1 == context2);
//        }

//        [Fact]
//        public void Test2()
//        {
//            Services.AddSingleton(StoreContextOptions);
//            Services.AddDbContext<DbContext>();

//            var sp1 = Services.BuildServiceProvider();
//            var context1 = sp1.GetRequiredService<DbContext>();
//            var um1 = sp1.GetRequiredService<UserManager<ApplicationUser>>();

//            var sp2 = Services.BuildServiceProvider();
//            var context2 = sp2.GetRequiredService<DbContext>();
//            var um2 = sp2.GetRequiredService<UserManager<ApplicationUser>>();

//            Assert.True(context1 != context2);
//        }
//    }
//}
