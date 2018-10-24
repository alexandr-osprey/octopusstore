using ApplicationCore.Entities;
using ApplicationCore.ViewModels;
using System.Collections.Generic;

namespace ApplicationCore.ViewModels
{
    public abstract class FileDetailsIndexViewModel<TFileDetails, TEntity, TViewModel> 
        : EntityIndexViewModel<TViewModel, TFileDetails> 
        where TFileDetails : FileInfo<TEntity>
        where TEntity : Entity
        where TViewModel: EntityViewModel<TFileDetails>
    {
        public FileDetailsIndexViewModel(int page, int totalPages, int totalCount, IEnumerable<TViewModel> entities)
            : base(page, totalPages, totalCount, entities)
        {
        }
    }
}
