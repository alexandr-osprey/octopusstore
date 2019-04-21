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
    public class CharacteristicValueServiceTests : ServiceTests<CharacteristicValue, ICharacteristicValueService>
    {
        public CharacteristicValueServiceTests(ITestOutputHelper output)
           : base(output)
        {
        }

        protected override IEnumerable<CharacteristicValue> GetCorrectNewEntites()
        {
            return new List<CharacteristicValue>()
            {
                new CharacteristicValue() { Title = "title 1",  CharacteristicId = _data.Characteristics.SmartphoneBattery.Id },
                new CharacteristicValue() { Title = "title 2", CharacteristicId =  _data.Characteristics.SmartphoneColor.Id },
            };
        }

        protected override IEnumerable<CharacteristicValue> GetIncorrectNewEntites()
        {
            return new List<CharacteristicValue>()
            {
                new CharacteristicValue() { Title = null, CharacteristicId = _data.Characteristics.SmartphoneBattery.Id },
                new CharacteristicValue() { Title = "new2", CharacteristicId = 0 },
            };
        }

        protected override Specification<CharacteristicValue> GetEntitiesToDeleteSpecification()
        {
            return new EntitySpecification<CharacteristicValue>(c => c == _data.CharacteristicValues.WomensDressColorWhite);
        }

        protected override IEnumerable<CharacteristicValue> GetCorrectEntitesForUpdate()
        {
            _data.CharacteristicValues.SmartphoneStorage64GB.Title = "UNLIMITED STORAGE!!!111";
            return new List<CharacteristicValue>() { _data.CharacteristicValues.SmartphoneStorage64GB };
        }

        protected override IEnumerable<CharacteristicValue> GetIncorrectEntitesForUpdate()
        {
            _data.CharacteristicValues.WomensFootwearColorBlack.Title = null;
            _data.CharacteristicValues.SmartphoneResolutionFullHD.CharacteristicId = _data.Characteristics.WomenFootwearSize.Id;
            return new List<CharacteristicValue>()
            {
                _data.CharacteristicValues.WomensFootwearColorBlack,
                _data.CharacteristicValues.SmartphoneResolutionFullHD
            };
        }
    }
}
