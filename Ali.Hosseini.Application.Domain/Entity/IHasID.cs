using System;
using System.ComponentModel.DataAnnotations;

namespace Ali.Hosseini.Application.Domain.Entity
{
    /// <summary>
    /// Base Entity Key interface
    /// </summary>
    /// <typeparam name="TKeyType"></typeparam>
    public interface IHasID<TKeyType>
         where TKeyType : IEquatable<TKeyType>
    {
        [Key]
        TKeyType ID { set; get; }
    }
}
