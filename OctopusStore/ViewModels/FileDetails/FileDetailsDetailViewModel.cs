using ApplicationCore.Entities;
using ApplicationCore.ViewModels;

namespace OctopusStore.ViewModels
{
    public abstract class FileDetailsDetailViewModel<TFileDetails, TEntity> 
        : DetailViewModel<TFileDetails> 
        where TFileDetails : FileInfo<TEntity>, new() 
        where TEntity : Entity, new()
    {
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

        public override void UpdateModel(TFileDetails modelToUpdate)
        {
            modelToUpdate.Title = Title;
            modelToUpdate.RelatedId = modelToUpdate.RelatedId;
        }
    }
}
