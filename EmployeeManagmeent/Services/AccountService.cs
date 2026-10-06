using EmployeeManagmeent.Data;
using EmployeeManagmeent.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagmeent.Services
{
    public class AccountService : IAccountService
    {
        private readonly MyDbContext _context;
        public AccountService(MyDbContext context)
        {
            _context = context;
        }

        public async Task<Admin?> ValidateAdminAsync( string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            return await _context.Admins .AsNoTracking()
                                        .FirstOrDefaultAsync(x =>
                                            x.Email == email &&
                                            x.Password == password &&
                                            x.IsActive);
        }
    }
}
