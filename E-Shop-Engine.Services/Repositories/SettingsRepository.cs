using System.Data.Entity;
using System.Linq;
using E_Shop_Engine.Domain.Abstract;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Services.Data;

namespace E_Shop_Engine.Services.Repositories
{
    public sealed class SettingsRepository : ISettingsRepository
    {
        private AppDbContext _context;
        private IDbSet<Settings> _dbSet;

        public SettingsRepository(IAppDbContext context)
        {
            _context = context as AppDbContext;
            _dbSet = _context.Settings;
        }

        /// <summary>
        /// Get instance of Settings.
        /// </summary>
        /// <returns>Settings.</returns>
        public Settings Get()
        {
            return _dbSet.FirstOrDefault();
        }

        /// <summary>
        /// Update Settings.
        /// </summary>
        /// <param name="entity">Entity.</param>
        public void Update(Settings entity)
        {
            _context.Entry<Settings>(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
