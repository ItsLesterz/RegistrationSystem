namespace RegistrationSystem.StudentsAPI.Interfaces
{
    public interface IDataService<T> where T : class
    {
        IEnumerable<T> GetEntities();
        T GetEntityById(int entityId);
    }
}
