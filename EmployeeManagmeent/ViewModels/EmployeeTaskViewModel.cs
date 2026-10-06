using EmployeeManagmeent.Models;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagmeent.ViewModels
{
    public class EmployeeTaskViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please select an employee.")]
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Please select status.")]
        public EmployeeTaskStatus Status { get; set; }

        [Required(ErrorMessage = "Due date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public string? EmployeeName { get; set; }
    }
}
