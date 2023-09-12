using NewJobSurveyAdmin.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewJobSurveyAdmin.Services
{
    public class EmployeeReconciliationService
    {
        private LoggingService logger;
        private EmployeeCreationService creationService;
        private EmployeeBlackoutPeriodService blackoutPeriodService;
        private EmployeeUpdateService updateService;

        public EmployeeReconciliationService(
            LoggingService logger,
            EmployeeCreationService creationService,
            EmployeeBlackoutPeriodService blackoutPeriodService,
            EmployeeUpdateService updateService
        )
        {
            this.logger = logger;
            this.creationService = creationService;
            this.blackoutPeriodService = blackoutPeriodService;
            this.updateService = updateService;
        }

        public async Task<EmployeeTaskResult> InsertEmployeesAndLog(
            TaskEnum callingTask,
            List<Employee> candidateEmployees
        )
        {
            var taskResult = await creationService.InsertEmployees(candidateEmployees);
            taskResult.Task = callingTask;
            await logger.LogEmployeeTaskResult(taskResult);
            return taskResult;
        }

        public async Task<EmployeeTaskResult> UpdateBlackoutPeriodsAndLog()
        {
            var taskResult = await blackoutPeriodService.UpdateBlackoutPeriods();
            await logger.LogEmployeeTaskResult(taskResult);
            return taskResult;
        }

        public async Task<EmployeeTaskResult> UpdateEmployeeStatusesAndLog()
        {
            var taskResult = await updateService.RefreshCallWebStatus();
            await logger.LogEmployeeTaskResult(taskResult);
            return taskResult;
        }
    }
}
