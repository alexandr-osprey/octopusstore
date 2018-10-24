namespace ApplicationCore.Entities
{
    /// <summary>
    /// Value of characteristic
    /// </summary>
    public class CharacteristicValue : Entity
    {
        public string Title { get; set; }
        public int CharacteristicId { get; set; }

        public Characteristic Characteristic { get; set; }
    }
}
