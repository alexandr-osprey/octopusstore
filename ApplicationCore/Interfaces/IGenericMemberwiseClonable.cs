namespace ApplicationCore.Interfaces
{
    /// <summary>
    /// Generic interface for shallow cloning objects
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGenericMemberwiseClonable<T> where T: class
    {
        /// <summary>
        /// Wraps Object's MemberwiseClone into a typed function
        /// </summary>
        /// <returns></returns>
        T ShallowClone();
    }
}
