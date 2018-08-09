using System;

namespace E_Shop_Engine.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Saves pending changes.
        /// </summary>
        /// <returns>The number of objects in an added, modified or deleted state.</returns>
        int Commit();
    }
}
