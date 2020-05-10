using Ali.Hosseini.Application.Data.DBContext;
using Ali.Hosseini.Application.Data.Repository;
using Ali.Hosseini.Application.Domain.AggregatesModel.ApplicantAggregate;
using Ali.Hosseini.Application.Tests.Core;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Ali.Hosseini.Application.Tests
{
    public class ApplicantRepositoryUnitTests : TestBase
    {
        AppDbContext context;
        public ApplicantRepositoryUnitTests() => context = GetDbContext();

        [Fact]
        public async Task RepositoryInsertAndGetAllDbContext()
        {
            // Insert seed data into the database using one instance of the context

            context.Applicants.RemoveRange(context.Applicants);
            await context.SaveChangesAsync();
            context.Applicants.Add(new Applicant { Name = "Ali_1", FamilyName = "Hosseini1", Address = "Address_1", CountryOfOrigin = "Germany", Age = 35, EMailAddress = "ali.hssini@gmail.com", Hired = false });
            context.Applicants.Add(new Applicant { Name = "Ali_2", FamilyName = "Hosseini_2", Address = "Address_2", CountryOfOrigin = "Aruba", Age = 30, EMailAddress = "ali.hssini@gmail.com", Hired = false });
            context.Applicants.Add(new Applicant { Name = "Ali_3", FamilyName = "Hosseini_3", Address = "Address_3", CountryOfOrigin = "Poland", Age = 25, EMailAddress = "ali.hssini@gmail.com", Hired = false });
            await context.SaveChangesAsync();


            // Use a clean instance of the context to run the test
            using (var newContext = GetDbContext())
            {
                IApplicantRepository applicantRepository = new ApplicantRepository(newContext);
                List<Applicant> applicants = applicantRepository.GetAll().ToList();

                Assert.Equal(3, applicants.Count);
            }
        }
        [Theory]
        [InlineData(1, "Ali_1", 25)]
        [InlineData(2, "Ali_2", 30)]
        public async Task RepositoryGetByIdAndCheckData(int index, string expectedName, int expectedAge)
        {
            var obj = new Applicant { Name = $"Ali_{index}", Age = 20 + (index * 5) };
            context.Applicants.Add(obj);
            await context.SaveChangesAsync();

            IApplicantRepository applicantRepository = new ApplicantRepository(context);
            Applicant applicant = await applicantRepository.GetAsync(obj.ID);

            Assert.Equal(expectedName, applicant.Name);
            Assert.Equal(expectedAge, applicant.Age);
        }
        [Theory]
        [InlineData("Ali_1", "Ali_11")]
        [InlineData("Ali_2", "Ali_22")]
        public async Task RepositoryUpdateAndCheckResult(string name, string expectedUpdatedName)
        {
            //add initial record
            var obj = new Applicant { Name = name };
            context.Applicants.Add(obj);
            await context.SaveChangesAsync();

            IApplicantRepository applicantRepository = new ApplicantRepository(context);
            Applicant applicant = await applicantRepository.GetAsync(obj.ID);
            //update with new value
            applicant.Name = expectedUpdatedName;
            await applicantRepository.UpdateAsync(applicant);
            //get and check updated record
            Applicant updatedApplicant = await applicantRepository.GetAsync(obj.ID);
            Assert.Equal(expectedUpdatedName, applicant.Name);

        }
        [Fact]
        public async Task RepositoryDeleteAndCheckResult()
        {
            //add initial record
            var applicant = new Applicant { Name = "Ali" };
            context.Applicants.Add(applicant);
          await  context.SaveChangesAsync();

            IApplicantRepository applicantRepository = new ApplicantRepository(context);
            var result = await applicantRepository.DeleteAsync(applicant.ID);

            //get and check deleted record
            Applicant deletedApplicant = await applicantRepository.GetAsync(applicant.ID);
            Assert.True(result);
            Assert.Null(deletedApplicant);

        }
    }
}
