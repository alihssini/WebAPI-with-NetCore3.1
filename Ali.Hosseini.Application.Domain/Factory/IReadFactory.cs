using System.Linq;
using System.Threading.Tasks;

namespace Ali.Hosseini.Application.Domain.Factory
{
    public interface IReadFactory<TAggregate, TPrimaryKey> : IFactory<TAggregate>
        where TAggregate : IAggregateRoot
    {
        IQueryable<TAggregate> GetAll();
        Task<TAggregate> GetAsync(TPrimaryKey id);
    }
}
