namespace ApplicationCore.Interfaces.Services
{
    /// <summary>
    /// Resolves instances of generic classes
    /// </summary>
    public interface IActivatorService
    {
        T GetInstance<T>(params object[] constructorArguments);
    }
}
