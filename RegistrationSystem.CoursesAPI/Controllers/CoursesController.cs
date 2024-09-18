using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RegistrationSystem.CoursesAPI.Interfaces;
using RegistrationSystem.CoursesAPI.Models;

namespace RegistrationSystem.CoursesAPI.Controllers
{
    [Route("courses")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly IDataService<Course> _dataService;
        public CoursesController(IDataService<Course> dataService)
        {
            _dataService = dataService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var courses = _dataService.GetEntities();

            return courses == null ? NotFound() : Ok(courses);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(string id)
        {
            var course = _dataService.GetEntityById(id);

            return course == null ? NotFound() : Ok(course);
        }
    }
}
