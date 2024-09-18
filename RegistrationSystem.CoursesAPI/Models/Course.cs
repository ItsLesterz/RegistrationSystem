namespace RegistrationSystem.CoursesAPI.Models
{
    public class Course
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int AvailableSeats { get; set; } // maximo de alumnos en el curso

        public int UV { get; set; }
    }
}
