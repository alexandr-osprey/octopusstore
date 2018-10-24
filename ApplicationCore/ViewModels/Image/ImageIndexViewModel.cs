using ApplicationCore.Entities;
using ApplicationCore.ViewModels;
using System.Collections.Generic;

namespace ApplicationCore.ViewModels
{
    public abstract class ImageIndexViewModel<TImageDetails, TEntity, TViewModel>
        : FileDetailsIndexViewModel<TImageDetails, TEntity, TViewModel>
        where TImageDetails : FileInfo<TEntity>
        where TEntity : Entity
        where TViewModel: EntityViewModel<TImageDetails>
    {
        public ImageIndexViewModel(int page, int totalPages, int totalCount, IEnumerable<TViewModel> entities)
            : base(page, totalPages, totalCount, entities)
        {
        }
    }
}
