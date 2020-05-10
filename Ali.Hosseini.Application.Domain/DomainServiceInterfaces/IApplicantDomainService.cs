using Ali.Hosseini.Application.Domain.AggregatesModel.ApplicantAggregate;
using Ali.Hosseini.Application.Domain.Factory;

namespace Ali.Hosseini.Application.Domain.DomainServiceInterfaces
{
    public interface IApplicantDomainService : ICRUDFactory<Applicant, int>
    {
    }
}
