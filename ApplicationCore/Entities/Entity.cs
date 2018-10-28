namespace ApplicationCore.Entities
{
    /// <summary>
    /// Abstract entity with Id and OwnerId. Has Equals method overriden for comparison (and therefore searching) by Id, ToString overridden for verbose logging.
    /// </summary>
    public abstract class Entity
    {
        public int Id { get; set; }

        public Entity()
        {
        }
        public Entity(Entity entity)
        {
            Id = entity.Id;
            OwnerId = entity.OwnerId;
        }

        public override string ToString() => (new { Type = GetType().Name, Id }).ToString();
        public bool Equals(Entity other) => null != other && Id == other.Id;
        public override bool Equals(object obj) => Equals(obj as Entity);
        public override int GetHashCode() => Id;
        /// <summary>
        /// Id of a user who created this Entity. May be used for authorization.
        /// </summary>
        public virtual string OwnerId { get; set; }
    }
}