using Autofac.Extras.Moq;
using AutoMapper;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TicketsBooking.Application.Common.Responses;
using TicketsBooking.Application.Components.Authentication;
using TicketsBooking.Application.Components.Authentication.DTOs;
using TicketsBooking.Application.Components.Authentication.Vlidators;
using TicketsBooking.Application.Components.Customers;
using TicketsBooking.Crosscut.Constants;
using TicketsBooking.Domain.Entities;
using Xunit;
using BC = BCrypt.Net.BCrypt;

namespace TicketsBooking.UnitTest.ServideLayerTesting.CustomerTests
{
    public class CustomerAuthTests
    {
        [Fact]
        public async void Authenticate_success()
        {
            using var mock = AutoMock.GetLoose();
            //Arrange
            var authCreds = new AuthCreds
            {
                Email = "test@test.com",
                Password = "123456789aH",
            };
            var customer = FakeCustomer;
            customer.Accepted = true;

            var authUserResult = FakeAuthedUserResult(customer);

            var expectedOutput = new OutputResponse<AuthedUserResult>
            {
                Success = true,
                StatusCode = HttpStatusCode.Accepted,
                Message = ResponseMessages.Success,
                Model = authUserResult,
            };

            //Act
            mock.Mock<IMapper>()
                .Setup(e => e.Map<AuthedUserResult>(customer))
                .Returns(authUserResult);

            mock.Mock<ICustomerRepo>()
                .Setup(e => e.GetSingleByEmail(authCreds.Email))
                .Returns(Task.FromResult(customer));

            mock.Mock<ITokenManager>()
                .Setup(t => t.GenerateToken(customer, Roles.Customer))
                .Returns(FakeToken);

            var customerService = mock.Create<CustomerService>();
            var actualOuptut = await customerService.Authenticate(authCreds);

            //Assert
            mock.Mock<IMapper>()
                .Verify(e => e.Map<AuthedUserResult>(customer), Times.Once);

            mock.Mock<ICustomerRepo>()
                .Verify(x => x.GetSingleByEmail(authCreds.Email), Times.Once);

            mock.Mock<ITokenManager>()
                .Verify(t => t.GenerateToken(customer, Roles.Customer), Times.Once);

            Assert.NotNull(actualOuptut);
            Assert.Equal(actualOuptut.Success, expectedOutput.Success);
            Assert.Equal(actualOuptut.StatusCode, expectedOutput.StatusCode);
            Assert.Equal(actualOuptut.Message, expectedOutput.Message);
            Assert.Equal(actualOuptut.Model.Name, expectedOutput.Model.Name);
            Assert.Equal(actualOuptut.Model.Email, expectedOutput.Model.Email);
            Assert.Equal(actualOuptut.Model.Token, expectedOutput.Model.Token);
        }

        [Theory]
        [ClassData(typeof(AuthFailureTestData))]
        public async void Authenticate_failure(AuthCreds authCreds)
        {
            using var mock = AutoMock.GetLoose();
            //Arrange
            var customer = FakeCustomer;
            customer.Accepted = false;

            var expectedOutput = new OutputResponse<AuthedUserResult>
            {
                Success = false,
                StatusCode = HttpStatusCode.Unauthorized,
                Message = ResponseMessages.Unauthorized,
                Model = null,
            };

            //Act
            mock.Mock<IMapper>()
                .Setup(e => e.Map<AuthedUserResult>(customer));

            mock.Mock<ICustomerRepo>()
                .Setup(e => e.GetSingleByEmail(authCreds.Email))
                .Returns(Task.FromResult(customer));

            mock.Mock<ITokenManager>()
                .Setup(t => t.GenerateToken(customer, Roles.Customer));

            var customerService = mock.Create<CustomerService>();
            var actualOuptut = await customerService.Authenticate(authCreds);

            //Assert
            mock.Mock<IMapper>()
                .Verify(e => e.Map<AuthedUserResult>(customer), Times.Never);

            mock.Mock<ICustomerRepo>()
                .Verify(x => x.GetSingleByEmail(authCreds.Email), Times.Once);

            mock.Mock<ITokenManager>()
                .Verify(t => t.GenerateToken(customer, Roles.Customer), Times.Never);

            Assert.NotNull(actualOuptut);
            Assert.Equal(actualOuptut.Success, expectedOutput.Success);
            Assert.Equal(actualOuptut.StatusCode, expectedOutput.StatusCode);
            Assert.Equal(actualOuptut.Message, expectedOutput.Message);
            Assert.Null(actualOuptut.Model);
        }

