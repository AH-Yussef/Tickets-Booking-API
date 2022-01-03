using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;
using TicketsBooking.Application.Common.Responses;
using TicketsBooking.Crosscut.Constants;
using TicketsBooking.Domain.Entities;
using Xunit;
using Assert = Xunit.Assert;
using TicketsBooking.Application.Components.Customers.DTOs.Results;
using TicketsBooking.Application.Components.Customers;

namespace TicketsBooking.UnitTest.ServideLayerTesting.CustomerTests
{
    public class CustomerDeleteTests
    {
        [Fact]
        public async void Delete_RecordExists()
        {
            using var mock = AutoMock.GetLoose();
            //Arange
            var fakeEmail = "LOL@mail";
            var fakeCustomer = GetSampleCustomers()[0];
            var fakeCustomerDTO = new GetSingleUserResult
            {
                Name = fakeCustomer.Name,
            };

            mock.Mock<ICustomerRepo>()
                .Setup(repo => repo.Delete(fakeEmail))
                .Returns(Task.FromResult(true));

            mock.Mock<ICustomerRepo>()
                .Setup(repo => repo.GetSingleByEmail(fakeEmail))
                .Returns(Task.FromResult(fakeCustomer));

            var customerService = mock.Create<CustomerService>();

            var expectedResponse = new OutputResponse<bool>
            {
                Success = true,
                StatusCode = HttpStatusCode.Accepted,
                Message = ResponseMessages.Success,
            };
            //Act
            var actualResponse = await customerService.Delete(fakeEmail);

            //Assert
            mock.Mock<ICustomerRepo>()
                .Verify(repo => repo.Delete(fakeEmail), Times.Once);

            mock.Mock<ICustomerRepo>()
                .Verify(repo => repo.GetSingleByEmail(fakeEmail), Times.Once);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
        }
        [Fact]
        public async void Delete_RecordDoesntExists()
        {
            using var mock = AutoMock.GetLoose();
            //Arange
            var fakeEmail = "LOL@mail";
            var fakeCustomer = GetSampleCustomers()[0];
            var fakeCustomerDTO = new GetSingleUserResult
            {
                Name = fakeCustomer.Name,
            };

            mock.Mock<ICustomerRepo>()
                .Setup(repo => repo.Delete(fakeEmail))
                .Returns(Task.FromResult(false));

            mock.Mock<ICustomerRepo>()
                .Setup(repo => repo.GetSingleByEmail(fakeEmail))
                .Returns(Task.FromResult((Customer)null));

            var customerService = mock.Create<CustomerService>();

            var expectedResponse = new OutputResponse<bool>
            {
                Success = false,
                StatusCode = HttpStatusCode.NotFound,
                Message = ResponseMessages.Failure,
            };
            //Act
            var actualResponse = await customerService.Delete(fakeEmail);

            //Assert
            mock.Mock<ICustomerRepo>()
                .Verify(repo => repo.Delete(fakeEmail), Times.Never);

            mock.Mock<ICustomerRepo>()
                .Verify(repo => repo.GetSingleByEmail(fakeEmail), Times.Once);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
        }
        [Fact]
        public async void Delete_NullInput()
        {
            using var mock = AutoMock.GetLoose();

            //Arange
            string fakeEmail = null;
            var fakeCustomer = GetSampleCustomers()[0];
            var fakeCustomerDTO = new GetSingleUserResult
            {
                Name = fakeCustomer.Name,
            };

            mock.Mock<ICustomerRepo>()
                .Setup(repo => repo.Delete(fakeEmail))
                .Returns(Task.FromResult(false));


            var customerService = mock.Create<CustomerService>();

            var expectedResponse = new OutputResponse<GetSingleUserResult>
            {
                Success = false,
                StatusCode = HttpStatusCode.UnprocessableEntity,
                Message = ResponseMessages.UnprocessableEntity,
            };
            //Act
            var actualResponse = await customerService.GetSingle(fakeEmail);

            //Assert
            mock.Mock<ICustomerRepo>()
                .Verify(repo => repo.Delete(fakeEmail), Times.Never);
            mock.Mock<ICustomerRepo>()
                .Verify(repo => repo.GetSingleByEmail(fakeEmail), Times.Never);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
        }

        private List<Customer> GetSampleCustomers()
        {
            var sample = new List<Customer>();
            sample.Add(new Customer
            {
                Name = "LOL",
            });
            sample.Add(new Customer
            {
                Name = "Mostafa",
            });
            sample.Add(new Customer
            {
                Name = "Tarek",
            });
            sample.Add(new Customer
            {
                Name = "Shosh",
            });

            return sample;
        }
    }
}
