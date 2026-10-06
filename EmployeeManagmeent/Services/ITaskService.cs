using EmployeeManagmeent.Models;
using EmployeeManagmeent.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagmeent.Services
{
    public interface ITaskService
    {
        Task<List<EmployeeTaskViewModel>> GetAllAsync();
        Task<TaskFilterViewModel> GetTasksAsync(int? employeeId, EmployeeTaskStatus? status, DateTime? fromDate, DateTime? toDate, int page, int pageSize);
        Task<TaskDetailsViewModel?> GetByIdAsync( int id);
        Task<ServiceResult> CreateAsync(EmployeeTaskViewModel model);
        Task<ServiceResult> UpdateAsync(EmployeeTaskViewModel model);
        Task<ServiceResult> DeleteAsync(int id);
        Task<TaskFilterViewModel> SearchAsync(TaskFilterViewModel filter);
        Task<EmployeeTaskViewModel?> GetForEditAsync(int id);
        Task<EmployeeViewModel> GetEmployeeInfoAsync(int employeeId);
    }
}
