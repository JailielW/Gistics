using Gistics.Models;

namespace Gistics.Services
{
    public interface IDataService
    {
        Task<List<Employees>> GetAllEmployeesAsync();
        Task<Employees> GetEmployeeAsync(int id);
        Task<bool> Insert_EmployeeAsync(string firstname, string lastname, string badgeNum, int titleid,
            DateOnly birthdate);
        Task<bool> UpdateAsync(int eID, string firstname, string lastname, string badgeNum, int titleid,
            DateOnly birthdate, DateOnly startdate, DateOnly enddate);
        Task<bool> TerminateEmployeeAsync(int empid);
        Task<List<EmployeeTitles>> GetTitlesAsync();
    }
}
