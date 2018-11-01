using ApplicationCore.Entities;

namespace ApplicationCore.ViewModels
{
    public abstract class FileInfoViewModel<TFileInfo, TEntity> 
       : EntityViewModel<TFileInfo> 
        where TFileInfo: FileInfo<TEntity>, new() 
        where TEntity: Entity, new()
    {
        public string Title { get; set; }
        public string ContentType { get; set; }
        public int RelatedId { get; set; }

        public FileInfoViewModel(): base()
        {
        }
        public FileInfoViewModel(TFileInfo fileDetail): base(fileDetail)
        {
            Title = fileDetail.Title;
            ContentType = fileDetail.ContentType;
            RelatedId = fileDetail.RelatedId;
        }

        public override TFileInfo UpdateModel(TFileInfo modelToUpdate)
        {
            modelToUpdate.Title = Title;
            //modelToUpdate.RelatedId = modelToUpdate.RelatedId;
            return modelToUpdate;
        }
    }
}
