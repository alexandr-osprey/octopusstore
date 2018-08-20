using ApplicationCore.Entities;

namespace OctopusStore.ViewModels
{
    public abstract class ImageViewModel<TImageDetails, TEntity>
        : FileDetailsViewModel<TImageDetails, TEntity>
        where TImageDetails : FileInfo<TEntity>, new()
        where TEntity : Entity, new()
    {
        public ImageViewModel()
            : base()
        { }
        public ImageViewModel(TImageDetails image)
            : base(image)
        { }
    }
}
