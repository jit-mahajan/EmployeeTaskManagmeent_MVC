using System.ComponentModel.DataAnnotations;

namespace EmployeeManagmeent.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Employee name is required.")]
        [StringLength(100)]
        [Display(Name = "Employee Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email.")]
        [StringLength(150)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Department is required.")]
        [StringLength(100)]
        public string Department { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
