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
using TicketsBooking.Application.Components.Customers.DTOs.Query;
using TicketsBooking.Application.Components.Customers.DTOs.Results;
using TicketsBooking.Crosscut.Constants;
using TicketsBooking.Domain.Entities;
using Xunit;

namespace TicketsBooking.UnitTest.ServideLayerTesting.CustomerTests
{
    public class CustomerGetAllTests
    {
        // GetAll tests
        [Fact]
        public async void GetAll_NoSearchTarget()
        {
            using var mock = AutoMock.GetLoose();
            //Arange

            var fakeCustomer = GetSampleCustomer()[0];
            var fakeCustomerDTO = new GetAllUserListedResult
            {
                Name = fakeCustomer.Name,
            };
            var fakeDTO = new GetAllUsersQuery()
            {
                pageNumber = 1,
                pageSize = 10,
            };
            List<GetAllUserListedResult> listFake = new List<GetAllUserListedResult>();
            listFake.Add(fakeCustomerDTO);

            List<Customer> list = new List<Customer>();
            list.Add(fakeCustomer);

            mock.Mock<ICustomerRepo>()
                .Setup(repo => repo.GetAll(fakeDTO))
                .Returns(Task.FromResult(list));

            mock.Mock<IMapper>()
                .Setup(mapper => mapper.Map<List<GetAllUserListedResult>>(list))
                .Returns(listFake);

            var customerService = mock.Create<CustomerService>();

            var expectedResponse = new OutputResponse<List<GetAllUserListedResult>>
            {
                Success = false,
                StatusCode = HttpStatusCode.UnprocessableEntity,
                Message = ResponseMessages.UnprocessableEntity,
                //_mapper.Map<List<EventProviderListedResult>>(eventProviders)
            };
            //Act
            var actualResponse = await customerService.GetAll(fakeDTO);

            //Assert
            mock.Mock<ICustomerRepo>()
                .Verify(repo => repo.GetAll(fakeDTO), Times.Never);

            mock.Mock<IMapper>()
                .Verify(mapper => mapper.Map<List<GetAllUserListedResult>>(list), Times.Never);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
        }
        [Fact]
        public async void GetAll_InvalidQuery()
        {
            using var mock = AutoMock.GetLoose();
            //Arange
            var fakeCustomer = GetSampleCustomer()[0];
            var fakeCustomerDTO = new GetAllUserListedResult
            {
                Name = fakeCustomer.Name,
            };
            var fakeDTO = new GetAllUsersQuery()
            {
                pageSize = 10,
                searchTarget = "LOL",
            };
            List<GetAllUserListedResult> listFake = new List<GetAllUserListedResult>();


            List<Customer> list = new List<Customer>();


            mock.Mock<ICustomerRepo>()
                .Setup(repo => repo.GetAll(fakeDTO))
                .Returns(Task.FromResult(list));
            //.Returns(Task.FromResult((EventProvider) null));

            mock.Mock<IMapper>()
                .Setup(mapper => mapper.Map<List<GetAllUserListedResult>>(list))
                .Returns(listFake);

            var customerService = mock.Create<CustomerService>();

            var expectedResponse = new OutputResponse<List<GetAllUserListedResult>>
            {
                Success = false,
                StatusCode = HttpStatusCode.UnprocessableEntity,
                Message = ResponseMessages.UnprocessableEntity,
            };
            //Act
            var actualResponse = await customerService.GetAll(fakeDTO);

            //Assert
            mock.Mock<ICustomerRepo>()
                .Verify(repo => repo.GetAll(fakeDTO), Times.Never);

            mock.Mock<IMapper>()
                .Verify(mapper => mapper.Map<List<GetAllUserListedResult>>(list), Times.Never);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model, expectedResponse.Model);
        }
        [Fact]
        public async void GetAll_RecordsExist()
        {
            using var mock = AutoMock.GetLoose();
            //Arange

            // var fakeName = "LOL";
            var fakeCustomer = GetSampleCustomer()[0];
            var fakeCustomerDTO = new GetAllUserListedResult
            {
                Name = fakeCustomer.Name,
            };
            var fakeDTO = new GetAllUsersQuery()
            {
                pageNumber = 1,
                pageSize = 10,
                searchTarget = "LOL",
            };
            List<GetAllUserListedResult> listFake = new List<GetAllUserListedResult>();
            listFake.Add(fakeCustomerDTO);

            List<Customer> list = new List<Customer>();
            list.Add(fakeCustomer);

            mock.Mock<ICustomerRepo>()
                .Setup(repo => repo.GetAll(fakeDTO))
                .Returns(Task.FromResult(list));

            mock.Mock<IMapper>()
                .Setup(mapper => mapper.Map<List<GetAllUserListedResult>>(list))
                .Returns(listFake);

            var customerService = mock.Create<CustomerService>();

            var expectedResponse = new OutputResponse<List<GetAllUserListedResult>>
            {
                Success = true,
                StatusCode = HttpStatusCode.Accepted,
                Message = ResponseMessages.Success,
                Model = listFake,
                //_mapper.Map<List<EventProviderListedResult>>(eventProviders)
            };
            //Act
            var actualResponse = await customerService.GetAll(fakeDTO);

            //Assert
            mock.Mock<ICustomerRepo>()
                .Verify(repo => repo.GetAll(fakeDTO), Times.Once);

            mock.Mock<IMapper>()
                .Verify(mapper => mapper.Map<List<GetAllUserListedResult>>(list), Times.Once);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model[0].Name, expectedResponse.Model[0].Name);
        }
        // GetSingle tests


        private List<Customer> GetSampleCustomer()
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
