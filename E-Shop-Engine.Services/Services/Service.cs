using System;
using System.Linq;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.Interfaces;

namespace E_Shop_Engine.Services.Services
{
    public abstract class Service<T> : IService<T> where T : DbEntity
    {
        IUnitOfWork _unitOfWork;
        IRepository<T> _repository;

        public Service(IUnitOfWork unitOfWork, IRepository<T> repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public virtual void Create(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _repository.Create(entity);
            _unitOfWork.Commit();
        }

        public virtual void Delete(int id)
        {
            _repository.Delete(id);
            _unitOfWork.Commit();
        }

        public virtual IQueryable<T> GetAll()
        {
            return _repository.GetAll();
        }

        public virtual T GetById(int id)
        {
            return _repository.GetById(id);
        }

        public virtual void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _repository.Update(entity);
            _unitOfWork.Commit();
        }
    }
}
