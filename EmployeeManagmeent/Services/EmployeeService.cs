using EmployeeManagmeent.Data;
using EmployeeManagmeent.Models;
using EmployeeManagmeent.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagmeent.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly MyDbContext _context;

        public EmployeeService(MyDbContext context)
        {
            _context = context;
        }

        public async Task<List<EmployeeViewModel>> GetAllAsync()
        {
            return await _context.Employees
                .AsNoTracking()
                .OrderByDescending(x => x.Id)
                .Select(x => new EmployeeViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    Department = x.Department,
                    IsActive = x.IsActive,
                    CreatedDate = x.CreatedDate
                })
                .ToListAsync();
        }

        public async Task<EmployeeListViewModel> GetEmployeesAsync( int page, int pageSize)
        {
            if (page < 1)
                page = 1;

            var query = _context.Employees.AsNoTracking().OrderBy(x => x.Id);

            var totalItems = await query.CountAsync();

            var employees = await query
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .Select(x => new EmployeeViewModel
                            {
                                Id = x.Id,
                                Name = x.Name,
                                Email = x.Email,
                                Department = x.Department,
                                IsActive = x.IsActive
                            })
                            .ToListAsync();

            return new EmployeeListViewModel
            {
                Employees = employees,

                Pagination = new PaginationViewModel
                {
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalItems = totalItems
                }
            };
        }

        public async Task<EmployeeViewModel?> GetByIdAsync(int id)
        {
            return await _context.Employees
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new EmployeeViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    Department = x.Department,
                    IsActive = x.IsActive,
                    CreatedDate = x.CreatedDate
                })
                .FirstOrDefaultAsync();
        }

        public async Task<EmployeeDetailsViewModel?> GetDetailsAsync(int id)
        {
            var employee =  await _context.Employees
                        .AsNoTracking()
                        .Include(x => x.Tasks)
                        .FirstOrDefaultAsync(x => x.Id == id);

            if (employee == null)
            {
                return null;
            }

            return new EmployeeDetailsViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Department = employee.Department,
                IsActive = employee.IsActive,
                CreatedDate = employee.CreatedDate,

                Tasks = employee.Tasks
                    .OrderByDescending(x => x.Id)
                    .Select(x => new EmployeeTaskViewModel
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Description = x.Description,
                        EmployeeId = x.EmployeeId,
                        EmployeeName = employee.Name,
                        Status = x.Status,
                        DueDate = x.DueDate,
                        CreatedDate = x.CreatedDate
                    })
                    .ToList()
            };
        }

        public async Task<ServiceResult> CreateAsync(EmployeeViewModel model)
        {
            var emailExists =  await _context.Employees.AnyAsync(x => x.Email == model.Email);

            if (emailExists)
            {
                return ServiceResult.Fail(
                    "An employee with this email already exists.");
            }

            var employee = new Employee
            {
                Name = model.Name.Trim(),
                Email = model.Email.Trim(),
                Department = model.Department.Trim(),
                IsActive = model.IsActive,
                CreatedDate = DateTime.Now
            };

            _context.Employees.Add(employee);

            await _context.SaveChangesAsync();

            return ServiceResult.Ok(
                "Employee created successfully.");
        }

        public async Task<ServiceResult> UpdateAsync(EmployeeViewModel model)
        {
            var employee =  await _context.Employees .FirstOrDefaultAsync(x =>x.Id == model.Id);

            if (employee == null)
            {
                return ServiceResult.Fail(
                    "Employee not found.");
            }

            var emailExists = await _context.Employees .AnyAsync(x =>  x.Email == model.Email && x.Id != model.Id);

            if (emailExists)
            {
                return ServiceResult.Fail( "Another employee already uses this email.");
            }

            employee.Name = model.Name.Trim();
            employee.Email = model.Email.Trim();
            employee.Department = model.Department.Trim();
            employee.IsActive = model.IsActive;

            await _context.SaveChangesAsync();

            return ServiceResult.Ok("Employee updated successfully.");
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var employee = await _context.Employees
                    .Include(x => x.Tasks)
                    .FirstOrDefaultAsync(x => x.Id == id);

            if (employee == null)
            {
                return ServiceResult.Fail("Employee not found.");
            }

            if (employee.Tasks.Any())
            {
                return ServiceResult.Fail("Employee cannot be deleted because tasks are assigned to this employee.");
            }

            _context.Employees.Remove(employee);

            await _context.SaveChangesAsync();

            return ServiceResult.Ok("Employee deleted successfully.");
        }

        public async Task<List<EmployeeViewModel>> GetActiveEmployeesAsync()
        {
            return await _context.Employees
                .AsNoTracking()
                .Where(x => x.IsActive)
                .OrderBy(x => x.Name)
                .Select(x => new EmployeeViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    Department = x.Department,
                    IsActive = x.IsActive,
                    CreatedDate = x.CreatedDate
                })
                .ToListAsync();
        }

        public async Task<EmployeeAjaxViewModel?> GetEmployeeForAjaxAsync(int id)
        {
            return await _context.Employees
                        .AsNoTracking()
                        .Where(x => x.Id == id)
                        .Select(x => new EmployeeAjaxViewModel
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Email = x.Email,
                            Department = x.Department,
                            IsActive = x.IsActive
                        })
                        .FirstOrDefaultAsync();
        }
    }
}

