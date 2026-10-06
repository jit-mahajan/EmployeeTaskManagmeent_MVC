using EmployeeManagmeent.ViewModels;

namespace EmployeeManagmeent.Services
{
    public interface IEmployeeService
    {
        Task<List<EmployeeViewModel>> GetAllAsync();
        Task<EmployeeListViewModel> GetEmployeesAsync(int page, int pageSize);
        
        Task<EmployeeViewModel?> GetByIdAsync(int id);

        Task<EmployeeDetailsViewModel?> GetDetailsAsync(int id);

        Task<ServiceResult> CreateAsync(EmployeeViewModel model);

        Task<ServiceResult> UpdateAsync(EmployeeViewModel model);

        Task<ServiceResult> DeleteAsync(int id);

        Task<List<EmployeeViewModel>> GetActiveEmployeesAsync();

        Task<EmployeeAjaxViewModel?> GetEmployeeForAjaxAsync(int id);
    }
}
