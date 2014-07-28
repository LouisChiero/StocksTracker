namespace Core.Framework.API.Support
{
    /// <summary>
    /// Generic object capable of creating other objects.
    /// </summary>
    /// <typeparam name="T">The type of object being created.</typeparam>
    public interface IObjectFactory<out T>
    {
        /// <summary>
        /// Gets an instance of an object.
        /// </summary>
        /// <returns>The object instance.</returns>
        T GetObject();
    }
}
