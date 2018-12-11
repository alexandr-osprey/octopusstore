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
                new CharacteristicValue() { Title = "title 1",  CharacteristicId = Data.Characteristics.Fashion.Id },
                new CharacteristicValue() { Title = "title 2", CharacteristicId =  Data.Characteristics.Storage.Id },
            };
        }

        protected override IEnumerable<CharacteristicValue> GetIncorrectNewEntites()
        {
            return new List<CharacteristicValue>()
            {
                new CharacteristicValue() { Title = null, CharacteristicId = Data.Characteristics.Fashion.Id },
                new CharacteristicValue() { Title = "new2", CharacteristicId = 0 },
            };
        }

        [Fact]
        public async Task DeleteSingleWithRelatedRelinkAsync()
        {
            var entity = Data.CharacteristicValues.GB32;
            int idToRelinkTo = Data.CharacteristicValues.GB16.Id;
            var characteristicValues = Data.ItemProperties.Entities.Where(i => i.CharacteristicValue == entity).ToList();
            await Service.DeleteSingleWithRelatedRelink(entity.Id, idToRelinkTo);
            characteristicValues.ForEach(i => Assert.Equal(i.CharacteristicValueId, idToRelinkTo));
            Assert.False(Context.Set<CharacteristicValue>().Any(c => c == entity));
        }

        protected override Specification<CharacteristicValue> GetEntitiesToDeleteSpecification()
        {
            return new EntitySpecification<CharacteristicValue>(c => c == Data.CharacteristicValues.GB16);
        }

        protected override IEnumerable<CharacteristicValue> GetCorrectEntitesForUpdate()
        {
            Data.CharacteristicValues.GB16.Title = "Updated storage";
            return new List<CharacteristicValue>() { Data.CharacteristicValues.GB16 };
        }

        protected override IEnumerable<CharacteristicValue> GetIncorrectEntitesForUpdate()
        {
            Data.CharacteristicValues.GB32.Title = null;
            Data.CharacteristicValues.FullHD.CharacteristicId = Data.Characteristics.Size.Id;
            return new List<CharacteristicValue>()
            {
                Data.CharacteristicValues.GB32,
                Data.CharacteristicValues.FullHD
            };
        }
    }
}
