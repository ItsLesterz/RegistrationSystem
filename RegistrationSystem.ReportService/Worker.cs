namespace RegistrationSystem.ReportService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IRegistrationService _registrationService;

        public Worker(ILogger<Worker> logger, IRegistrationService registrationService)
        {
            _logger = logger;
            _registrationService = registrationService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    var registrations = await _registrationService.GetSuccessfulRegistrationsAsync();
                    foreach (var registration in registrations)
                    {
                        var message = $"Matrícula exitosa para el estudiante {registration.StudentName} con cuenta {registration.AccountNumber} en los cursos:\n\n";
                        foreach (var course in registration.Courses)
                        {
                            message += $"{course}\n\n";
                        }
                        _logger.LogInformation(message);
                    }
                }
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
