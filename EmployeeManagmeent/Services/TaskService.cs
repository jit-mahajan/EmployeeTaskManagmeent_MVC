using EmployeeManagmeent.Data;
using EmployeeManagmeent.Models;
using EmployeeManagmeent.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagmeent.Services
{
    public class TaskService : ITaskService
    {
        private readonly MyDbContext _context;

        public TaskService(MyDbContext context)
        {
            _context = context;
        }

        public async Task<List<EmployeeTaskViewModel>> GetAllAsync()
        {
            return await _context.EmployeeTasks
                .AsNoTracking()
                .Include(x => x.Employee)
                .OrderByDescending(x => x.Id)
                .Select(x => new EmployeeTaskViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    EmployeeId = x.EmployeeId,
                    EmployeeName =
                        x.Employee != null
                            ? x.Employee.Name
                            : string.Empty,
                    Status = x.Status,
                    DueDate = x.DueDate,
                    CreatedDate = x.CreatedDate
                })
                .ToListAsync();
        }
        public async Task<TaskFilterViewModel> GetTasksAsync( int? employeeId, EmployeeTaskStatus? status, DateTime? fromDate, DateTime? toDate,  int page, int pageSize)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (pageSize <= 0)
            {
                pageSize = 10;
            }

            var query = _context.EmployeeTasks
                .AsNoTracking()
                .AsQueryable();

            if (employeeId.HasValue)
            {
                query = query.Where(x => x.EmployeeId == employeeId.Value);
            }

            if (status.HasValue)
            {
                query = query.Where(x =>
                    x.Status == status.Value);
            }


            if (fromDate.HasValue)
            {
                query = query.Where(x =>
                    x.DueDate >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(x =>
                    x.DueDate <= toDate.Value);
            }


            var totalItems = await query.CountAsync();

            var tasks = await query
                .OrderByDescending(x => x.CreatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new EmployeeTaskViewModel
                {
                    Id = x.Id,

                    Title = x.Title,

                    Description = x.Description,

                    EmployeeId = x.EmployeeId,

                    EmployeeName = x.Employee != null
                        ? x.Employee.Name
                        : string.Empty,

                    Status = x.Status,

                    DueDate = x.DueDate,

                    CreatedDate = x.CreatedDate
                })
                .ToListAsync();

            var employees = await _context.Employees
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .Select(x => new EmployeeViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();


            return new TaskFilterViewModel
            {
                EmployeeId = employeeId,

                Status = status,

                FromDate = fromDate,

                ToDate = toDate,

                Tasks = tasks,

                Employees = employees,

                Pagination = new PaginationViewModel
                {
                    CurrentPage = page,

                    PageSize = pageSize,

                    TotalItems = totalItems
                }
            };
        }

        public async Task<TaskDetailsViewModel?> GetByIdAsync(int id)
        {
            return await _context.EmployeeTasks
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new TaskDetailsViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    EmployeeId = x.EmployeeId,
                    EmployeeName =
                        x.Employee != null
                            ? x.Employee.Name
                            : string.Empty,
                    Status = x.Status,
                    DueDate = x.DueDate,
                    CreatedDate = x.CreatedDate
                })
                .FirstOrDefaultAsync();
        }

        public async Task<ServiceResult> CreateAsync( EmployeeTaskViewModel model)
        {
            var employee = await _context.Employees
                    .FirstOrDefaultAsync(x =>
                        x.Id == model.EmployeeId);

            if (employee == null)
            {
                return ServiceResult.Fail("Selected employee does not exist.");
            }

            if (!employee.IsActive)
            {
                return ServiceResult.Fail("Task can only be assigned to an active employee.");
            }

            var task = new EmployeeTask
            {
                Title = model.Title.Trim(),
                Description = model.Description?.Trim(),
                EmployeeId = model.EmployeeId,
                Status = model.Status,
                DueDate = model.DueDate,
                CreatedDate = DateTime.Now
            };

            _context.EmployeeTasks.Add(task);

            await _context.SaveChangesAsync();

            return ServiceResult.Ok("Task created successfully.");
        }

        public async Task<ServiceResult> UpdateAsync(EmployeeTaskViewModel model)
        {
            var task = await _context.EmployeeTasks
                    .FirstOrDefaultAsync(x =>
                        x.Id == model.Id);

            if (task == null)
            {
                return ServiceResult.Fail("Task not found.");
            }

            var employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == model.EmployeeId);

            if (employee == null)
            {
                return ServiceResult.Fail("Selected employee does not exist.");
            }

            if (!employee.IsActive)
            {
                return ServiceResult.Fail("Task can only be assigned to an active employee.");
            }

            task.Title = model.Title.Trim();
            task.Description = model.Description?.Trim();
            task.EmployeeId = model.EmployeeId;
            task.Status = model.Status;
            task.DueDate = model.DueDate;

            await _context.SaveChangesAsync();

            return ServiceResult.Ok("Task updated successfully.");
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var task = await _context.EmployeeTasks.FirstOrDefaultAsync(x => x.Id == id);

            if (task == null)
            {
                return ServiceResult.Fail("Task not found.");
            }

            _context.EmployeeTasks.Remove(task);

            await _context.SaveChangesAsync();

            return ServiceResult.Ok("Task deleted successfully.");
        }

        public async Task<TaskFilterViewModel> SearchAsync(TaskFilterViewModel filter)
        {
            IQueryable<EmployeeTask> query = _context.EmployeeTasks
                                            .AsNoTracking()
                                            .Include(x => x.Employee);

            if (filter.EmployeeId.HasValue)
            {
                query = query.Where(x => x.EmployeeId == filter.EmployeeId.Value);
            }

            if (filter.Status.HasValue)
            {
                query = query.Where(x => x.Status == filter.Status.Value);
            }

            if (filter.FromDate.HasValue)
            {
                var fromDate = filter.FromDate.Value.Date;

                query = query.Where(x =>  x.DueDate >= fromDate);
            }

            if (filter.ToDate.HasValue)
            {
                var toDate = filter.ToDate.Value.Date.AddDays(1);

                query = query.Where(x => x.DueDate < toDate);
            }

            filter.Tasks =   await query
                            .OrderByDescending(x => x.Id)
                            .Select(x => new EmployeeTaskViewModel
                            {
                                Id = x.Id,
                                Title = x.Title,
                                Description = x.Description,
                                EmployeeId = x.EmployeeId,
                                EmployeeName =
                                    x.Employee != null
                                        ? x.Employee.Name
                                        : string.Empty,
                                Status = x.Status,
                                DueDate = x.DueDate,
                                CreatedDate = x.CreatedDate
                            })
                            .ToListAsync();
            return filter;
        }

        public async Task<EmployeeTaskViewModel?> GetForEditAsync(int id)
        {
            return await _context.EmployeeTasks
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new EmployeeTaskViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    EmployeeId = x.EmployeeId,
                    Status = x.Status,
                    DueDate = x.DueDate,
                    CreatedDate = x.CreatedDate,
                    EmployeeName = x.Employee != null
                        ? x.Employee.Name
                        : string.Empty
                })
                .FirstOrDefaultAsync();
        }
        public async Task<EmployeeViewModel> GetEmployeeInfoAsync(int employeeId)
        {
            var employee = await _context.Employees
                .Where(x => x.Id == employeeId)
                .Select(x => new EmployeeViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    Department = x.Department,
                    IsActive = x.IsActive
                })
                .FirstOrDefaultAsync();

            return employee;
        }
    }
}

