using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using FluentValidation;
using TicketsBooking.Crosscut.Utilities.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace TicketsBooking.Application.Common.Responses
{
    public static class ValidationExtension
    {
        public static void UseFluentValidationExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(x =>
            {
                x.Run(async context =>
                {
                    string errorText;
                    var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var exception = errorFeature.Error;

                    if (exception.GetType() == typeof(ValidationException))
                    {
                        var errorBag = new Dictionary<string, List<string>>();
                        var propertiesErrors = ((ValidationException) exception)
                            .Errors
                            .GroupBy(
                                error => error.PropertyName,
                                error => error.ErrorMessage
                            );

                        foreach (var propertyErrors in propertiesErrors)
                        {
                            errorBag[propertyErrors.Key.ToLowerFirstChar()] = propertyErrors.ToList();
                        }

                        var response = new OutputResponse<string>
                        {
                            Message = "one or more validation failures has been occurred",
                            StatusCode = HttpStatusCode.UnprocessableEntity,
                            Success = false,
                            Model = null,
                            Errors = errorBag
                        };

                        errorText = JsonSerializer.Serialize(response, new JsonSerializerOptions()
                        {
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                        });
                        context.Response.StatusCode = (int) HttpStatusCode.UnprocessableEntity;
                    }
                    else
                    {
                        var response = new OutputResponse<string>
                        {
                            Message = exception.Message,
                            StatusCode = HttpStatusCode.BadRequest,
                            Success = false,
                            Model = null
                        };
                        errorText = JsonSerializer.Serialize(response, new JsonSerializerOptions()
                        {
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                        });
                    }

                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(errorText, Encoding.UTF8);
                });
            });
        }
    }
}
