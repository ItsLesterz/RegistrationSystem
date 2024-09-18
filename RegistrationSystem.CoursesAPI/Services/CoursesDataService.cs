using Newtonsoft.Json;
using RegistrationSystem.CoursesAPI.Interfaces;
using RegistrationSystem.CoursesAPI.Models;

namespace RegistrationSystem.CoursesAPI.Services
{
    public class CoursesDataService : IDataService<Course>
    {
        private const string CoursesFileName = "courses.json";
        public IEnumerable<Course> GetEntities()
        {
            return JsonConvert.DeserializeObject<IEnumerable<Course>>(File.ReadAllText(CoursesFileName));
        }

        public Course GetEntityById(string entityId)
        {
            var json = File.ReadAllText(CoursesFileName);
            var students = JsonConvert.DeserializeObject<List<Course>>(json);
            return students.FirstOrDefault(student => student.Id == entityId);
        }
    }
}
