using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewJobSurveyAdmin.Models;
using NewJobSurveyAdmin.Services;
using NewJobSurveyAdmin.Services.CallWeb;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewJobSurveyAdmin.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly NewJobSurveyAdminContext context;
        private readonly SieveProcessor SieveProcessor;
        private readonly EmployeeInfoLookupService EmployeeInfoLookup;
        private readonly EmployeeReconciliationService EmployeeReconciler;
        private readonly CallWebService callWebService;
        private readonly LoggingService logger;

        public EmployeesController(
            NewJobSurveyAdminContext context,
            SieveProcessor sieveProcessor,
            EmployeeInfoLookupService employeeInfoLookup,
            EmployeeReconciliationService employeeReconciler,
            CallWebService callWebService,
            LoggingService loggingService
        )
        {
            this.context = context;
            SieveProcessor = sieveProcessor;
            EmployeeInfoLookup = employeeInfoLookup;
            EmployeeReconciler = employeeReconciler;
            this.callWebService = callWebService;
            logger = loggingService;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IList<Employee>>> GetEmployees(
            [FromQuery] SieveModel sieveModel
        )
        {
            // Validate the page size and page.
            if (sieveModel.PageSize < 1)
            {
                throw new ArgumentOutOfRangeException("Page size must be >= 1.");
            }

            if (sieveModel.Page < 1)
            {
                throw new ArgumentOutOfRangeException("Page must be >= 1.");
            }

            // Employee query.
            var employees = context.Employees
                .AsNoTracking()
                .Include(e => e.TimelineEntries);

            var sievedEmployees = await SieveProcessor
                .GetPagedAsync(employees, sieveModel);
            Response.Headers.Add("X-Pagination", sievedEmployees
                .SerializeMetadataToJson());

            return Ok(sievedEmployees.Results);
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await FindById(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        // PATCH: api/Employees/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchEmployee(int id, EmployeePatchDto employeePatchDto)
        {
            var existingEmployee = await FindById(id);

            var updatedEmployee = employeePatchDto.ApplyPatch(existingEmployee);

            context.Entry(updatedEmployee).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();

                // Patch the row in CallWeb.
                await callWebService.UpdateSurvey(updatedEmployee);

                return Ok(updatedEmployee);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // EmployeesFromPsaApi: Load employees from the PSA API and immediately
        // try to insert them.
        // POST: api/Employees/FromPsaApi
        [HttpPost("FromPsaApi")]
        public async Task<ActionResult<List<Employee>>> EmployeesFromPsaApi()
        {
            try
            {
                // Get a list of candidate Employee objects based on the PSA API.
                var currentEmployees = await psaApiService.GetCurrent();

                // Reconcile the employees with the database.
                var taskResult = await employeeReconciler.InsertEmployeesAndLog(currentEmployees.GetRange(10, 5));
                await logger.LogSuccess(TaskEnum.LoadPsa, $"EmployeesFromPsaApi: Success.");
                return Ok(taskResult.GoodEmployees);

            }
            catch (Exception e)
            {
                await logger.LogFailure(TaskEnum.LoadPsa,
                    $"Error reconciling employee records: {e.Message} Stacktrace:\r\n" +
                    e.StackTrace
                );
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new { message = e.StackTrace }
                );
            }
        }

        // EmployeesFromJson: Given incomplete Employees in JSON format (as
        // obtained, for instance, from the PSA API), reconcile those employees.
        // POST: api/Employees/FromJson
        [HttpPost("FromJson")]
        public async Task<ActionResult<List<Employee>>> EmployeesFromJson(List<Employee> employees)
        {
            try
            {
                // Reconcile the employees with the database.
                var taskResult = await employeeReconciler.InsertEmployeesAndLog(employees);
                await logger.LogSuccess(TaskEnum.LoadFromJson, $"EmployeesFromJson: Success.");
                return Ok(taskResult.GoodEmployees);
            }
            catch (Exception e)
            {
                await logger.LogFailure(TaskEnum.LoadFromJson,
                    $"EmployeesFromJson: Error reconciling employee records: {e.Message} Stacktrace:\r\n" +
                    e.StackTrace
                );
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new { message = e.StackTrace }
                );
            }
        }

        // EmployeesFromCsv: Given the raw text of the PSA CSV extract (as
        // obtained, for instance, from the PSA CSV file drop), transform it
        // into an array of nicely-formatted Employee JSON objects, then
        // reconcile each of those Employees.
        // POST: api/Employees/FromCsv
        [HttpPost("FromCsv")]
        public async Task<ActionResult<List<Employee>>> EmployeesFromCsv()
        {
            try
            {
                // Get a list of candidate Employee objects based on the CSV.
                var readResult = await csvService.ProcessCsvAndLog(Request);

                // Reconcile the employees with the database.
                var taskResult = await employeeReconciler.InsertEmployeesAndLog(readResult.GoodEmployees);
                await logger.LogSuccess(TaskEnum.LoadFromCsv, $"EmployeesFromCsv: Success.");
                return Ok(taskResult.GoodEmployees);

            }
            catch (Exception e)
            {
                await logger.LogFailure(TaskEnum.LoadFromCsv,
                    $"Error reconciling employee records: {e.Message} Stacktrace:\r\n" +
                    e.StackTrace
                );
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new { message = e.StackTrace }
                );
            }
        }

        [HttpPost("RefreshEmployeeStatus")]
        public async Task<ActionResult> RefreshEmployeeStatus()
        {
            try
            {
                // Update existing employee statuses.
                await EmployeeReconciler.UpdateEmployeeStatuses();

                await logger.LogSuccess(TaskEnum.RefreshStatuses,
                    $"Triggered refresh of employee statuses."
                );
            }
            catch (Exception e)
            {
                await logger.LogFailure(TaskEnum.ReconcileCsv,
                    $"Error refreshing employee statuses. Stacktrace:\r\n" +
                    e.StackTrace
                );
            }

            return Ok();
        }

        [HttpPost("ScheduledLoadAndUpdate")]
        public async Task<ActionResult> ScheduledLoadAndUpdate()
        {
            try
            {
                // In all cases, update existing employee statuses.
                await RefreshEmployeeStatus();

                // Also update the blackout periods, if required.
                await employeeReconciler.UpdateBlackoutPeriodsAndLog();

                // Get the day of the week to pull data on.
                var pullDayOfWeek = await context.AdminSettings.FirstOrDefaultAsync(
                    i => i.Key.Equals(AdminSetting.DataPullDayOfWeek)
                );
                int dataPullDayOfWeek = Convert.ToInt32(pullDayOfWeek.Value);

                if (dataPullDayOfWeek == (int)DateTime.Today.DayOfWeek)
                {
                    // If today is the same day of the week as a pull day, pull
                    // the data.
                    await EmployeesFromPsaApi();
                }

                await logger.LogSuccess(
                    TaskEnum.ScheduledTask,
                    "Scheduled load and update ran successfully.");

                return Ok();
            }
            catch (Exception e)
            {
                await logger.LogFailure(TaskEnum.ScheduledTask,
                    $"Error during scheduled load and update: {e.Message} " +
                    "Stacktrace:\r\n" + e.StackTrace
                );

                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new { message = e.StackTrace }
                );
            }
        }

        private bool EmployeeExists(int id)
        {
            return context.Employees.Any(e => e.Id == id);
        }

        private async Task<Employee> FindById(int id)
        {
            var employee = await context.Employees
                .Include(e => e.TimelineEntries)
                .FirstOrDefaultAsync(i => i.Id == id);

            return employee;
        }
    }
}