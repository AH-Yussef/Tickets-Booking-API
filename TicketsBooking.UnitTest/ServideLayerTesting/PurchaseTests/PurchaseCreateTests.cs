using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;
using TicketsBooking.Application.Common.Responses;
using TicketsBooking.Crosscut.Constants;
using TicketsBooking.Application.Components.Events.DTOs.Queries;
using TicketsBooking.Application.Components.Events.DTOs.Results;
using TicketsBooking.Application.Components.Events.DTOs.Commands;
using TicketsBooking.Domain.Entities;
using Xunit;
using Assert = Xunit.Assert;
using System.Collections;
using TicketsBooking.Application.Components.Events;
using AutoMapper;
using TicketsBooking.Integration.Email.Models;
using TicketsBooking.Integration.Email;
using TicketsBooking.Application.Components.EventProviders;
using TicketsBooking.Application.Components.Purchases.DTOs.Commands;
using TicketsBooking.Application.Components.Purchases.DTOs.Results;
using TicketsBooking.Application.Components.Purchases;

namespace TicketsBooking.UnitTest.ServideLayerTesting.PurchaseTests
{
    public class PurchaseCreateTests
    {
        [Fact]
        public async Task TestPurchaseCreateSuccess()
        {
            using var mock = AutoMock.GetLoose();
            CreateNewPurchaseCommand cnpc = new CreateNewPurchaseCommand
            {
                CustomerID = "ci",
                EventID = "ei",
                TicketsCount = 1,
            };
            PurchaseSingleResult psr = new PurchaseSingleResult
            {
                PurchaseID = "ciei0",
                CustomerID = "ci",
                EventID = "ei",
            };
            Purchase p = new Purchase
            {
                PurchaseID = "ciei0",
            };
            var expectedResponse = new OutputResponse<PurchaseSingleResult>
            {
                Success = true,
                StatusCode = HttpStatusCode.Created,
                Message = ResponseMessages.Success,
                Model = psr,
            };
            mock.Mock<IPurchaseRepo>()
                .Setup(repo => repo.CreateNewPurchase(cnpc))
                .Returns(Task.FromResult(p));

            var purchaseService = mock.Create<PurchaseService>();

            var actualResponse = await purchaseService.CreateNewPurchase(cnpc);

            //Assert
            mock.Mock<IPurchaseRepo>()
                .Verify(repo => repo.CreateNewPurchase(cnpc), Times.Once);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model.PurchaseID, expectedResponse.Model.PurchaseID);
        }
        [Fact]
        public async Task TestPurchaseCreateFail()
        {
            using var mock = AutoMock.GetLoose();
            CreateNewPurchaseCommand cnpc = new CreateNewPurchaseCommand
            {
                CustomerID = "ci",
                //EventID = "ei",
                TicketsCount = 1,
            };
            PurchaseSingleResult psr = new PurchaseSingleResult
            {
                PurchaseID = "ciei0",
                CustomerID = "ci",
                EventID = "ei",
                
            };
            Purchase p = new Purchase
            {
                PurchaseID = "ciei0",
            };
            var expectedResponse = new OutputResponse<PurchaseSingleResult>
            {
                Success = false,
                StatusCode = HttpStatusCode.UnprocessableEntity,
                Message = ResponseMessages.UnprocessableEntity,
            };
            mock.Mock<IPurchaseRepo>()
                .Setup(repo => repo.CreateNewPurchase(cnpc))
                .Returns(Task.FromResult((Purchase)null));

            var purchaseService = mock.Create<PurchaseService>();

            var actualResponse = await purchaseService.CreateNewPurchase(cnpc);

            //Assert
            mock.Mock<IPurchaseRepo>()
                .Verify(repo => repo.CreateNewPurchase(cnpc), Times.Never);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model, expectedResponse.Model);
        }
    }
}
