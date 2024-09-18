using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RegistrationSystem.RegistrationAPI.Models;
using System.Collections.Generic;
using System.Linq;
using RegistrationSystem.RegistrationAPI.Interfaces;

public class RegistrationService
{
    string connectionString = "amqp://username:password@localhost:5672/myhost";
    private readonly IDataService<Register> _dataService;
    private readonly HttpClient _studentsHttpClient;
    private readonly HttpClient _coursesHttpClient;
    private readonly IConnection _rabbitMqConnection;

    public RegistrationService(IDataService<Register> dataService, HttpClient studentsHttpClient, HttpClient coursesHttpClient, IConnection rabbitMqConnection)
    {
        _dataService = dataService;
        _studentsHttpClient = studentsHttpClient;
        _coursesHttpClient = coursesHttpClient;
        _rabbitMqConnection = rabbitMqConnection;
    }
    public IEnumerable<Register> GetRegistrationsByCourseId(string courseId)
    {
        // Retrieve registrations for the given course ID from the data service
        var registrations = _dataService.GetEntities()
            .Where(r => r.CourseId == courseId)
            .ToList();

        return registrations;
    }
    public async Task<bool> ValidateStudentAsync(int studentId)
    {
        var response = await _studentsHttpClient.GetAsync($"/students/{studentId}");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> ValidateCourseAsync(string courseId)
    {
        var response = await _coursesHttpClient.GetAsync($"/courses/{courseId}");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> CheckCourseAvailabilityAsync(string courseId)
    {
        var response = await _coursesHttpClient.GetAsync($"/courses/{courseId}/availability");
        if (!response.IsSuccessStatusCode) return false;

        var availability = await response.Content.ReadAsStringAsync();
        return bool.Parse(availability);
    }

    public async Task<IEnumerable<Register>> CreateRegistrationsAsync(RegisterDto registerDto)
    {
        // Validate student
        if (!await ValidateStudentAsync(registerDto.StudentId))
        {
            throw new HttpRequestException("Invalid student ID.");
        }

        // Validate courses and check availability
        foreach (var courseId in registerDto.CourseIds)
        {
            if (!await ValidateCourseAsync(courseId))
            {
                throw new HttpRequestException($"Invalid course ID: {courseId}");
            }

            if (!await CheckCourseAvailabilityAsync(courseId))
            {
                throw new HttpRequestException($"No available slots for course ID: {courseId}");
            }
        }

        // Create registrations
        var registrations = registerDto.CourseIds.Select(courseId => new Register
        {
            StudentId = registerDto.StudentId,
            CourseId = courseId
        }).ToList();

        var result = _dataService.PostEntities(registrations);

        // Send report to ReportService via RabbitMQ
        SendReportToRabbitMq(result);

        return result;
    }

    private void SendReportToRabbitMq(IEnumerable<Register> registrations)
    {
        using (var channel = _rabbitMqConnection.CreateModel())
        {
            channel.QueueDeclare(queue: "reportQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);

            var message = JsonSerializer.Serialize(registrations);
            var body = System.Text.Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "", routingKey: "reportQueue", basicProperties: null, body: body);
        }
    }
}
