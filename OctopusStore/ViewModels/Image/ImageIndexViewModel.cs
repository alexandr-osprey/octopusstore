using ApplicationCore.Entities;
using ApplicationCore.ViewModels;
using System.Collections.Generic;

namespace OctopusStore.ViewModels
{
    public abstract class ImageIndexViewModel<TImageDetails, TEntity, TViewModel>
        : FileDetailsIndexViewModel<TImageDetails, TEntity, TViewModel>
        where TImageDetails : FileDetails<TEntity>
        where TEntity : Entity
        where TViewModel: ViewModel<TImageDetails>
    {
        public ImageIndexViewModel(int page, int totalPages, int totalCount, IEnumerable<TViewModel> entities)
            : base(page, totalPages, totalCount, entities)
        { }
    }
}