        [Theory]
        [ClassData(typeof(AuthValidationTestData))]
        public async void Authenticate_inValidCreds(AuthCreds authCreds)
        {
            using var mock = AutoMock.GetLoose();
            //Arrange
            var customer = FakeCustomer;

            var expectedOutput = new OutputResponse<AuthedUserResult>
            {
                Success = false,
                StatusCode = HttpStatusCode.UnprocessableEntity,
                Message = ResponseMessages.UnprocessableEntity,
                Model = null,
            };

            //Act
            mock.Mock<IMapper>()
                .Setup(e => e.Map<AuthedUserResult>(customer));

            mock.Mock<ICustomerRepo>()
                .Setup(e => e.GetSingleByEmail(authCreds.Email));

            mock.Mock<ITokenManager>()
                .Setup(t => t.GenerateToken(customer, Roles.Customer));

            var customerService = mock.Create<CustomerService>();
            var actualOuptut = await customerService.Authenticate(authCreds);

            //Assert
            mock.Mock<IMapper>()
                .Verify(e => e.Map<AuthedUserResult>(customer), Times.Never);

            mock.Mock<ICustomerRepo>()
                .Verify(x => x.GetSingleByEmail(authCreds.Email), Times.Never);

            mock.Mock<ITokenManager>()
                .Verify(t => t.GenerateToken(customer, Roles.EventProvider), Times.Never);

            Assert.NotNull(actualOuptut);
            Assert.Equal(actualOuptut.Success, expectedOutput.Success);
            Assert.Equal(actualOuptut.StatusCode, expectedOutput.StatusCode);
            Assert.Equal(actualOuptut.Message, expectedOutput.Message);
            Assert.Null(actualOuptut.Model);
        }

        [Theory]
        [ClassData(typeof(AuthValidationTestData))]
        public void Authenticate_validation_failure(AuthCreds authCreds)
        {
            var authCredsValidator = new AuthCredsValidator();
            var result = authCredsValidator.Validate(authCreds).IsValid;
            Assert.False(result, authCreds.Email);
        }

        [Fact]
        public void Authenticate_validation_success()
        {
            var authCreds = new AuthCreds
            {
                Email = "test@test.com",
                Password = "123456789aH",
            };

            var authCredsValidator = new AuthCredsValidator();
            var result = authCredsValidator.Validate(authCreds).IsValid;
            Assert.True(result);
        }

        private string FakeToken => "123456789";

        private AuthedUserResult FakeAuthedUserResult(Customer customer)
        {
            return new AuthedUserResult
            {
                Name = customer.Name,
                Email = customer.Email,
                Token = FakeToken
            };
        }

        private Customer FakeCustomer => new Customer
        {
            Name = "Test org",
            Email = "test@test.com",
            Password = BC.HashPassword("123456789aH"),
        };

        public class AuthFailureTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                var nonExistingEmailTestData = new AuthCreds
                {
                    Email = "NonExisting@test.com",
                    Password = "12345678aH",
                };
                var wrongPasswrordTestData = new AuthCreds
                {
                    Email = "test@test.com",
                    Password = "2222222222aH",
                };

                yield return new object[] { nonExistingEmailTestData };
                yield return new object[] { wrongPasswrordTestData };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public class AuthValidationTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                var nullEmail = new AuthCreds
                {
                    Email = null,
                    Password = "12345678aH",
                };
                var nullPassword = new AuthCreds
                {
                    Email = "test@test.com",
                    Password = null,
                };
                var nullEmailAndPassword = new AuthCreds
                {
                    Email = null,
                    Password = null,
                };
                var emptyEmail = new AuthCreds
                {
                    Email = "",
                    Password = "12345678aH",
                };
                var emptyPassword = new AuthCreds
                {
                    Email = "test@test.com",
                    Password = "",
                };
                var emptyEmailAndPassword = new AuthCreds
                {
                    Email = "test@test.com",
                    Password = null,
                };
                var invalidEmailFormat_1 = new AuthCreds
                {
                    Email = "test",
                    Password = "2222222222aH",
                };
                var invalidEmailFormat_2 = new AuthCreds
                {
                    Email = "test.com",
                    Password = "2222222222aH",
                };
                var invalidEmailFormat_3 = new AuthCreds
                {
                    Email = "@test.com",
                    Password = "2222222222aH",
                };

                yield return new object[] { nullEmail };
                yield return new object[] { nullPassword };
                yield return new object[] { nullEmailAndPassword };
                yield return new object[] { emptyEmail };
                yield return new object[] { emptyPassword };
                yield return new object[] { emptyEmailAndPassword };
                yield return new object[] { invalidEmailFormat_1 };
                yield return new object[] { invalidEmailFormat_2 };
                yield return new object[] { invalidEmailFormat_3 };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
