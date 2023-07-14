using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EmployeeOnboarding.Web.Models;
using EmployeeOnboarding.Web.Data;
using EmployeeOnboarding.Web.Views.Home;
using Microsoft.EntityFrameworkCore;
using EmployeeOnboarding.Web.Services;

namespace EmployeeOnboarding.Web.Controllers;

public class HomeController : Controller
{
    private readonly OnboardingDbContext _dbContext;
    private readonly ElsaClient _elsaClient;

    public HomeController(OnboardingDbContext dbContext, ElsaClient elsaClient)
    {
        _dbContext = dbContext;
        _elsaClient = elsaClient;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var tasks = await _dbContext.Tasks.Where(x => !x.IsCompleted).ToListAsync(cancellationToken: cancellationToken);
        var model = new IndexViewModel(tasks);
        return View(model);
    }

    public async Task<IActionResult> CompleteTask(int taskId, CancellationToken cancellationToken)
    {
        var task = _dbContext.Tasks.FirstOrDefault(x => x.Id == taskId);

        if (task == null)
            return NotFound();

        await _elsaClient.ReportTaskCompletedAsync(task.ExternalId, cancellationToken: cancellationToken);

        task.IsCompleted = true;
        task.CompletedAt = DateTimeOffset.Now;

        _dbContext.Tasks.Update(task);
        await _dbContext.SaveChangesAsync(cancellationToken);


        return RedirectToAction("Index");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

