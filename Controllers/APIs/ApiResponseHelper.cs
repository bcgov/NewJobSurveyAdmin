using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewJobSurveyAdmin.Models;
using NewJobSurveyAdmin.Services;
using System;
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
            var message = MessageHelper.MessageFromException(exception);

            await logger.LogFailure(task, message);

            return controllerBase.StatusCode(
                StatusCodes.Status500InternalServerError,
                new { message = message }
            );
        }
    }
}