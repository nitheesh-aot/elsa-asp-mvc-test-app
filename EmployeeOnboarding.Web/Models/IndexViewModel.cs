using EmployeeOnboarding.Web.Entities;

namespace EmployeeOnboarding.Web.Views.Home;

public class IndexViewModel
{
    public IndexViewModel(ICollection<OnboardingTask> tasks)
    {
        Tasks = tasks;
    }

    public ICollection<OnboardingTask> Tasks { get; set; }
}
