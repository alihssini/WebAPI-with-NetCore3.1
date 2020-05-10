using System.Threading.Tasks;

namespace Ali.Hosseini.Application.Domain.Factory
{
    public interface IUpdateFactory<TAggregate> : IFactory<TAggregate>
        where TAggregate : IAggregateRoot
    {
        Task<TAggregate> UpdateAsync(TAggregate model);
    }
}
