using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RegistrationSystem.StudentsAPI.Interfaces;
using RegistrationSystem.StudentsAPI.Models;

namespace RegistrationSystem.StudentsAPI.Controllers
{
    [Route("students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IDataService<Student> _dataService;
        public StudentsController(IDataService<Student> dataService)
        {
            _dataService = dataService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var students = _dataService.GetEntities();

            return students == null ? NotFound() : Ok(students);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult Get(int id)
        {
            var student = _dataService.GetEntityById(id);

            return student == null ? NotFound() : Ok(student);
        }
    }
}
