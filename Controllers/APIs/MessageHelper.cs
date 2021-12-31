using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewJobSurveyAdmin.Models;
using NewJobSurveyAdmin.Services;
using System;
using System.Threading.Tasks;

namespace NewJobSurveyAdmin
{

    public class MessageHelper
    {

        public static string EmailSubjectFromTaskAndOutcome(
            TaskEnum task, TaskOutcomeEnum taskOutcome
        )
        {
            return $"Task result: {task.Code}: {taskOutcome.Code}";
        }

        public static string MessageFromException(Exception exception)
        {
            var message =
                $"Error: {exception.Message} \r\n" +
                $"Stacktrace:\r\n {exception.StackTrace}";

            return message;
        }
    }
}