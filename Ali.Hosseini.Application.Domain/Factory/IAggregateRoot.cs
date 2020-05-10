using Ali.Hosseini.Application.Domain.Entity;
using System;

namespace Ali.Hosseini.Application.Domain.Factory
{
    public interface IAggregateRoot : IEntity
    {
        //may be some props for tenants or ...
    }
    public interface IAggregateRoot<TprimaryKey> : IAggregateRoot, IEntity<TprimaryKey>
        where TprimaryKey : IEquatable<TprimaryKey>
    {
    }
}
