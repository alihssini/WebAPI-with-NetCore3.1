using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ali.Hosseini.Application.Domain.Entity
{
    /// <summary>
    /// Base Entity calss for handle Validation and key properties
    /// </summary>
    public class BaseEntity<TPrimaryKey> : IEntity<TPrimaryKey> where TPrimaryKey : IEquatable<TPrimaryKey>
    {
        #region Ctor
        //public BaseEntity() => ValidationResult = new List<ValidationFailure>();
        #endregion
        #region Props
        public TPrimaryKey ID { get; set; }
        //[NotMapped]
        //public IList<ValidationFailure> ValidationResult { get; set; }
        #endregion
    }
}
