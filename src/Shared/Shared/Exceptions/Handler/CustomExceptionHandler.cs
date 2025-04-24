﻿using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace Shared.Exceptions.Handler
{
    public class CustomExceptionHandler
        (ILogger<CustomExceptionHandler> logger)
        : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError("Error Message: {exception message}, Time of occurence: {time}", exception.Message, DateTime.UtcNow);
            (string Detail, string Title, int StatusCode) details = exception switch
            {
                InternalServerException => (
                    exception.Message,
                    exception.GetType().Name,
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError
                ),
                ValidationException => (
                    exception.Message,
                    exception.GetType().Name,
                    context.Response.StatusCode = StatusCodes.Status400BadRequest
                ),
                BadRequestException => (
                    exception.Message,
                    exception.GetType().Name,
                    context.Response.StatusCode = StatusCodes.Status400BadRequest
                ),
                NotFoundException => (
                    exception.Message,
                    exception.GetType().Name,
                    context.Response.StatusCode = StatusCodes.Status404NotFound
                ),
                _ => (
                    exception.Message,
                    exception.GetType().Name,
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError
                )
            };

            var problemDetails = new ProblemDetails
            {
                Title = details.Title,
                Detail = details.Detail,
                Status = details.StatusCode,
                Instance = context.Request.Path
            };

            problemDetails.Extensions.Add("traceId", context.TraceIdentifier);

            await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);
            return true;
        }
    }
}
