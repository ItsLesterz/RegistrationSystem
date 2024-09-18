using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RegistrationSystem.RegistrationAPI.Models;
using RegistrationSystem.RegistrationAPI.Services;

namespace RegistrationSystem.RegistrationAPI.Controllers
{
    [Route("registrations")]
    [ApiController]
    public class RegistrationsController : ControllerBase
    {
        private readonly RegistrationService _registrationService;

        public RegistrationsController(RegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        [HttpGet]
        [Route("{courseId}")]
        public IActionResult Get(string courseId)
        {
            // método para listar alumnos matriculados dado un id de clase


            return Ok();
        }

        [HttpPost]
        public IActionResult Post([FromBody] RegisterDto registerDto)
        {
            // método para matricular a un estudiante en una o más clases

            return Ok();
        }
    }
}
