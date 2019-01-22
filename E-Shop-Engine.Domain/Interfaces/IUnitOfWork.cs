using System;

namespace E_Shop_Engine.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Save changes to database.
        /// </summary>
        void SaveChanges();
    }
}
