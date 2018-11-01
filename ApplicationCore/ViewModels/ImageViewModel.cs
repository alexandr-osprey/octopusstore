using ApplicationCore.Entities;

namespace ApplicationCore.ViewModels
{
    public abstract class ImageViewModel<TImageDetail, TEntity>
       : FileInfoViewModel<TImageDetail, TEntity>
        where TImageDetail: FileInfo<TEntity>, new()
        where TEntity: Entity, new()
    {
        public ImageViewModel(): base()
        {
        }
        public ImageViewModel(TImageDetail image): base(image)
        {
        }
    }
}
