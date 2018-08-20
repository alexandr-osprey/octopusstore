using ApplicationCore.Entities;
using ApplicationCore.ViewModels;
using System.Collections.Generic;

namespace OctopusStore.ViewModels
{
    public abstract class FileDetailsIndexViewModel<TFileDetails, TEntity, TViewModel> 
        : IndexViewModel<TViewModel, TFileDetails> 
        where TFileDetails : FileDetails<TEntity>
        where TEntity : Entity
        where TViewModel: ViewModel<TFileDetails>
    {
        public FileDetailsIndexViewModel(int page, int totalPages, int totalCount, IEnumerable<TViewModel> entities)
            : base(page, totalPages, totalCount, entities)
        {  }
    }
}
