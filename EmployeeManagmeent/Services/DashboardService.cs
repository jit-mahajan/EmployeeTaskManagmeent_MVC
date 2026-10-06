using EmployeeManagmeent.Data;
using EmployeeManagmeent.Models;
using EmployeeManagmeent.ViewModels;
using Microsoft.EntityFrameworkCore;
using TaskStatus = EmployeeManagmeent.Models;


namespace EmployeeManagmeent.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly MyDbContext _context;

        public DashboardService(MyDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardViewModel> GetDashboardAsync()
        {
            var model = new DashboardViewModel();

            model.TotalEmployees = await _context.Employees.CountAsync();

            model.ActiveEmployees = await _context.Employees.CountAsync(x => x.IsActive);

            model.TotalTasks = await _context.EmployeeTasks.CountAsync();

            model.PendingTasks = await _context.EmployeeTasks.CountAsync(x => x.Status == EmployeeTaskStatus.Pending);

            model.InProgressTasks = await _context.EmployeeTasks.CountAsync(x => x.Status == EmployeeTaskStatus.InProgress);

            model.CompletedTasks = await _context.EmployeeTasks.CountAsync(x => x.Status == EmployeeTaskStatus.Completed);

            model.RecentTasks = await _context.EmployeeTasks
                                .OrderByDescending(x => x.CreatedDate)
                                .Take(5)
                                .Select(x => new EmployeeTaskViewModel
                                {
                                    Id = x.Id,
                                    Title = x.Title,
                                    EmployeeName = x.Employee != null
                                        ? x.Employee.Name
                                        : string.Empty,
                                    Status = x.Status,
                                    DueDate = x.DueDate,
                                    CreatedDate = x.CreatedDate
                                })
                                .ToListAsync();

            return model;
        }
    }
}

