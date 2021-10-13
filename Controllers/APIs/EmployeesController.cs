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
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly NewJobSurveyAdminContext context;
        private readonly SieveProcessor sieveProcessor;
        private readonly EmployeeInfoLookupService employeeInfoLookup;
        private readonly EmployeeReconciliationService employeeReconciler;
        private readonly CallWebService callWebService;
        private readonly LoggingService logger;
        private readonly CsvService csvService;
        private readonly PsaApiService psaApiService;

        public EmployeesController(
            NewJobSurveyAdminContext context,
            SieveProcessor sieveProcessor,
            EmployeeInfoLookupService employeeInfoLookup,
            EmployeeReconciliationService employeeReconciler,
            CallWebService callWebService,
            LoggingService loggingService,
            CsvService csvService,
            PsaApiService psaApiService
        )
        {
            this.context = context;
            this.sieveProcessor = sieveProcessor;
            this.employeeInfoLookup = employeeInfoLookup;
            this.employeeReconciler = employeeReconciler;
            this.callWebService = callWebService;
            this.csvService = csvService;
            this.logger = loggingService;
            this.psaApiService = psaApiService;
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

            var sievedEmployees = await sieveProcessor
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
                // Get a list of candidate Employee objects based on the CSV.
                var currentEmployees = await psaApiService.GetCurrent();

                var newEmployees = currentEmployees;

                // Reconcile the employees with the database.
                var taskResult = await employeeReconciler.InsertEmployeesAndLog(newEmployees);
                await logger.LogSuccess(TaskEnum.LoadPsa, $"EmployeesFromPsaApi: Success.");
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
                var taskResult = await employeeReconciler.UpdateEmployeeStatusesAndLog();
                await logger.LogSuccess(TaskEnum.RefreshStatuses,
                    $"Triggered refresh of employee statuses."
                );

                return Ok();
            }
            catch (Exception e)
            {
                await logger.LogFailure(TaskEnum.RefreshStatuses,
                    $"Error refreshing employee statuses: {e.Message} Stacktrace:\r\n" +
                    e.StackTrace
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