namespace EmployeeManagmeent.ViewModels
{
    public class EmployeeDetailsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Department { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public List<EmployeeTaskViewModel> Tasks { get; set; }
    }
}
