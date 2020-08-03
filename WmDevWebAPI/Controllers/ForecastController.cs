using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using WmDevWebAPI.Models;
using WmDevWebAPI.Services;

namespace WmDevWebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ForecastController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };


        public ForecastController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }


        [Authorize(Roles = Role.Admin)]
        [HttpPost("show")]
        public IActionResult Show([FromBody] UserVm userParam)
        {
            string message = null;
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                IList<Claim> claims = identity.Claims.ToList();
                var userName = claims[0].Value;

                message = string.Format("Hey {0}!, You are authorized to access API.", userName);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Show", ex);
            }

            return Ok(message);
        }


        [Authorize(Roles = Role.User)]
        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            try
            {
                var rng = new Random();
                return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
                .ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Show", ex);
            }

            return null;
        }
    }
}
