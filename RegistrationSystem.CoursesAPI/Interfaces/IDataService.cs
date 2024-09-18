﻿namespace RegistrationSystem.CoursesAPI.Interfaces
{
    public interface IDataService<T> where T : class
    {
        IEnumerable<T> GetEntities();
        T GetEntityById(string entityId);
    }
}
