namespace MicroZoo.AnimalsApi.Apis
{
    /// <summary>
    /// Obsolete interface
    /// </summary>
    [Obsolete]
    public interface IApi
    {
        /// <summary>
        /// Registers all apis
        /// </summary>
        /// <param name="app"></param>
        void Register(WebApplication app);
    }
}
