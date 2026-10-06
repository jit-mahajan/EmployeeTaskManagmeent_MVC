using EmployeeManagmeent.Models;

namespace EmployeeManagmeent.ViewModels
{
    public class TaskFilterViewModel
    {
        public int? EmployeeId { get; set; }

        public EmployeeTaskStatus? Status { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public List<EmployeeTaskViewModel> Tasks { get; set; }
            = new List<EmployeeTaskViewModel>();

        public List<EmployeeViewModel> Employees { get; set; }
            = new List<EmployeeViewModel>();

        public PaginationViewModel Pagination { get; set; }
            = new PaginationViewModel();
    }
}
