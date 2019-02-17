using E_Shop_Engine.Domain.DomainModel;

namespace E_Shop_Engine.Domain.Abstract
{
    public interface ISettingsRepository
    {
        /// <summary>
        /// Get instance of Settings.
        /// </summary>
        /// <returns>Settings.</returns>
        Settings Get();

        /// <summary>
        /// Update Settings.
        /// </summary>
        /// <param name="entity">Entity.</param>
        void Update(Settings entity);
    }
}
