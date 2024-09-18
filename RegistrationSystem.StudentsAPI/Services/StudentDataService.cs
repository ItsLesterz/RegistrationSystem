using Newtonsoft.Json;
using RegistrationSystem.StudentsAPI.Interfaces;
using RegistrationSystem.StudentsAPI.Models;

namespace RegistrationSystem.StudentsAPI.Services
{
    public class StudentDataService : IDataService<Student>
    {
        private const string StudentsFileName = "students.json";
        public IEnumerable<Student> GetEntities()
        {
            return JsonConvert.DeserializeObject<IEnumerable<Student>>(File.ReadAllText(StudentsFileName));
        }

        public Student GetEntityById(int entityId)
        {
            var json = File.ReadAllText(StudentsFileName);
            var students = JsonConvert.DeserializeObject<List<Student>>(json);
            return students.FirstOrDefault(student => student.Id == entityId);
        }
    }
}
