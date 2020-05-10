using Ali.Hosseini.Application.Domain.AggregatesModel.ApplicantAggregate;
using Ali.Hosseini.Application.Domain.DomainServiceInterfaces;
using Ali.Hosseini.Application.Domain.Repository;
using FluentValidation;

namespace Ali.Hosseini.Application.Domain.DomainService
{
    public class ApplicantDomainService : BaseCRUDFactory<Applicant, int, IApplicantRepository>, IApplicantDomainService
    {
        #region Ctor
        public ApplicantDomainService(IApplicantRepository repository)
            : base(repository)
            => Validator = ServiceProviderHandler.GetService<IValidator<Applicant>>();
        #endregion
    }
}
