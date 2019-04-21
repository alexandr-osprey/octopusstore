namespace ApplicationCore.Interfaces.Services
{
    /// <summary>
    /// Service resolving instances of classes
    /// </summary>
    public interface IActivatorService
    {
        /// <summary>
        /// Resolve instance of a class in a generic way
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="constructorArguments"></param>
        /// <returns></returns>
        T GetInstance<T>(params object[] constructorArguments);
    }
}
