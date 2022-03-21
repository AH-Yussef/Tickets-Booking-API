using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Microsoft.AspNetCore.Mvc;
using TicketsBooking.APIs.Controllers;
using TicketsBooking.Application.Common.Responses;
using TicketsBooking.Application.Components.Authentication;
using TicketsBooking.Application.Components.Authentication.DTOs;
using TicketsBooking.Application.Components.EventProviders;
using TicketsBooking.Application.Components.EventProviders.DTOs.Commands;
using TicketsBooking.Application.Components.Purchases;
using TicketsBooking.Application.Components.Purchases.DTOs.Commands;
using TicketsBooking.Application.Components.Purchases.DTOs.Results;
using TicketsBooking.Application.Components.SocialMedias.DTOs;
using TicketsBooking.Crosscut.Constants;
using Xunit;

namespace TicketsBooking.UnitTest.APILayerTesting.PurchaseAPITests
{
    public class PurchaseRefundTests
    {
        private OutputResponse<PurchaseSingleResult> FakeSuccessOutput => new OutputResponse<PurchaseSingleResult>
        {
            Success = true,
            StatusCode = HttpStatusCode.Created,
            Message = ResponseMessages.Success,
            Model = new PurchaseSingleResult(),
        };
        private OutputResponse<bool> FakeInvalidInputFailOutput => new OutputResponse<bool>
        {
            Success = false,
            StatusCode = HttpStatusCode.UnprocessableEntity,
            Message = ResponseMessages.UnprocessableEntity,
        };
        private OutputResponse<bool> FakeDoesntExistFailOutput => new OutputResponse<bool>
        {
            Success = false,
            StatusCode = HttpStatusCode.BadRequest,
            Message = ResponseMessages.Failure,
        };
    }
}
