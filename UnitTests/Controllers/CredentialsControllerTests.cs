using ApplicationCore.Entities;
using ApplicationCore.ViewModels;
using OctopusStore.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Controllers
{
    public class CredentialsControllerTests : TestBase<Item>
    {
        private CredentialsController _credentialsController;

        public CredentialsControllerTests(ITestOutputHelper output) : base(output)
        {
            _credentialsController = Resolve<CredentialsController>();
        }
        [Fact]
        public async Task CreateUser()
        {
            string email = "user99@mail.com";
            string password = "Pass1@word";
            var created = await _credentialsController.PostAsync(new Credentials() { Email = email, Password = password });
            var user = await _userManager.FindByEmailAsync(email);
            Assert.NotNull(user);
        }
    }
}
