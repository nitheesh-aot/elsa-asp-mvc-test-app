﻿using EmployeeOnboarding.Web.Data;
using EmployeeOnboarding.Web.Entities;
using EmployeeOnboarding.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeOnboarding.Web.Controllers;

[ApiController]
[Route("api/webhooks")]
public class WebhookController : Controller
{
    private readonly OnboardingDbContext _dbContext;

    public WebhookController(OnboardingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost("run-task")]
    public async Task<IActionResult> RunTask(WebhookEvent webhookEvent)
    {
        var payload = webhookEvent.Payload;
        var taskPayload = payload.TaskPayload;
        var employee = taskPayload.Employee;

        var task = new OnboardingTask
        {
            ProcessId = payload.WorkflowInstanceId,
            ExternalId = payload.TaskId,
            Name = payload.TaskName,
            Description = taskPayload.Description,
            EmployeeEmail = employee.Email,
            EmployeeName = employee.Name,
            CreatedAt = DateTimeOffset.Now
        };

        await _dbContext.Tasks.AddAsync(task);
        await _dbContext.SaveChangesAsync();

        return Ok();
    }
}
