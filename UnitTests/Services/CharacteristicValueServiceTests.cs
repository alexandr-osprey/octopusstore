using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Specifications;
using System.Collections.Generic;
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
                new CharacteristicValue() { Title = "title 1",  CharacteristicId = _data.Characteristics.Fashion.Id },
                new CharacteristicValue() { Title = "title 2", CharacteristicId =  _data.Characteristics.Storage.Id },
            };
        }

        protected override IEnumerable<CharacteristicValue> GetIncorrectNewEntites()
        {
            return new List<CharacteristicValue>()
            {
                new CharacteristicValue() { Title = null, CharacteristicId = _data.Characteristics.Fashion.Id },
                new CharacteristicValue() { Title = "new2", CharacteristicId = 0 },
            };
        }

        protected override Specification<CharacteristicValue> GetEntitiesToDeleteSpecification()
        {
            return new EntitySpecification<CharacteristicValue>(c => c == _data.CharacteristicValues.GB16);
        }

        protected override IEnumerable<CharacteristicValue> GetCorrectEntitesForUpdate()
        {
            _data.CharacteristicValues.GB16.Title = "Updated storage";
            return new List<CharacteristicValue>() { _data.CharacteristicValues.GB16 };
        }

        protected override IEnumerable<CharacteristicValue> GetIncorrectEntitesForUpdate()
        {
            _data.CharacteristicValues.GB32.Title = null;
            return new List<CharacteristicValue>() { _data.CharacteristicValues.GB32 };
        }
    }
}
