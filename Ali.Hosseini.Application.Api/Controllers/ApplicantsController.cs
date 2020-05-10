using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ali.Hosseini.Application.Domain.AggregatesModel.ApplicantAggregate;
using Ali.Hosseini.Application.Domain.DomainServiceInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace Ali.Hosseini.Application.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicantsController : ControllerBase
    {
        #region Vars
        private readonly IApplicantDomainService _service;
        private readonly ILogger<ApplicantsController> _logger;
        #endregion
        public ApplicantsController(IApplicantDomainService applicantDomainService, ILogger<ApplicantsController> logger)
        {
            _service = applicantDomainService;
            _logger = logger;
        }
        /// <summary>
        /// Get all Applicants
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Applicant>>> GetApplicants()
        {
            return await _service.GetAll().ToListAsync();
        }
        /// <summary>
        /// Return a specific Applicant
        /// </summary>
        /// <param name="id">Applicant ID</param>
        /// <returns>Applicant</returns>
        [HttpGet("{id}")]
        [SwaggerResponse(200, "Success")]
        [SwaggerResponse(404, "If the Applicant not found!")]
        public async Task<ActionResult<Applicant>> GetApplicant(int id)
        {
            var applicant = await _service.GetAsync(id);

            if (applicant == null)
            {
                _logger?.LogDebug($"GET - Applicant with ID:\"{id}\" not found!");
                return NotFound();
            }

            return applicant;
        }
        /// <summary>
        /// Update an Applicant.
        /// </summary>
        /// <remarks>
        /// Note that your data must meet the following validations:
        ///  
        ///     POST /Todo
        ///     {
        ///        "id": same value with given id (integer)
        ///        "name": "at least 5 Characters",
        ///        "familyName": "at least 5 Characters",
        ///        "address": "at least 10 Characters",
        ///        "countryOfOrigin": "must be a valid Country",
        ///        "eMailAddress": "must be an valid email",
        ///        "age": must be between 20 and 60
        ///     }
        /// 
        /// </remarks>
        /// <param name="id">Applicant ID (integer)</param>
        /// <param name="applicant">Applicant object</param>
        /// <returns></returns>
        [SwaggerResponse(200, "Success with created applicant link in header")]
        [SwaggerResponse(400, "If the Applicant is not valid!")]
        [SwaggerResponse(404, "If the Applicant not found!")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApplicant(int id, Applicant applicant)
        {
            if (id != applicant.ID)
            {
                _logger?.LogDebug($"PUT - The object with ID:\"{applicant.ID}\" does not match with the given ID:\"{id}\".");
                return BadRequest();
            }

            var result = await _service.UpdateAsync(applicant);

            if (result == null)
            {
                _logger?.LogDebug($"PUT - Applicant with ID:\"{id}\" not found!");
                return NotFound();
            }
            return Ok();
        }
        /// <summary>
        /// Creates an Applicant.
        /// </summary>
        /// <remarks>
        /// Note that your data must meet the following validations:
        ///  
        ///     POST /Todo
        ///     {
        ///        "name": "at least 5 Characters",
        ///        "familyName": "at least 5 Characters",
        ///        "address": "at least 10 Characters",
        ///        "countryOfOrigin": "must be a valid Country",
        ///        "eMailAddress": "must be an valid email",
        ///        "age": must be between 20 and 60
        ///     }
        /// 
        /// </remarks>
        /// <param name="applicant">Applicant ID</param>
        /// <returns>New Created Applicant</returns>
        [SwaggerResponse(201, "Success with created applicant link in header")]
        [SwaggerResponse(400, "If the Applicant is not valid!")]
        [HttpPost]
        public async Task<ActionResult<Applicant>> PostApplicant(Applicant applicant)
        {
            var result = await _service.CreateAsync(applicant);
            return CreatedAtAction("GetApplicant", new { id = result.ID }, result);
        }
        /// <summary>
        /// Delete a specific Applicant
        /// </summary>
        /// <param name="id">Applicant ID (integer)</param>
        /// <returns></returns>
        [SwaggerResponse(200, "Success")]
        [SwaggerResponse(404, "If the Applicant not found!")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteApplicant(int id)
        {
            var applicant = await _service.GetAsync(id);
            if (applicant == null)
            {
                _logger?.LogDebug($"DELETE - Applicant with ID:\"{id}\" not found!");
                return NotFound();
            }

            return await _service.DeleteAsync(applicant);
        }

    }
}
