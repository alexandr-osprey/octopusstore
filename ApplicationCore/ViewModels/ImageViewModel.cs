using ApplicationCore.Entities;

namespace ApplicationCore.ViewModels
{
    public abstract class ImageViewModel<TImageDetails, TEntity>
       : FileInfoViewModel<TImageDetails, TEntity>
        where TImageDetails: FileInfo<TEntity>, new()
        where TEntity: Entity, new()
    {
        public ImageViewModel(): base()
        {
        }
        public ImageViewModel(TImageDetails image): base(image)
        {
        }
    }
}
