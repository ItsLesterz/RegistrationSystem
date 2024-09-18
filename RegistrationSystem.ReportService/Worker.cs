using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading;
using System.Threading.Tasks;

namespace RegistrationSystem.ReportService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;

            // Configuración de la conexión con RabbitMQ
            var factory = new ConnectionFactory() { HostName = "localhost", Port = 7022 }; // Cambia el puerto si es necesario
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Declarar la cola desde la que se va a consumir
            _channel.QueueDeclare(queue: "registrations", durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Crear un consumidor para recibir mensajes de RabbitMQ
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                // Deserializar el mensaje para obtener la matrícula
                var registration = JsonSerializer.Deserialize<Register>(message);

                // Formatear el log de la matrícula exitosa
                var logMessage = $"Matrícula exitosa para el estudiante {registration.StudentName} con cuenta {registration.StudentId} en los cursos:\n\n";
                foreach (var course in registration.Courses)
                {
                    logMessage += $"{course.CourseName}\n\n";
                }

                // Imprimir el log
                _logger.LogInformation(logMessage);
            };

            // Iniciar la escucha de mensajes
            _channel.BasicConsume(queue: "registrations", autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }

    // Clase auxiliar para manejar la deserialización
    public class Register
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public List<Course> Courses { get; set; }
    }

    public class Course
    {
        public string CourseId { get; set; }
        public string CourseName { get; set; }
    }
}
