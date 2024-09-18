namespace RegistrationSystem.RegistrationAPI.Models
{
    public class RegisterDto
    {
        public int StudentId { get; set; }

        public List<string> CourseIds { get; set; }
    }
}
