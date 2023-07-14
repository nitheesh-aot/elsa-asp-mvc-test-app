using EmployeeOnboarding.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeeOnboarding.Web.HostedServices;

public class MigrationsHostedService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public MigrationsHostedService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<OnboardingDbContext>>();
        await using var dbContext = dbContextFactory.CreateDbContext();
        await dbContext.Database.MigrateAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}