using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OctopusStore.Controllers;
using ApplicationCore.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Controllers
{
    public class BrandControllerTests: ControllerTestBase<Brand, BrandsController, IBrandService>
    {
        public BrandControllerTests(ITestOutputHelper output): base(output)
        {
        }
    }
}
