using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using WmDevWebAPI.Models;
using WmDevWebAPI.Services;

namespace WmDevWebAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IUserService _userService;
       
        public AuthController(ILogger<AuthController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("API Started");
        }


        /*
            How to call from client
            Request URL : http://localhost:54688/Auth

            Header Body:
                Content-Type: application/json

            Request Body Post:
                { "Username": "Pavan", "Password": "Pass"}

         */


       
        [HttpPost]
        public IActionResult Post([FromBody] UserVm userParam)
        {
            UserVm user = null;

            try
            {
                user = _userService.IsValid(userParam.Username, userParam.Password);

                if (user == null)
                    return BadRequest(new { message = "Username or password is incorrect" });                
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Authenticate", ex);
            }

            return Ok(user);
        }
    }
}
