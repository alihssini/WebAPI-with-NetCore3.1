using Ali.Hosseini.Application.Domain.AggregatesModel.ApplicantAggregate;
using Ali.Hosseini.Application.Tests.Core;
using FluentValidation;
using Xunit;

namespace Ali.Hosseini.Application.Tests
{
    public class ApplicantValidationUnitTests : TestBase
    {
        [Fact]
        public void ApplicantValidation_InvalidName()
        {
            var validator = GetService<IValidator<Applicant>>();
            var applicant = new Applicant { Name = "Ali", FamilyName = "Hosseini", Address = "Address test", Age = 35, CountryOfOrigin = "Aruba", EMailAddress = "ali.hssini@gmail.com" };
            var result = validator.Validate(applicant);
            Assert.NotNull(result);
            Assert.Equal(1, result.Errors.Count);
            Assert.False(result.IsValid);
        }
        [Fact]
        public void ApplicantValidation_InvalidFamily()
        {
            var validator = GetService<IValidator<Applicant>>();
            var applicant = new Applicant { Name = "Ali_1", FamilyName = "test", Address = "Address test", Age = 35, CountryOfOrigin = "Aruba", EMailAddress = "ali.hssini@gmail.com" };
            var result = validator.Validate(applicant);
            Assert.NotNull(result);
            Assert.Equal(1, result.Errors.Count);
            Assert.False(result.IsValid);
        }
        [Fact]
        public void ApplicantValidation_InvalidEmail()
        {
            var validator = GetService<IValidator<Applicant>>();
            var applicant = new Applicant { Name = "Ali_1", FamilyName = "Hosseini", Address = "Address test", Age = 35, CountryOfOrigin = "Aruba", EMailAddress = "ali.hssinigmail.com" };
            var result = validator.Validate(applicant);
            Assert.NotNull(result);
            Assert.Equal(1, result.Errors.Count);
            Assert.False(result.IsValid);
        }
        [Fact]
        public void ApplicantValidation_InvalidAge()
        {
            var validator = GetService<IValidator<Applicant>>();
            var applicant = new Applicant { Name = "Ali_1", FamilyName = "Hosseini", Address = "Address test", Age = 19, CountryOfOrigin = "Aruba", EMailAddress = "ali.hssini@gmail.com" };
            var result = validator.Validate(applicant);
            Assert.NotNull(result);
            Assert.Equal(1, result.Errors.Count);
            Assert.False(result.IsValid);
        }
        [Fact]
        public void ApplicantValidation_InvalidCountry()
        {
            var validator = GetService<IValidator<Applicant>>();
            var applicant = new Applicant { Name = "Ali_1", FamilyName = "Hosseini", Address = "Address test", Age = 35, CountryOfOrigin = "Aruba2", EMailAddress = "ali.hssini@gmail.com" };
            var result = validator.Validate(applicant);
            Assert.NotNull(result);
            Assert.Equal(1, result.Errors.Count);
            Assert.False(result.IsValid);
        }
        [Fact]
        public void ApplicantValidation_InvalidAddress()
        {
            var validator = GetService<IValidator<Applicant>>();
            var applicant = new Applicant { Name = "Ali_1", FamilyName = "Hosseini", Address = "Address", Age = 35, CountryOfOrigin = "Aruba", EMailAddress = "ali.hssini@gmail.com" };
            var result = validator.Validate(applicant);
            Assert.NotNull(result);
            Assert.Equal(1, result.Errors.Count);
            Assert.False(result.IsValid);
        }
    }
}
