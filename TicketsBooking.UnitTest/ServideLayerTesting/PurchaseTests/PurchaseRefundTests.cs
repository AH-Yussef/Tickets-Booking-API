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
using TicketsBooking.Application.Components.Purchases.DTOs.RepoDTO;

namespace TicketsBooking.UnitTest.ServideLayerTesting.PurchaseTests
{
    public class PurchaseRefundTests
    {
        [Fact]
        public async Task TestRefundSuccess()
        {
            using var mock = AutoMock.GetLoose();
            string pid = "pid";
            var expectedResponse = new OutputResponse<bool>
            {
                Success = true,
                StatusCode = HttpStatusCode.Accepted,
                Message = ResponseMessages.Success,
                Model = true,
            };
            mock.Mock<IPurchaseRepo>()
                .Setup(repo => repo.GetSingle(pid))
                .Returns(Task.FromResult(new PurchaseRepoDTO()));
            mock.Mock<IPurchaseRepo>()
                .Setup(repo => repo.Refund(pid))
                .Returns(Task.FromResult(true));

            var purchaseService = mock.Create<PurchaseService>();

            var actualResponse = await purchaseService.Refund(pid);

            //Assert
            mock.Mock<IPurchaseRepo>()
                .Verify(repo => repo.GetSingle(pid), Times.Once);
            mock.Mock<IPurchaseRepo>()
                .Verify(repo => repo.Refund(pid), Times.Once);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model, expectedResponse.Model);
        }
        [Fact]
        public async Task TestRefundFailed()
        {
            using var mock = AutoMock.GetLoose();
            string pid = "pid";
            var expectedResponse = new OutputResponse<bool>
            {
                Success = false,
                StatusCode = HttpStatusCode.NotFound,
                Message = ResponseMessages.Failure,
                Model = false,
            };
            mock.Mock<IPurchaseRepo>()
                .Setup(repo => repo.GetSingle(pid))
                .Returns(Task.FromResult((PurchaseRepoDTO)null));
            mock.Mock<IPurchaseRepo>()
                .Setup(repo => repo.Refund(pid))
                .Returns(Task.FromResult(false));

            var purchaseService = mock.Create<PurchaseService>();

            var actualResponse = await purchaseService.Refund(pid);

            //Assert
            mock.Mock<IPurchaseRepo>()
                .Verify(repo => repo.GetSingle(pid), Times.Once);
            mock.Mock<IPurchaseRepo>()
                .Verify(repo => repo.Refund(pid), Times.Never);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model, expectedResponse.Model);
        }
    }
}
