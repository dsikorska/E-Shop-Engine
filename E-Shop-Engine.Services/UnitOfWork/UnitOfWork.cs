using System;
using E_Shop_Engine.Domain.Interfaces;
using E_Shop_Engine.Services.Data;

namespace E_Shop_Engine.Services
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }
        public int Commit()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }
        }
    }
}
