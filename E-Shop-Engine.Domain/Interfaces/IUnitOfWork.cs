using System;

namespace E_Shop_Engine.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUnitOfWork NewUnitOfWork();
        void SaveChanges();
    }
}
