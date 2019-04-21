using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Specifications;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Services
{
    public class CharacteristicServiceTests : ServiceTests<Characteristic, ICharacteristicService>
    {
        public CharacteristicServiceTests(ITestOutputHelper output)
           : base(output)
        {
        }

        protected override IEnumerable<Characteristic> GetCorrectNewEntites()
        {
            return new List<Characteristic>()
            {
                new Characteristic() { Title = "title 1", CategoryId = _data.Categories.WomensDresses.Id },
                new Characteristic() { Title = "title 2", CategoryId = _data.Categories.Smartphones.Id },
            };
        }

        protected override IEnumerable<Characteristic> GetIncorrectNewEntites()
        {
            return new List<Characteristic>()
            {
                new Characteristic() { Title = null, CategoryId = _data.Categories.WomensDresses.Id },
                new Characteristic() { Title = "new2", CategoryId = 0 },
            };
        }

        protected override Specification<Characteristic> GetEntitiesToDeleteSpecification()
        {
            return new EntitySpecification<Characteristic>(c => c == _data.Characteristics.SmartphoneRAM);
        }

        protected override IEnumerable<Characteristic> GetCorrectEntitesForUpdate()
        {
            _data.Characteristics.SmartphoneBattery.Title = "Updated storage";
            return new List<Characteristic>() { _data.Characteristics.SmartphoneBattery };
        }

        protected override IEnumerable<Characteristic> GetIncorrectEntitesForUpdate()
        {
            _data.Characteristics.WomenFootwearSize.Title = "";
            _data.Characteristics.SmartphoneColor.CategoryId = _data.Categories.Electronics.Id;
            return new List<Characteristic>()
            {
                _data.Characteristics.WomenFootwearSize,
                _data.Characteristics.SmartphoneColor
            };
        }
    }
}
