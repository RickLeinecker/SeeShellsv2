using System;

namespace SeeShellsV2.Factories
{
    /// <summary>
    /// Used for constructing objects of type T
    /// </summary>
    /// <typeparam name="T">The type of object produced by the factory</typeparam>
    public interface IAbstractFactory<T>
    {
        /// <summary>
        /// Create an instance of T
        /// </summary>
        /// <param name="subtype">name of specific subtype of T to create</param>
        /// <returns>a new instance of T</returns>
        T Create(string subtype = null);
    }
}
