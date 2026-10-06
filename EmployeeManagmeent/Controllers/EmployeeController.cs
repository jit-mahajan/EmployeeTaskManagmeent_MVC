using EmployeeManagmeent.Services;
using EmployeeManagmeent.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagmeent.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController( IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        // GET: /Employee
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var model = await _employeeService.GetEmployeesAsync(page, pageSize);

            return View(model);
        }

        // GET: /Employee/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var employee =  await _employeeService.GetDetailsAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: /Employee/Create
        [HttpGet]
        public IActionResult Create()
        {
            var model = new EmployeeViewModel
            {
                IsActive = true
            };

            ViewData["FormMode"] = "Create";

            return View(model);
        }

        // POST: /Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["FormMode"] = "Create";

                return View(model);
            }

            var result = await _employeeService.CreateAsync(model);

            if (!result.Success)
            {
                ModelState.AddModelError( "Email", result.Message);

                return View(model);
            }

            TempData["SuccessMessage"] = "Employee created successfully.";

            return RedirectToAction(nameof(Index));
        }

        // GET: /Employee/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _employeeService.GetByIdAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            ViewData["FormMode"] = "Edit";

            return View(employee);
        }

        // POST: /Employee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            EmployeeViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                ViewData["FormMode"] = "Edit";

                return View(model);
            }

            var result = await _employeeService.UpdateAsync(model);

            if (!result.Success)
            {
                ModelState.AddModelError("Email", result.Message);

                return View(model);
            }

            TempData["SuccessMessage"] ="Employee updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        // GET: /Employee/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _employeeService.GetByIdAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: /Employee/DeleteConfirmed/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(
            int id)
        {
            var result = await _employeeService.DeleteAsync(id);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            TempData["SuccessMessage"] = "Employee deleted successfully.";

            return RedirectToAction(nameof(Index));
        }

        // AJAX
        [HttpGet]
        public async Task<IActionResult>
            GetEmployeeDetails(int id)
        {
            var employee =  await _employeeService.GetEmployeeForAjaxAsync(id);

            if (employee == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Employee not found."
                });
            }

            return Json(new
            {
                success = true,
                data = employee
            });
        }
    }
}
       

