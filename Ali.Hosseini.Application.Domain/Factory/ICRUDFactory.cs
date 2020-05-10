using FluentValidation;
using System;

namespace Ali.Hosseini.Application.Domain.Factory
{
    public interface ICRUDFactory<TAggregate, TPrimaryKey> : IFactory<TAggregate>, ICreateFactory<TAggregate>,
        IReadFactory<TAggregate, TPrimaryKey>, IUpdateFactory<TAggregate>, IDeleteFactory<TAggregate, TPrimaryKey>
        where TAggregate : IAggregateRoot<TPrimaryKey>
        where TPrimaryKey : IEquatable<TPrimaryKey>
    {
        IValidator<TAggregate> Validator { get; set; }
    }
}
