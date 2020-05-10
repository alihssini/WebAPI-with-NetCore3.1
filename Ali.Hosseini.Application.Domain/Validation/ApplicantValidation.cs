using Ali.Hosseini.Application.Domain.AggregatesModel.ApplicantAggregate;
using FluentValidation;
using FluentValidation.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Ali.Hosseini.Application.Domain.Validation
{
    public class ApplicantValidation : AbstractValidator<Applicant>
    {
        #region Vars
        private readonly HttpClient _httpClient;
        private readonly IStringLocalizer<ApplicantValidation> _localizer;
        private readonly ILogger<ApplicantValidation> _logger;
        #endregion
        #region Ctor
        public ApplicantValidation(IHttpClientFactory clientFactory, IStringLocalizer<ApplicantValidation> localizer, ILogger<ApplicantValidation> Logger)
        {
            _httpClient = clientFactory.CreateClient("HttpClient");
            _localizer = localizer;
            _logger = Logger;
            DefineRule();
        }
        #endregion
        #region ValidationMethods
        /// <summary>
        /// Validation Rules
        /// </summary>
        private void DefineRule()
        {
            MinLen(Entity => Entity.Name, 5);
            MinLen(Entity => Entity.FamilyName, 5);
            MinLen(Entity => Entity.Address, 10);
            RuleFor(TEntity => TEntity.CountryOfOrigin).MustAsync(IsValidCountry).WithMessage(_localizer["Country is not available"]);//for use custom message with resources
            RuleFor(TEntity => TEntity.EMailAddress).NotEmpty().EmailAddress();
            RuleFor(TEntity => TEntity.Age).InclusiveBetween(20, 60);
            RuleFor(TEntity => TEntity.Hired).NotNull();

            void MinLen(Expression<Func<Applicant, string>> expression, int min)
            {
                RuleFor(expression).NotEmpty().MinimumLength(min);
            }
        }
        /// <summary>
        /// Check Country availability with basic resiliency
        /// </summary>
        /// <remarks>
        /// Used: Polly library (try 3 times and after that fallback with deafult action)
        /// </remarks>
        /// <param name="applicant"></param>
        /// <param name="countryOfOrigin"></param>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<bool> IsValidCountry(
               Applicant applicant,
               string countryOfOrigin,
               PropertyValidatorContext context,
               CancellationToken cancellationToken)
        {
            try
            {
                var responseMessage = await _httpClient.GetAsync($"https://restcountries.eu/rest/v2/name/{countryOfOrigin}?fullText=true");
                _logger?.LogInformation($"StatusCode for country[\"{countryOfOrigin}\"]: {responseMessage.StatusCode}({(int)responseMessage.StatusCode})");
                return responseMessage.StatusCode == System.Net.HttpStatusCode.OK;
            }
            catch(Exception ex)
            {
                _logger?.LogError("Webservice is down. Exception: {0}",ex.Message);
                return false;
            }
          
        }
        #endregion
    }
}
