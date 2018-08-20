﻿using ApplicationCore.Entities;
using ApplicationCore.ViewModels;

namespace OctopusStore.ViewModels
{
    public class CharacteristicViewModel : ViewModel<Characteristic>
    {
        public string Title { get; set; }
        public int CategoryId { get; set; }

        public CharacteristicViewModel()
            : base()
        { }
        public CharacteristicViewModel(Characteristic categoryProperty)
            : base(categoryProperty)
        {
            Title = categoryProperty.Title;
            CategoryId = categoryProperty.CategoryId;
        }

        public override Characteristic ToModel()
        {
            return new Characteristic()
            {
                Id = Id,
                Title = Title,
                CategoryId = CategoryId
            };
        }
        public override void UpdateModel(Characteristic modelToUpdate)
        {
            modelToUpdate.Title = Title;
            modelToUpdate.CategoryId = CategoryId;
        }
    }
}
