namespace ApplicationCore.Entities
{
    public abstract class Entity
    {
        public int Id { get; set; }

        public Entity()
        {  }
        public Entity(Entity entity)
        {
            Id = entity.Id;
        }

        public override string ToString()
        {
            return (new { Type = GetType().Name, Id }).ToString();
        }

        public bool Equals(Entity other)
        {
            return null != other && Id == other.Id;
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as Entity);
        }
        public override int GetHashCode()
        {
            return Id;
        }
    }
}
