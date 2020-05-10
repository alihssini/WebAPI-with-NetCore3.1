using Ali.Hosseini.Application.Domain.Factory;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ali.Hosseini.Application.Domain.Repository
{
    public abstract class BaseCRUDFactory<TAggregate, TPrimaryKey, TRepository>
         : BaseCRUDFactory<TAggregate, TPrimaryKey>
         where TAggregate : class, IAggregateRoot<TPrimaryKey>
        where TPrimaryKey : IEquatable<TPrimaryKey>
        where TRepository : IRepository<TAggregate, TPrimaryKey>
    {
        #region Ctor
        public BaseCRUDFactory(TRepository repository)
            : base(repository)
        {
        }
        #endregion
    }
    public abstract class BaseCRUDFactory<TAggregate, TPrimaryKey> : ICRUDFactory<TAggregate, TPrimaryKey>
    where TAggregate : class, IAggregateRoot<TPrimaryKey>
        where TPrimaryKey : IEquatable<TPrimaryKey>
    {
        #region Props
        public IValidator<TAggregate> Validator { get; set; }
        public IRepository<TAggregate, TPrimaryKey> Repository { set; get; }
        public ILogger<TAggregate> Logger { get; set; }//if we need to log every action in base crud ;-)
        #endregion
        #region Ctor
        public BaseCRUDFactory(IRepository<TAggregate, TPrimaryKey> repository) => Repository = repository;
        #endregion
        #region Impl
        public TAggregate Create()
            => Activator.CreateInstance<TAggregate>();


        public async Task<TAggregate> CreateAsync(TAggregate model)
        {
            if (Validator != null && !Validator.Validate(model).IsValid) throw new ArgumentException();//for example we can log this exceptions
            return await Repository.InsertAsync(model);
        }
        public async Task<bool> DeleteAsync(TPrimaryKey id)
            => await Repository.DeleteAsync(id);


        public async Task<TAggregate> GetAsync(TPrimaryKey id)
            => await Repository.GetAsync(id);


        public IQueryable<TAggregate> GetAll()
            => Repository.GetAll();


        public async Task<TAggregate> UpdateAsync(TAggregate model)
        {
            if (Validator != null && !Validator.Validate(model).IsValid) throw new ArgumentException();
            return await Repository.UpdateAsync(model);
        }
        #endregion
    }
}
