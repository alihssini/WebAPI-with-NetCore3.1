using System;
using System.Threading.Tasks;

namespace Ali.Hosseini.Application.Domain.Factory
{
    public interface IDeleteFactory<TAggregate, TPrimaryKey> : IFactory<TAggregate>
        where TAggregate : IAggregateRoot<TPrimaryKey>
        where TPrimaryKey: IEquatable<TPrimaryKey>
    {
        Task<bool> DeleteAsync(TAggregate model) => DeleteAsync(model.ID);
        Task<bool> DeleteAsync(TPrimaryKey id);
    }
}
