//using ApplicationCore.Entities;
//using ApplicationCore.ViewModels;
//using Microsoft.AspNetCore.Http;
//using Moq;
//using OctopusStore.Controllers;
//using System;
//using System.Collections.Generic;
//using System.Net;
//using System.Text;
//using System.Threading.Tasks;
//using Xunit;
//using Xunit.Abstractions;

//namespace UnitTests.Controllers
//{
//    public class CredentialsControllerTests : TestBase<Item>
//    {
//        private CredentialsController _credentialsController;

//        public CredentialsControllerTests(ITestOutputHelper output) : base(output)
//        {
//            _credentialsController = Resolve<CredentialsController>();

//            var authenticationManagerMock = new Mock<AuthenticationManager>();
//            var httpContextMock = new Mock<HttpContext>();
//            authenticationManagerMock.Setup(x => x.HttpContext.User.Identity.Name).Returns("Siddhartha");
//            httpContextMock.Setup(x => x.Authentication).Returns(authenticationManagerMock.Object);
//            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
//            httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContextMock.Object);
//            var httpContextMockObject = httpContextAccessorMock.Object;
//            services.AddScoped(x => httpContextAccessorMock);
//            services.AddScoped(x => httpContextMockObject);
//            serviceProvider = services.BuildServiceProvider();
//        }
//        [Fact]
//        public async Task CreateUser()
//        {
//            string email = "user99@mail.com";
//            string password = "Pass1@word";
//            var created = await _credentialsController.PostAsync(new Credentials() { Email = email, Password = password });
//            var user = await _userManager.FindByEmailAsync(email);
//            Assert.NotNull(user);
//        }
//        [Fact]
//        public async Task TooEasyPasswordFailToCreateUser()
//        {
//            string email = "user99@mail.com";
//            string password = "Password1";
//            var created = await _credentialsController.PostAsync(new Credentials() { Email = email, Password = password });
//            var user = await _userManager.FindByEmailAsync(email);
//            Assert.NotNull(user);
//        }
//    }
//}
