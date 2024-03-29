using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewJobSurveyAdmin.Models;
using NewJobSurveyAdmin.Services;
using NewJobSurveyAdmin.Services.CallWeb;
using NewJobSurveyAdmin.Services.CsvService;
using NewJobSurveyAdmin.Services.PsaApi;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewJobSurveyAdmin.Controllers
{
    [Authorize(Policy = "UserRole")]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly NewJobSurveyAdminContext context;
        private readonly SieveProcessor sieveProcessor;
        private readonly EmployeeReconciliationService employeeReconciler;
        private readonly CallWebService callWebService;
        private readonly LoggingService logger;
        private readonly CsvService csvService;
        private readonly PsaApiService psaApiService;
        private readonly EmailService emailService;

        public EmployeesController(
            NewJobSurveyAdminContext context,
            SieveProcessor sieveProcessor,
            EmployeeReconciliationService employeeReconciler,
            CallWebService callWebService,
            LoggingService loggingService,
            CsvService csvService,
            PsaApiService psaApiService,
            EmailService emailService
        )
        {
            this.context = context;
            this.sieveProcessor = sieveProcessor;
            this.employeeReconciler = employeeReconciler;
            this.callWebService = callWebService;
            this.csvService = csvService;
            this.logger = loggingService;
            this.psaApiService = psaApiService;
            this.emailService = emailService;
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
            var employees = context.Employees.AsNoTracking().Include(e => e.TimelineEntries);

            var sievedEmployees = await sieveProcessor.GetPagedAsync(employees, sieveModel);

            Response.Headers.Add("X-Pagination", sievedEmployees.SerializeMetadataToJson());

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

        // GET: api/Employees/Values/StaffingReason
        [HttpGet("Values/StaffingReason")]
        public async Task<ActionResult<List<string>>> ValuesStaffingReason()
        {
            var uniqueValues = await context.Employees
                .Select(e => e.StaffingReason)
                .Distinct()
                .ToListAsync();

            uniqueValues.Sort();

            return uniqueValues;
        }

        // GET: api/Employees/Values/NewHireOrInternalStaffing
        [HttpGet("Values/NewHireOrInternalStaffing")]
        public async Task<ActionResult<List<string>>> ValuesNewHireOrInternalStaffing()
        {
            var uniqueValues = await context.Employees
                .Select(e => e.NewHireOrInternalStaffing)
                .Distinct()
                .ToListAsync();

            uniqueValues.Sort();

            return uniqueValues;
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
                await callWebService.UpdateSurveys(new List<Employee>() { updatedEmployee });

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
        public async Task<ActionResult<List<Employee>>> EmployeesFromPsaApi(
            int startIndex,
            int count
        )
        {
            try
            {
                // Get a list of candidate Employee objects based on the PSA
                // API. They will be in JSON format.
                var currentEmployees = await psaApiService.GetCurrent();

                List<Employee> employeesToLoad;

                if (startIndex > -1 && count > 0)
                {
                    // TODO: Validate.
                    employeesToLoad = currentEmployees.Succeeded.GetRange(startIndex, count);
                }
                else
                {
                    employeesToLoad = currentEmployees.Succeeded;
                }

                // Reconcile the employees with the database.
                var taskResult = await employeeReconciler.InsertEmployeesAndLog(
                    TaskEnum.LoadFromJson,
                    employeesToLoad
                );

                emailService.SendTaskResultEmail(taskResult);

                return Ok(taskResult.Succeeded);
            }
            catch (Exception exception)
            {
                emailService.SendFailureEmail(TaskEnum.ParsePsa, exception);

                return await ApiResponseHelper.LogFailureAndSendStacktrace(
                    this,
                    TaskEnum.ParsePsa,
                    exception,
                    logger
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
                var taskResult = await employeeReconciler.InsertEmployeesAndLog(
                    TaskEnum.LoadFromJson,
                    employees
                );
                return Ok(taskResult.Succeeded);
            }
            catch (Exception e)
            {
                return await ApiResponseHelper.LogFailureAndSendStacktrace(
                    this,
                    TaskEnum.LoadFromJson,
                    e,
                    logger
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
                var taskResult = await employeeReconciler.InsertEmployeesAndLog(
                    TaskEnum.LoadFromCsv,
                    readResult.Succeeded
                );
                return Ok(taskResult.Succeeded);
            }
            catch (Exception e)
            {
                return await ApiResponseHelper.LogFailureAndSendStacktrace(
                    this,
                    TaskEnum.LoadFromCsv,
                    e,
                    logger
                );
            }
        }

        [HttpPost("RefreshEmployeeStatus")]
        public async Task<ActionResult> RefreshEmployeeStatus()
        {
            try
            {
                // Update existing employee statuses.
                var taskResult = await employeeReconciler.UpdateEmployeeStatusesAndLog();

                return Ok();
            }
            catch (Exception e)
            {
                return await ApiResponseHelper.LogFailureAndSendStacktrace(
                    this,
                    TaskEnum.RefreshStatuses,
                    e,
                    logger
                );
            }
        }

        [AllowAnonymous]
        [HttpPost("ScheduledLoadAndUpdate")]
        public async Task<ActionResult> ScheduledLoadAndUpdate(int startIndex, int count)
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
                    await EmployeesFromPsaApi(startIndex, count);
                }

                await logger.LogSuccess(
                    TaskEnum.ScheduledTask,
                    "Scheduled load and update ran successfully."
                );

                return Ok();
            }
            catch (Exception e)
            {
                return await ApiResponseHelper.LogFailureAndSendStacktrace(
                    this,
                    TaskEnum.ScheduledTask,
                    e,
                    logger
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
