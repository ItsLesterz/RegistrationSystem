using Newtonsoft.Json;
using RegistrationSystem.RegistrationAPI.Interfaces;
using RegistrationSystem.RegistrationAPI.Models;

namespace RegistrationSystem.RegistrationAPI.Services
{
    public class RegistrationDataService : IDataService<Register>
    {
        private const string RegistrationFileName = "registrations.json";
        public IEnumerable<Register> GetEntities()
        {
            return JsonConvert.DeserializeObject<IEnumerable<Register>>(File.ReadAllText(RegistrationFileName));
        }

        public Register PostEntity(Register entity)
        {
            var json = File.ReadAllText(RegistrationFileName);

            var registrations = JsonConvert.DeserializeObject<List<Register>>(json) ?? new List<Register>();
            registrations.Add(entity);

            var updatedJson = JsonConvert.SerializeObject(registrations, Formatting.Indented);
            File.WriteAllText(RegistrationFileName, updatedJson);
            return entity;
        }

        public IEnumerable<Register> PostEntities(IEnumerable<Register> entities)
        {
            var json = File.ReadAllText(RegistrationFileName);

            var registrations = JsonConvert.DeserializeObject<List<Register>>(json) ?? new List<Register>();
            registrations.AddRange(entities);

            var updatedJson = JsonConvert.SerializeObject(registrations, Formatting.Indented);
            File.WriteAllText(RegistrationFileName, updatedJson);
            return entities;
        }
    }
}
