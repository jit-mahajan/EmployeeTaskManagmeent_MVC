using EmployeeManagmeent.Models;

namespace EmployeeManagmeent.ViewModels
{
    public class EmployeeListViewModel
    {
        public List<EmployeeViewModel> Employees { get; set; }
        public PaginationViewModel Pagination { get; set; }

    }
}
