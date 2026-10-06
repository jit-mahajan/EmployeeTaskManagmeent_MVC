using EmployeeManagmeent.Models;

namespace EmployeeManagmeent.Services
{
    public interface IAccountService
    {
        Task<Admin?> ValidateAdminAsync(string email, string password);
    }
}
