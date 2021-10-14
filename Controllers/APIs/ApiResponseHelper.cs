using Microsoft.AspNetCore.Http;
using NewJobSurveyAdmin.Models;
using NewJobSurveyAdmin.Services;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewJobSurveyAdmin.Services.CallWeb;
using NewJobSurveyAdmin.Services.CsvService;
using NewJobSurveyAdmin.Services.PsaApi;
using Sieve.Models;
using Sieve.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewJobSurveyAdmin
{

    public class ApiResponseHelper : ControllerBase
    {

        public static async Task<ObjectResult> LogFailureAndSendStacktrace(
            ControllerBase controllerBase,
            TaskEnum task,
            Exception exception,
            LoggingService logger
        )
        {
            var message =
                $"Error: {exception.Message} " +
                $"Stacktrace:\r\n {exception.StackTrace}";

            await logger.LogFailure(task, message);

            return controllerBase.StatusCode(
                StatusCodes.Status500InternalServerError,
                new { message = message }
            );
        }
    }
}