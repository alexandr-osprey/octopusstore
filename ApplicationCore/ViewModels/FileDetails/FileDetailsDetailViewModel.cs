using ApplicationCore.Entities;

namespace ApplicationCore.ViewModels
{
    public abstract class FileDetailsDetailViewModel<TFileDetails, TEntity> 
        : EntityDetailViewModel<TFileDetails> 
        where TFileDetails : FileInfo<TEntity>, new() 
        where TEntity : Entity, new()
    {
        public string Title { get; set; }
        public string ContentType { get; set; }
        public int RelatedId { get; set; }
        public string OwnerUsername { get; set; }

        public FileDetailsDetailViewModel(TFileDetails fileDetail)
            : base(fileDetail)
        {
            Title = Title;
            ContentType = fileDetail.ContentType;
            OwnerUsername = fileDetail.OwnerId;
            RelatedId = fileDetail.RelatedId;
        }

        public override TFileDetails UpdateModel(TFileDetails modelToUpdate)
        {
            modelToUpdate.Title = Title;
            modelToUpdate.RelatedId = modelToUpdate.RelatedId;
            return modelToUpdate;
        }
    }
}
