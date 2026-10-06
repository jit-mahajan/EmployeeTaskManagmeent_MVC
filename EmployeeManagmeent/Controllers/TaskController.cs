using EmployeeManagmeent.Models;
using EmployeeManagmeent.Services;
using EmployeeManagmeent.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeManagmeent.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly IEmployeeService _employeeService;

        public TaskController( ITaskService taskService, IEmployeeService employeeService)
        {
            _taskService = taskService;
            _employeeService = employeeService;
        }

        // GET: /Task
        public async Task<IActionResult> Index( int? employeeId, EmployeeTaskStatus? status,  DateTime? fromDate,  DateTime? toDate,  int page = 1,   int pageSize = 10)
        {
            var model = await _taskService.GetTasksAsync(employeeId, status, fromDate, toDate, page, pageSize);

            return View(model);
        }

        // GET: /Task/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var task = await _taskService.GetByIdAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // GET: /Task/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadEmployeesAsync();

            var model = new EmployeeTaskViewModel
            {
                Status = EmployeeTaskStatus.Pending,
                DueDate = DateTime.Today.AddDays(1)
            };

            ViewData["FormMode"] = "Create";

            return View(model);
        }

        // POST: /Task/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeTaskViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadEmployeesAsync();

                return View(model);
            }

            var result =  await _taskService.CreateAsync(model);

            if (!result.Success)
            {
                ModelState.AddModelError("EmployeeId", result.Message);

                await LoadEmployeesAsync();

                return View(model);
            }

            TempData["SuccessMessage"] = "Task created successfully.";

            return RedirectToAction(nameof(Index));
        }

        // GET: /Task/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var task =
                await _taskService.GetForEditAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            await LoadEmployeesAsync();

            return View(task);
        }

        // POST: /Task/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmployeeTaskViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                await LoadEmployeesAsync();

                return View(model);
            }

            var result = await _taskService.UpdateAsync(model);

            if (!result.Success)
            {
                ModelState.AddModelError("EmployeeId", result.Message);

                await LoadEmployeesAsync();

                return View(model);
            }

            TempData["SuccessMessage"] = "Task updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        // GET: /Task/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _taskService.GetByIdAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // POST: /Task/DeleteConfirmed/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result =
                await _taskService.DeleteAsync(id);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;

                return RedirectToAction(nameof(Index));
            }

            TempData["SuccessMessage"] = "Task deleted successfully.";

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadEmployeesAsync()
        {
            var employees =await _employeeService.GetActiveEmployeesAsync();

            ViewBag.Employees = new SelectList(employees, "Id", "Name");
        }
        [HttpGet]
        public async Task<IActionResult> GetEmployeeInfo(int id)
        {
            var employee = await _taskService.GetEmployeeInfoAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return Json(employee);
        }
    }
}
