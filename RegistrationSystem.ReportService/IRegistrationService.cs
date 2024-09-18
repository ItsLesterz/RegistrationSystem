using System.Collections.Generic;
using System.Threading.Tasks;

public interface IRegistrationService
{
    Task<IEnumerable<Registration>> GetSuccessfulRegistrationsAsync();
}

public class Registration
{
    public string StudentName { get; set; }
    public string AccountNumber { get; set; }
    public List<string> Courses { get; set; }
}