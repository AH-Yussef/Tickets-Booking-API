using Autofac.Extras.Moq;
using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TicketsBooking.Application.Common.Responses;
using TicketsBooking.Application.Components.Customers;
using TicketsBooking.Application.Components.Customers.DTOs.Results;
using TicketsBooking.Crosscut.Constants;
using TicketsBooking.Domain.Entities;
using Xunit;

namespace TicketsBooking.UnitTest.ServideLayerTesting.CustomerTests
{
    public class CustomerGetSingleTests
    {
        [Fact]
        public async void GetSingle_RecordExists()
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
                .Setup(repo => repo.GetSingleByEmail(fakeEmail))
                .Returns(Task.FromResult(fakeCustomer));

            mock.Mock<IMapper>()
                .Setup(mapper => mapper.Map<GetSingleUserResult>(fakeCustomer))
                .Returns(fakeCustomerDTO);

            var customerService = mock.Create<CustomerService>();

            var expectedResponse = new OutputResponse<GetSingleUserResult>
            {
                Success = true,
                StatusCode = HttpStatusCode.Accepted,
                Message = ResponseMessages.Success,
                Model = fakeCustomerDTO,
            };
            //Act
            var actualResponse = await customerService.GetSingle(fakeEmail);

            //Assert
            mock.Mock<ICustomerRepo>()
                .Verify(repo => repo.GetSingleByEmail(fakeEmail), Times.Once);

            mock.Mock<IMapper>()
                .Verify(mapper => mapper.Map<GetSingleUserResult>(fakeCustomer), Times.Once);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model.Name, expectedResponse.Model.Name);
        }

        [Fact]
        public async void GetSingle_RecordDoesNotExist()
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
                .Setup(repo => repo.GetSingleByEmail(fakeEmail))
                .Returns(Task.FromResult((Customer)null));

            mock.Mock<IMapper>()
                .Setup(mapper => mapper.Map<GetSingleUserResult>(fakeCustomer))
                .Returns(fakeCustomerDTO);

            var customerService = mock.Create<CustomerService>(); //return null

            var expectedResponse = new OutputResponse<GetSingleUserResult>
            {
                Success = false,
                StatusCode = HttpStatusCode.NotFound,
                Message = ResponseMessages.Failure,
                Model = null,
            };
            //Act
            var actualResponse = await customerService.GetSingle(fakeEmail);

            //Assert
            mock.Mock<ICustomerRepo>()
                .Verify(repo => repo.GetSingleByEmail(fakeEmail), Times.Once);

            mock.Mock<IMapper>()
                .Verify(mapper => mapper.Map<GetSingleUserResult>(fakeCustomer), Times.Never);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model, expectedResponse.Model);
        }
        [Fact]
        public async void GetSingle_NullInput()
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
                .Setup(repo => repo.GetSingleByEmail(fakeEmail))
                .Returns(Task.FromResult(fakeCustomer));

            mock.Mock<IMapper>()
                .Setup(mapper => mapper.Map<GetSingleUserResult>(fakeCustomer))
                .Returns(fakeCustomerDTO);

            var customerService = mock.Create<CustomerService>();

            var expectedResponse = new OutputResponse<GetSingleUserResult>
            {
                Success = false,
                StatusCode = HttpStatusCode.UnprocessableEntity,
                Message = ResponseMessages.UnprocessableEntity,
                Model = null,
            };
            //Act
            var actualResponse = await customerService.GetSingle(fakeEmail);

            //Assert
            mock.Mock<ICustomerRepo>()
                .Verify(repo => repo.GetSingleByEmail(fakeEmail), Times.Never);

            mock.Mock<IMapper>()
                .Verify(mapper => mapper.Map<GetSingleUserResult>(fakeCustomer), Times.Never);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model, expectedResponse.Model);
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
