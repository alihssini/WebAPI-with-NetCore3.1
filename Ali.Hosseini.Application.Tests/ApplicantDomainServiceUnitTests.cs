using Ali.Hosseini.Application.Data.DBContext;
using Ali.Hosseini.Application.Domain.AggregatesModel.ApplicantAggregate;
using Ali.Hosseini.Application.Domain.DomainServiceInterfaces;
using Ali.Hosseini.Application.Tests.Core;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Ali.Hosseini.Application.Tests
{
    public class ApplicantDomainServiceUnitTests : TestBase
    {
        #region Create
        [Fact]
        public async Task CreateValidApplicant()
        {
            IApplicantDomainService service = GetService<IApplicantDomainService>();
            var applicant = new Applicant { Name = "Ali_1", FamilyName = "Hosseini", Address = "Address test", Age = 35, CountryOfOrigin = "Aruba", EMailAddress = "ali.hssini@gmail.com", Hired = true };
            var result = await service.CreateAsync(applicant);
            Assert.NotNull(result);
            Assert.NotEqual(0, result.ID);
        }
        [Fact]
        public async Task CreateValidApplicantWithoutHired_CheckHired()
        {
            IApplicantDomainService service = GetService<IApplicantDomainService>();
            var applicant = new Applicant { Name = "Ali_1", FamilyName = "Hosseini", Address = "Address test", Age = 35, CountryOfOrigin = "Aruba", EMailAddress = "ali.hssini@gmail.com" };
            var result = await service.CreateAsync(applicant);
            Assert.NotNull(result);
            Assert.NotEqual(0, result.ID);
            var resultData = service.GetAsync(1);
            Assert.NotNull(result);
            Assert.False(result.Hired);
        }
        [Fact]
        public async Task CreateApplicant_InvalidName()
        {
            IApplicantDomainService service = GetService<IApplicantDomainService>();
            var applicant = new Applicant { Name = "Ali", FamilyName = "Hosseini", Address = "Address test", Age = 35, CountryOfOrigin = "Aruba", EMailAddress = "ali.hssini@gmail.com" };
            await Assert.ThrowsAsync<ArgumentException>(async () => await service.CreateAsync(applicant));
        }
        [Fact]
        public async Task CreateInvalidApplicant_InvalidFamily()
        {
            IApplicantDomainService service = GetService<IApplicantDomainService>();
            var applicant = new Applicant { Name = "Ali_1", FamilyName = "test", Address = "Address test", Age = 35, CountryOfOrigin = "Aruba", EMailAddress = "ali.hssini@gmail.com" };
            await Assert.ThrowsAsync<ArgumentException>(async () => await service.CreateAsync(applicant));
        }
        [Fact]
        public async Task CreateInvalidApplicant_InvalidEmail()
        {
            IApplicantDomainService service = GetService<IApplicantDomainService>();
            var applicant = new Applicant { Name = "Ali_1", FamilyName = "Hosseini", Address = "Address test", Age = 35, CountryOfOrigin = "Aruba", EMailAddress = "ali.hssinigmail.com" };
            await Assert.ThrowsAsync<ArgumentException>(async () => await service.CreateAsync(applicant));
        }
        [Fact]
        public async Task CreateInvalidApplicant_InvalidAge()
        {
            IApplicantDomainService service = GetService<IApplicantDomainService>();
            var applicant = new Applicant { Name = "Ali_1", FamilyName = "Hosseini", Address = "Address test", Age = 19, CountryOfOrigin = "Aruba", EMailAddress = "ali.hssini@gmail.com" };
            await Assert.ThrowsAsync<ArgumentException>(async () => await service.CreateAsync(applicant));
        }
        [Fact]
        public async Task CreateInvalidApplicant_InvalidCountry()
        {
            IApplicantDomainService service = GetService<IApplicantDomainService>();
            var applicant = new Applicant { Name = "Ali_1", FamilyName = "Hosseini", Address = "Address test", Age = 35, CountryOfOrigin = "Aruba2", EMailAddress = "ali.hssini@gmail.com" };
            await Assert.ThrowsAsync<ArgumentException>(async () => await service.CreateAsync(applicant));
        }
        [Fact]
        public async Task CreateInvalidApplicant_InvalidAddress()
        {
            IApplicantDomainService service = GetService<IApplicantDomainService>();
            var applicant = new Applicant { Name = "Ali_1", FamilyName = "Hosseini", Address = "Address", Age = 35, CountryOfOrigin = "Aruba", EMailAddress = "ali.hssini@gmail.com" };
            await Assert.ThrowsAsync<ArgumentException>(async () => await service.CreateAsync(applicant));
        }
        #endregion
        #region Read
        [Fact]
        public async Task GetApplicant_All()
        {
            using AppDbContext context = new AppDbContext(GetDbContextOptions());
            context.Applicants.RemoveRange(context.Applicants);
            context.SaveChanges();
            IApplicantDomainService service = GetService<IApplicantDomainService>();
            var applicant = new Applicant { Name = "Ali_1", FamilyName = "Hosseini", Address = "Address test", Age = 35, CountryOfOrigin = "Aruba", EMailAddress = "ali.hssini@gmail.com" };
            await service.CreateAsync(applicant);
            var applicant2 = new Applicant { Name = "Ali_1", FamilyName = "Hosseini", Address = "Address test", Age = 35, CountryOfOrigin = "Aruba", EMailAddress = "ali.hssini@gmail.com" };
            await service.CreateAsync(applicant2);
            var applicants = service.GetAll().ToList();
            Assert.Equal(2, applicants.Count);
        }
        [Fact]
        public async Task GetApplicant_ById()
        {
            IApplicantDomainService service = GetService<IApplicantDomainService>();
            var applicant = new Applicant { Name = "Ali_1", FamilyName = "Hosseini", Address = "Address test", Age = 35, CountryOfOrigin = "Aruba", EMailAddress = "ali.hssini@gmail.com" };
            await service.CreateAsync(applicant);
            var result = await service.GetAsync(applicant.ID);
            Assert.NotNull(result);
            Assert.NotEqual(0, result.ID);
        }
        #endregion
        #region Update
        [Fact]
        public async Task UpdateApplicant()
        {
            IApplicantDomainService service = GetService<IApplicantDomainService>();
            var applicant = new Applicant { Name = "Ali_1", FamilyName = "Hosseini", Address = "Address test", Age = 35, CountryOfOrigin = "Aruba", EMailAddress = "ali.hssini@gmail.com" };
            await service.CreateAsync(applicant);
            var result = await service.GetAsync(applicant.ID);
            result.Name = "Ali_2";
            await service.UpdateAsync(result);
            var updatedResult = await service.GetAsync(applicant.ID);
            Assert.NotNull(updatedResult);
            Assert.Equal("Ali_2", result.Name);
        }

        #endregion
        #region Delete
        [Fact]
        public async Task DeleteApplicant()
        {
            IApplicantDomainService service = GetService<IApplicantDomainService>();
            var applicant = new Applicant { Name = "Ali_1", FamilyName = "Hosseini", Address = "Address test", Age = 35, CountryOfOrigin = "Aruba", EMailAddress = "ali.hssini@gmail.com" };
            await service.CreateAsync(applicant);
            //Check Existence
            var result = await service.GetAsync(applicant.ID);
            Assert.NotNull(result);

            //Delete
            var updateState = await service.DeleteAsync(applicant.ID);
            Assert.True(updateState);

            //Check 
            var deletedResult = await service.GetAsync(applicant.ID);
            Assert.Null(deletedResult);
        }

        #endregion
    }
}
