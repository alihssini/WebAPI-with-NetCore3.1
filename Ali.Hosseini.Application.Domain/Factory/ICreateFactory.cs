using System.Threading.Tasks;

namespace Ali.Hosseini.Application.Domain.Factory
{
    public interface ICreateFactory<TAggregate> : IFactory<TAggregate>
        where TAggregate : IAggregateRoot
    {
        /// <summary>
        /// Create new instance
        /// </summary>
        /// <returns></returns>
        TAggregate Create();
        /// <summary>
        /// Save model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<TAggregate> CreateAsync(TAggregate model);
    }
}
