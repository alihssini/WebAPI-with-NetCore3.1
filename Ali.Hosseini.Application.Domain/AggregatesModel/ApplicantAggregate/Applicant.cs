using Ali.Hosseini.Application.Domain.Entity;
using Ali.Hosseini.Application.Domain.Factory;

namespace Ali.Hosseini.Application.Domain.AggregatesModel.ApplicantAggregate
{
    public class Applicant : BaseEntity<int>, IAggregateRoot<int>
    {
        public string Name { get; set; }
        public string FamilyName { get; set; }
        public string Address { get; set; }
        public string CountryOfOrigin { get; set; }
        public string EMailAddress { get; set; }
        public int Age { get; set; }
        public bool Hired { get; set; } = false;
    }
}
