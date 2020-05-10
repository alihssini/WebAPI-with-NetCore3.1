namespace Ali.Hosseini.Application.Domain.Factory
{
    public interface IFactory
    {
    }
    public interface IFactory<TAggregate> : IFactory where TAggregate : IAggregateRoot
    {
    }
}
