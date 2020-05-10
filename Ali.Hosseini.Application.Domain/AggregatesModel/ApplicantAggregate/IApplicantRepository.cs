using Ali.Hosseini.Application.Domain.Repository;

namespace Ali.Hosseini.Application.Domain.AggregatesModel.ApplicantAggregate
{
    public interface IApplicantRepository: IRepository<Applicant,int>
    {
    }
}
