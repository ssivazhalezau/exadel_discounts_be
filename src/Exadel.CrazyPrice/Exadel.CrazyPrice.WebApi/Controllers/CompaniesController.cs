﻿using System.Collections.Generic;
using Exadel.CrazyPrice.Common.Interfaces;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Exadel.CrazyPrice.Common.Models;

namespace Exadel.CrazyPrice.WebApi.Controllers
{
    /// <summary>
    /// An example controller performs operations on companies.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ILogger<CompaniesController> _logger;
        private readonly ICompanyRepository _repository;

        /// <summary>
        /// Creates Company Controller.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="repository"></param>
        public CompaniesController(ILogger<CompaniesController> logger, ICompanyRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        /// <summary>
        /// Gets company names from string.
        /// </summary>
        /// <param name="companyName">The search string.</param>
        /// <returns></returns>
        /// <response code="200">Company names found.</response>
        /// <response code="400">Bad request.</response> 
        /// <response code="404">No company names found.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [Route("get/{companyName}")]
        public async Task<IActionResult> GetCompanyNames([FromRoute, CustomizeValidator(RuleSet = "SearchString")] string companyName)
        {
            _logger.LogInformation("Company name incoming: {companyName}", companyName);
            var companies = await _repository.GetCompanyNamesAsync(companyName);

            if (companies == null || companies.Count == 0)
            {
                _logger.LogWarning("Companies get: {@companies}.", companies);
                return NotFound("No company names found.");
            }

            _logger.LogInformation("Companies get: {@companies}", companies);
            return Ok(companies);
        }
    }
}
