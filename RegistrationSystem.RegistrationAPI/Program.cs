using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

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

            // Configuración de la conexión a RabbitMQ
            var factory = new ConnectionFactory() { HostName = "localhost", Port = 7022 }; // Usa tu puerto adecuado
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Asegúrate de que la cola esté declarada en RabbitMQ
            _channel.QueueDeclare(queue: "reportQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            // Evento que se dispara cuando se recibe un mensaje en RabbitMQ
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                // Deserializar el mensaje recibido
                var registrations = JsonSerializer.Deserialize<List<Register>>(message);

                foreach (var registration in registrations)
                {
                    // Crear el log con la información de cada matrícula
                    var logMessage = $"Matrícula exitosa para el estudiante {registration.StudentName} con cuenta {registration.StudentId} en los cursos:\n";
                    foreach (var course in registration.Courses)
                    {
                        logMessage += $"{course}\n";
                    }

                    // Imprimir el reporte en el log
                    _logger.LogInformation(logMessage);
                }
            };

            // Iniciar el consumo de mensajes desde RabbitMQ
            _channel.BasicConsume(queue: "reportQueue", autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }

    // Clase para manejar el objeto Register
    public class Register
    {
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public List<string> Courses { get; set; } // Lista de cursos
    }
}
