using System;

namespace E_Shop_Engine.Domain.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Save changes to database.
        /// </summary>
        void SaveChanges();
    }
}
