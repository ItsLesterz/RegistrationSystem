using Microsoft.AspNetCore.Mvc;
using RegistrationSystem.RegistrationAPI.Models;
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
            // Crear método para listar alumnos matriculados dado un id de clase
            var registrations = _registrationService.GetRegistrationsByCourseId(courseId)
                .Select(r => r.StudentId)
                .ToList();

            if (registrations == null || !registrations.Any())
            {
                return NotFound();
            }

            return Ok(registrations);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RegisterDto registerDto)
        {
            // método para matricular a un estudiante en una o más clases
            if (registerDto == null || registerDto.CourseIds == null || !registerDto.CourseIds.Any())
            {
                return BadRequest("Invalid registration data.");
            }

            try
            {
                var result = await _registrationService.CreateRegistrationsAsync(registerDto);
                return StatusCode(StatusCodes.Status201Created, result);
            }
            catch (HttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
