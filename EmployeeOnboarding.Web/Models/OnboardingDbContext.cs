using EmployeeOnboarding.Web.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeOnboarding.Web.Data;

public class OnboardingDbContext : DbContext
{
    public OnboardingDbContext(DbContextOptions<OnboardingDbContext> options) : base(options)
    {
    }

    public DbSet<OnboardingTask> Tasks { get; set; } = default!;
}