using System.ComponentModel.DataAnnotations;

namespace EmployeeManagmeent.Models
{
    public class Admin
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(150)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
