using EmployeeManagmeent.ViewModels;

namespace EmployeeManagmeent.Services
{
    public interface IDashboardService
    {
        Task<DashboardViewModel> GetDashboardAsync();
    }
}
