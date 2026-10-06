using EmployeeManagmeent.Models;

namespace EmployeeManagmeent.ViewModels
{
    public class TaskDetailsViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public EmployeeTaskStatus Status { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
