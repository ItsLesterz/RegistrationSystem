using RegistrationSystem.RegistrationAPI.Interfaces;
using RegistrationSystem.RegistrationAPI.Models;

namespace RegistrationSystem.RegistrationAPI.Services
{
    public class RegistrationService
    {
        private readonly IDataService<Register> _dataSevice;
        private readonly HttpClient _studentsHttpClient;
        private readonly HttpClient _coursesHttpClient;

        public RegistrationService(IDataService<Register> dataService, HttpClient studentsHttpClient, HttpClient coursesHttpClient)
        {
            _dataSevice = dataService;
            _studentsHttpClient = studentsHttpClient;
            _coursesHttpClient = coursesHttpClient;
        }

    }
}
