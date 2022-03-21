using Autofac.Extras.Moq;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TicketsBooking.Application.Common.Responses;
using TicketsBooking.Application.Components.Customers;
using TicketsBooking.Application.Components.Customers.DTOs.Command;
using TicketsBooking.Crosscut.Constants;
using TicketsBooking.Domain.Entities;
using TicketsBooking.Integration.Email;
using TicketsBooking.Integration.Email.Models;
using Xunit;

namespace TicketsBooking.UnitTest.ServideLayerTesting.CustomerTests
{
    public class CustomerRegisterTests
    {
        [Theory]
        [ClassData(typeof(RegisterInvalidTest))]
        public async void Register_Invalid(RegisterCustomerCommand fakeCustomerDTO)
        {
            using var mock = AutoMock.GetLoose();
            //Arrange
            var customerDTO = fakeCustomerDTO;

            var fakeEmail = customerDTO.Email;
            var fakeCustomer = new Customer
            {
                Name = customerDTO.Name,
                Password = customerDTO.Password,
                Email = customerDTO.Email,
            };

            var expectedResponse = new OutputResponse<bool>
            {
                Success = false,
                StatusCode = HttpStatusCode.UnprocessableEntity,
                Message = ResponseMessages.UnprocessableEntity,
                Model = false
            };

            var fakeMailDTO = new MailModel
            {
                ToEmail = "test@email.com",
                Subject = "test",
                Body = "test",
            };

            //Act
            mock.Mock<ICustomerRepo>()
                .Setup(repo => repo.Register(customerDTO))
                .Returns(Task.FromResult(fakeCustomer));

            mock.Mock<ICustomerRepo>()
                .Setup(repo => repo.GetSingleByEmail(fakeEmail))
                .Returns(Task.FromResult(fakeCustomer));

            mock.Mock<IMailService>()
                .Setup(mailService => mailService.SendEmailAsync(fakeMailDTO));

            var customerService = mock.Create<CustomerService>();

            var actualResponse = await customerService.Register(customerDTO);

            //Assert
            mock.Mock<ICustomerRepo>()
                .Verify(repo => repo.GetSingleByEmail(fakeEmail), Times.Never);
            mock.Mock<ICustomerRepo>()
                .Verify(repo => repo.Register(customerDTO), Times.Never);
            mock.Mock<IMailService>()
                .Setup(mailService => mailService.SendEmailAsync(fakeMailDTO));

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model, expectedResponse.Model);
        }

        [Fact]
        public async void Create_ValidAlreadyExist()
        {
            using var mock = AutoMock.GetLoose();
            //Arange
            var fakeCustomer = new Customer
            {
                Name = "Lol",
                Password = "dassa",
                Email = "m@test",
            };
            var fakeEmail = fakeCustomer.Email;
            var fakeCustomerDTO = new RegisterCustomerCommand
            {
                Name = fakeCustomer.Name,
                Password = fakeCustomer.Password,
                Email = fakeCustomer.Email,
            };

            var fakeMailDTO = new MailModel
            {
                ToEmail = "test@email.com",
                Subject = "test",
                Body = "test",
            };

            mock.Mock<ICustomerRepo>()
                .Setup(repo => repo.Register(fakeCustomerDTO))
                .Returns(Task.FromResult(fakeCustomer));

            mock.Mock<ICustomerRepo>()
                .Setup(repo => repo.GetSingleByEmail(fakeEmail))
                .Returns(Task.FromResult(fakeCustomer));

            mock.Mock<IMailService>()
                .Verify(mailService => mailService.SendEmailAsync(fakeMailDTO), Times.Never);

            var customerService = mock.Create<CustomerService>();

            var expectedResponse = new OutputResponse<bool>
            {
                Success = false,
                StatusCode = HttpStatusCode.BadRequest,
                Message = ResponseMessages.Failure,
                Model = false
            };
            //Act
            var actualResponse = await customerService.Register(fakeCustomerDTO);

            //Assert
            mock.Mock<ICustomerRepo>()
                .Verify(repo => repo.GetSingleByEmail(fakeEmail), Times.Once);
            mock.Mock<ICustomerRepo>()
                .Verify(repo => repo.Register(fakeCustomerDTO), Times.Never);
            mock.Mock<IMailService>()
                .Verify(mailService => mailService.SendEmailAsync(fakeMailDTO), Times.Never);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model, expectedResponse.Model);
        }
        [Fact]
        public async void Create_ValidNew()
        {
            using var mock = AutoMock.GetLoose();
            //Arange
            var fakeCustomer = new Customer
            {
                Name = "Lol",
                Password = "dassa",
                Email = "m@test"
            };
            var fakeEmail = fakeCustomer.Email;
            var fakeCustomerDTO = new RegisterCustomerCommand
            {
                Name = fakeCustomer.Name,
                Password = fakeCustomer.Password,
                Email = fakeCustomer.Email,
            };

            mock.Mock<ICustomerRepo>()
                .Setup(repo => repo.Register(fakeCustomerDTO))
                .Returns(Task.FromResult(fakeCustomer));

            mock.Mock<ICustomerRepo>()
                .Setup(repo => repo.GetSingleByEmail(fakeEmail))
                .Returns(Task.FromResult((Customer)null));

            var customerService = mock.Create<CustomerService>();

            var expectedResponse = new OutputResponse<bool>
            {
                Success = true,
                StatusCode = HttpStatusCode.Created,
                Message = ResponseMessages.Success,
                Model = true,
            };
            //Act
            var actualResponse = await customerService.Register(fakeCustomerDTO);

            //Assert
            mock.Mock<ICustomerRepo>()
                .Verify(repo => repo.GetSingleByEmail(fakeEmail), Times.Once);
            mock.Mock<ICustomerRepo>()
                .Verify(repo => repo.Register(fakeCustomerDTO), Times.Once);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model, expectedResponse.Model);
        }

        public class RegisterInvalidTest : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                var NullPassword = new RegisterCustomerCommand
                {
                    Name = "Lol",
                    Password = null,
                    Email = "m@test",
                    
                };
                var NullName = new RegisterCustomerCommand
                {
                    Name = null,
                    Password = "dassa",
                    Email = "m@test",
                };
                var NullEmail = new RegisterCustomerCommand
                {
                    Name = "Lol",
                    Password = "dassa",
                    Email = null,
                };
                var NullCombination1 = new RegisterCustomerCommand
                {
                    Name = "Lol",
                    Password = null,
                    Email = null,
                };
                var NullCombination2 = new RegisterCustomerCommand
                {
                    Name = null,
                    Password = "dassa",
                    Email = null,
                };

                yield return new object[] { NullName };
                yield return new object[] { NullEmail };
                yield return new object[] { NullPassword };
                yield return new object[] { NullCombination1 };
                yield return new object[] { NullCombination2 };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
