using Ali.Hosseini.Application.Api.Controllers;
using Ali.Hosseini.Application.Data.DBContext;
using Ali.Hosseini.Application.Domain.AggregatesModel.ApplicantAggregate;
using Ali.Hosseini.Application.Domain.DomainServiceInterfaces;
using Ali.Hosseini.Application.Tests.Core;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Ali.Hosseini.Application.Tests
{

    public class ApplicantsControllerUnitTests : TestBase
    {
        private ApplicantsController _controller;
        private AppDbContext context;
        public ApplicantsControllerUnitTests()
        {
            var svc = GetService<IApplicantDomainService>();
            svc.Validator = GetService<IValidator<Applicant>>();
            _controller = new ApplicantsController(svc,null);
            context = GetDbContext();
        }
        #region Get
        [Fact]
        public async Task ControllerGetAll()
        {
            context.Applicants.RemoveRange(context.Applicants);
            await context.SaveChangesAsync();
            var applicant = new Applicant { Name = "Ali_1", FamilyName = "Hosseini", Address = "Address test", Age = 35, CountryOfOrigin = "Aruba", EMailAddress = "ali.hssini@gmail.com" };
            await _controller.PostApplicant(applicant);
            //Act
            var actionResult = await _controller.GetApplicants();
            var result = Get(actionResult);
            Assert.True(result.Count()>0);
        }
        #endregion
        #region Post
        [Fact]
        public async Task ControllerPostApplicant_InvalidName()
        {
            var applicant = new Applicant { Name = "Ali", FamilyName = "Hosseini", Address = "Address test", Age = 35, CountryOfOrigin = "Aruba", EMailAddress = "ali.hssini@gmail.com" };
            await Assert.ThrowsAsync<ArgumentException>(async () => await _controller.PostApplicant(applicant));
        }
        [Fact]
        public async Task ControllerPostApplicant_InvalidFamily()
        {
            var applicant = new Applicant { Name = "Ali_1", FamilyName = "test", Address = "Address test", Age = 35, CountryOfOrigin = "Aruba", EMailAddress = "ali.hssini@gmail.com" };
            await Assert.ThrowsAsync<ArgumentException>(async () => await _controller.PostApplicant(applicant));
        }
        [Fact]
        public async Task ControllerPostApplicant_InvalidAddress()
        {
            var applicant = new Applicant { Name = "Ali_1", FamilyName = "Hosseini", Address = "Address", Age = 35, CountryOfOrigin = "Aruba", EMailAddress = "ali.hssini@gmail.com" };
            await Assert.ThrowsAsync<ArgumentException>(async () => await _controller.PostApplicant(applicant));
        }
        [Fact]
        public async Task ControllerPostApplicant_InvalidAge()
        {
            var applicant = new Applicant { Name = "Ali_1", FamilyName = "Hosseini", Address = "Address test", Age = 15, CountryOfOrigin = "Aruba", EMailAddress = "ali.hssini@gmail.com" };
            await Assert.ThrowsAsync<ArgumentException>(async () => await _controller.PostApplicant(applicant));
        }
        [Fact]
        public async Task ControllerPostApplicant_InvalidCountry()
        {
            var applicant = new Applicant { Name = "Ali_1", FamilyName = "Hosseini", Address = "Address test", Age = 35, CountryOfOrigin = "asd", EMailAddress = "ali.hssini@gmail.com" };
            await Assert.ThrowsAsync<ArgumentException>(async () => await _controller.PostApplicant(applicant));
        }
        [Fact]
        public async Task ControllerPostApplicant_InvalidEmail()
        {
            var applicant = new Applicant { Name = "Ali_1", FamilyName = "Hosseini", Address = "Address test", Age = 35, CountryOfOrigin = "Aruba", EMailAddress = "ali.hssinigmail.com" };
            await Assert.ThrowsAsync<ArgumentException>(async () => await _controller.PostApplicant(applicant));
        }
        [Fact]
        public async Task ControllerPostApplicant_ValidApplicant()
        {
            //Act
            var applicant = new Applicant { Name = "Ali_1", FamilyName = "Hosseini", Address = "Address test", Age = 35, CountryOfOrigin = "Aruba", EMailAddress = "ali.hssini@gmail.com" };
            var actionResult = await _controller.PostApplicant(applicant);
            var result = Get(actionResult);
            Assert.NotNull(actionResult);
            Assert.NotEqual(0, result.ID);
        }
        #endregion
        #region Delete
        [Fact]
        public async Task ControllerDeleteApplicant_DeleteByID()
        {
            var applicant = new Applicant { Name = "Ali_1", FamilyName = "Hosseini", Address = "Address test", Age = 35, CountryOfOrigin = "Aruba", EMailAddress = "ali.hssini@gmail.com" };
            await _controller.PostApplicant(applicant);

            //Delete
            var deleteResult = await _controller.DeleteApplicant(applicant.ID);
            var boolResult = Get(deleteResult);

            Assert.True(boolResult);
        }
        #endregion
        #region Update
        [Fact]
        public async Task ControllerPutApplicant_ValidApplicantAndCheckResult()
        {
            //Act
            var applicant = new Applicant { Name = "Ali_1", FamilyName = "Hosseini", Address = "Address test", Age = 35, CountryOfOrigin = "Aruba", EMailAddress = "ali.hssini@gmail.com" };
            await _controller.PostApplicant(applicant);


            var actionResult = await _controller.GetApplicant(applicant.ID);
            var result = Get(actionResult);
            result.Name = "Ali_2";
            await _controller.PutApplicant(result.ID, result);

            var updatedResult = await _controller.GetApplicant(result.ID);
            var finalResult = Get(updatedResult);
            Assert.Equal("Ali_2", finalResult.Name);
        }
        #endregion
    }
}
