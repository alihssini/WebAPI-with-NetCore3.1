using Ali.Hosseini.Application.Domain.Validation;
using System;

namespace Ali.Hosseini.Application.Domain.Entity
{
    /// <summary>
    /// Base entity with framework properties ;-)
    /// </summary>
    public interface IEntity : IValidatable
    {
        //some framework props...[e.g : CreatedDate,CreatedUser,RowId,...]
    }
    /// <summary>
    /// Defines interface for base entity type.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity</typeparam>
    public interface IEntity<TPrimaryKey> : IEntity, IHasID<TPrimaryKey>
        where TPrimaryKey : IEquatable<TPrimaryKey>
    {
    }
}
