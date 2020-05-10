using FluentValidation.Results;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ali.Hosseini.Application.Domain.Validation
{
    public interface IValidatable
    {
        //[NotMapped]
        //IList<ValidationFailure> ValidationResult { get; set; }
    }
}
