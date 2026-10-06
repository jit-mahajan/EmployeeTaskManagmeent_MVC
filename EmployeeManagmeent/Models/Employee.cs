using System.ComponentModel.DataAnnotations;

namespace EmployeeManagmeent.Models
{
    public class Employee
    {
        public Employee()
        {
            Tasks = new HashSet<EmployeeTask>();
            CreatedDate = DateTime.Now;
            IsActive = true;
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Employee name is required.")]
        [StringLength(100)]
        [Display(Name = "Employee Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        [StringLength(150)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Department is required.")]
        [StringLength(100)]
        public string Department { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<EmployeeTask> Tasks { get; set; }


    }
}
