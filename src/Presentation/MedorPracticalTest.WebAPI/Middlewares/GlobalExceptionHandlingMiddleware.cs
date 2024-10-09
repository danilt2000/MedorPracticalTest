using MedorPracticalTest.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace MedorPracticalTest.WebAPI.Middlewares
{
        /// <summary>
        /// Middleware for handling exceptions globally in application.
        /// Support custom exception types
        /// </summary>
        public class GlobalExceptionHandlingMiddleware
        {
                private readonly RequestDelegate _next;

                /// <summary>
                /// Constructor for <see cref="GlobalExceptionHandlingMiddleware"/>
                /// </summary>
                /// <param name="next"></param>
                public GlobalExceptionHandlingMiddleware(RequestDelegate next)
                {
                        _next = next;
                }

                /// <summary>
                /// Method that is triggered when request goes through
                /// </summary>
                /// <param name="context">HttpContext</param>
                /// <returns>Executes functions or handles exception</returns>
                public async Task InvokeAsync(HttpContext context)
                {
                        try
                        {
                                await _next(context);
                        }
                        catch (Exception e)
                        {
                                var problemDetails = new ProblemDetails()
                                {
                                        Status = (int)HttpStatusCode.InternalServerError
                                };

                                var exceptionMessage = "";
                                
                                switch (e)
                                {
                                        case BitcoinNotFoundException notFoundEx:
                                                problemDetails.Status = (int)HttpStatusCode.NotFound;
                                                exceptionMessage = notFoundEx.Message;
                                                break;

                                        default:
                                                problemDetails.Type = "Server Error";
                                                problemDetails.Title = "Server Error";
                                                exceptionMessage = "An internal server error has occurred";
                                                break;
                                }

                                problemDetails.Detail = exceptionMessage;
                                
                                var json = JsonSerializer.Serialize(problemDetails);
                                context.Response.ContentType = "application/json";
                                context.Response.StatusCode = (int)problemDetails.Status;
                                await context.Response.WriteAsync(json);
                        }
                }
        }
}
