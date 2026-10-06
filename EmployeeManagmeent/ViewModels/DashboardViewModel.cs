namespace EmployeeManagmeent.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalEmployees { get; set; }

        public int ActiveEmployees { get; set; }

        public int TotalTasks { get; set; }

        public int PendingTasks { get; set; }

        public int InProgressTasks { get; set; }

        public int CompletedTasks { get; set; }

        public List<EmployeeTaskViewModel> RecentTasks { get; set; } = new List<EmployeeTaskViewModel>();
    }
}
