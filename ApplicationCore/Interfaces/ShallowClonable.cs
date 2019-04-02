namespace ApplicationCore.Interfaces
{
    /// <summary>
    /// Generic interface for shallow cloning objects
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ShallowClonable<T> where T: class
    {
        /// <summary>
        /// Shallow clones object
        /// </summary>
        /// <returns></returns>
        T ShallowClone();
    }
}
