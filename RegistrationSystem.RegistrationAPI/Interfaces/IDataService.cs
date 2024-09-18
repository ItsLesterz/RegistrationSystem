namespace RegistrationSystem.RegistrationAPI.Interfaces
{
    public interface IDataService<T> where T : class
    {
        IEnumerable<T> GetEntities();
        T PostEntity(T entity);
        IEnumerable<T> PostEntities(IEnumerable<T> entities);
    }
}
