using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TicketsBooking.Application.Common.Responses;
using TicketsBooking.Application.Components.EventProviders;
using TicketsBooking.Application.Components.EventProviders.DTOs.Queries;
using TicketsBooking.Application.Components.EventProviders.DTOs.Results;
using TicketsBooking.Crosscut.Constants;
using TicketsBooking.Application.Components.EventProviders.DTOs.Commands;
using TicketsBooking.Domain.Entities;
using Xunit;
using Assert = Xunit.Assert;

namespace TicketsBooking.UnitTest
{
    public class EventProviderServiceTests
    {
        // create provider tests

        // update verified tests
        [Fact]
        public async void UpdateVerified_InvalidInput()
        {
            using var mock = AutoMock.GetLoose();
            //Arange
            var fakeDTO = new SetVerifiedCommand
            {
                Name = "LOL",
                //Verified = true,
            };

            mock.Mock<IEventProviderRepo>()
                .Setup(repo => repo.UpdateVerified(fakeDTO))
                .Returns(Task.FromResult(false));

            var eventProviderService = mock.Create<EventProviderService>();

            var expectedResponse = new OutputResponse<bool>
            {
                Success = false,
                StatusCode = HttpStatusCode.UnprocessableEntity,
                Message = ResponseMessages.UnprocessableEntity,
            };
            //Act
            var actualResponse = await eventProviderService.UpdateVerified(fakeDTO);

            //Assert
            mock.Mock<IEventProviderRepo>()
                .Verify(repo => repo.UpdateVerified(fakeDTO), Times.Never);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model, expectedResponse.Model);
        }
        [Fact]
        public async void UpdateVerified_ValidDoesntExist()
        {
            using var mock = AutoMock.GetLoose();
            //Arange
            var fakeDTO = new SetVerifiedCommand
            {
                Name = "LOL",
                Verified = true,

            };

            mock.Mock<IEventProviderRepo>()
                .Setup(repo => repo.UpdateVerified(fakeDTO))
                .Returns(Task.FromResult(false));

            var eventProviderService = mock.Create<EventProviderService>();

            var expectedResponse = new OutputResponse<bool>
            {
                Success = true,
                StatusCode = HttpStatusCode.Accepted,
                Message = ResponseMessages.Success,
                Model = false,
            };
            //Act
            var actualResponse = await eventProviderService.UpdateVerified(fakeDTO);

            //Assert
            mock.Mock<IEventProviderRepo>()
                .Verify(repo => repo.UpdateVerified(fakeDTO), Times.Once);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model, expectedResponse.Model);
        }
        [Fact]
        public async void UpdateVerified_ValidExist()
        {
            using var mock = AutoMock.GetLoose();
            //Arange
            var fakeDTO = new SetVerifiedCommand
            {
                Name = "LOL",
                Verified = true,

            };
 
            mock.Mock<IEventProviderRepo>()
                .Setup(repo => repo.UpdateVerified(fakeDTO))
                .Returns(Task.FromResult(true));

            var eventProviderService = mock.Create<EventProviderService>();

            var expectedResponse = new OutputResponse<bool>
            {
                Success = true,
                StatusCode = HttpStatusCode.Accepted,
                Message = ResponseMessages.Success,
                Model = true,
            };
            //Act
            var actualResponse = await eventProviderService.UpdateVerified(fakeDTO);

            //Assert
            mock.Mock<IEventProviderRepo>()
                .Verify(repo => repo.UpdateVerified(fakeDTO), Times.Once);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model, expectedResponse.Model);
        }
        // GetAll tests
        [Fact]
        public async void GetAll_RecordsExistNoSearchTarget()
        {
            using var mock = AutoMock.GetLoose();
            //Arange

            // var fakeName = "LOL";
            var fakeEventProvider = GetSampleEventProviders()[0];
            var fakeEventProviderDTO = new EventProviderListedResult
            {
                Name = fakeEventProvider.Name,
            };
            var fakeDTO = new GetAllEventProvidersQuery()
            {
                pageNumber = 1,
                pageSize = 10,
                isVerified = true,

            };
            List<EventProviderListedResult> listFake = new List<EventProviderListedResult>();
            listFake.Add(fakeEventProviderDTO);

            List<EventProvider> list = new List<EventProvider>();
            list.Add(fakeEventProvider);

            mock.Mock<IEventProviderRepo>()
                .Setup(repo => repo.GetAll(fakeDTO))
                .Returns(Task.FromResult(list));

            mock.Mock<IMapper>()
                .Setup(mapper => mapper.Map<List<EventProviderListedResult>>(list))
                .Returns(listFake);

            var eventProviderService = mock.Create<EventProviderService>();

            var expectedResponse = new OutputResponse<List<EventProviderListedResult>>
            {
                Success = true,
                StatusCode = HttpStatusCode.Accepted,
                Message = ResponseMessages.Success,
                Model = listFake,
                //_mapper.Map<List<EventProviderListedResult>>(eventProviders)
            };
            //Act
            var actualResponse = await eventProviderService.GetAll(fakeDTO);

            //Assert
            mock.Mock<IEventProviderRepo>()
                .Verify(repo => repo.GetAll(fakeDTO), Times.Once);

            mock.Mock<IMapper>()
                .Verify(mapper => mapper.Map<List<EventProviderListedResult>>(list), Times.Once);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model[0].Name, expectedResponse.Model[0].Name);
        }
        [Fact]
        public async void GetAll_InvalidQuery()
        {
            using var mock = AutoMock.GetLoose();
            //Arange

            // var fakeName = "LOL";
            var fakeEventProvider = GetSampleEventProviders()[0];
            var fakeEventProviderDTO = new EventProviderListedResult
            {
                Name = fakeEventProvider.Name,
            };
            var fakeDTO = new GetAllEventProvidersQuery()
            {
                pageSize = 10,
                searchTarget = "LOL",
                isVerified = false,

            };
            List<EventProviderListedResult> listFake = new List<EventProviderListedResult>();


            List<EventProvider> list = new List<EventProvider>();


            mock.Mock<IEventProviderRepo>()
                .Setup(repo => repo.GetAll(fakeDTO))
                .Returns(Task.FromResult(list));
            //.Returns(Task.FromResult((EventProvider) null));

            mock.Mock<IMapper>()
                .Setup(mapper => mapper.Map<List<EventProviderListedResult>>(list))
                .Returns(listFake);

            var eventProviderService = mock.Create<EventProviderService>();

            var expectedResponse = new OutputResponse<List<EventProviderListedResult>>
            {
                Success = false,
                StatusCode = HttpStatusCode.UnprocessableEntity,
                Message = ResponseMessages.UnprocessableEntity,
            };
            //Act
            var actualResponse = await eventProviderService.GetAll(fakeDTO);

            //Assert
            mock.Mock<IEventProviderRepo>()
                .Verify(repo => repo.GetAll(fakeDTO), Times.Never);

            mock.Mock<IMapper>()
                .Verify(mapper => mapper.Map<List<EventProviderListedResult>>(list), Times.Never);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model, expectedResponse.Model);
        }
        [Fact]
        public async void GetAll_RecordsDoesntExist()
        {
            using var mock = AutoMock.GetLoose();
            //Arange

            // var fakeName = "LOL";
            var fakeEventProvider = GetSampleEventProviders()[0];
            var fakeEventProviderDTO = new EventProviderListedResult
            {
                Name = fakeEventProvider.Name,
            };
            var fakeDTO = new GetAllEventProvidersQuery()
            {
                pageNumber = 1,
                pageSize = 10,
                searchTarget = "LOL",
                isVerified = true,

            };
            List<EventProviderListedResult> listFake = new List<EventProviderListedResult>();
            

            List<EventProvider> list = new List<EventProvider>();
            

            mock.Mock<IEventProviderRepo>()
                .Setup(repo => repo.GetAll(fakeDTO))
                .Returns(Task.FromResult(list));
            //.Returns(Task.FromResult((EventProvider) null));

            mock.Mock<IMapper>()
                .Setup(mapper => mapper.Map<List<EventProviderListedResult>>(list))
                .Returns(listFake);

            var eventProviderService = mock.Create<EventProviderService>();

            var expectedResponse = new OutputResponse<List<EventProviderListedResult>>
            {
                Success = true,
                StatusCode = HttpStatusCode.Accepted,
                Message = ResponseMessages.Success,
                Model = new List<EventProviderListedResult>(),
                //_mapper.Map<List<EventProviderListedResult>>(eventProviders)
            };
            //Act
            var actualResponse = await eventProviderService.GetAll(fakeDTO);

            //Assert
            mock.Mock<IEventProviderRepo>()
                .Verify(repo => repo.GetAll(fakeDTO), Times.Once);

            mock.Mock<IMapper>()
                .Verify(mapper => mapper.Map<List<EventProviderListedResult>>(list), Times.Once);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model.Count, expectedResponse.Model.Count);
        }
        [Fact]
        public async void GetAll_RecordsExist()
        {
            using var mock = AutoMock.GetLoose();
            //Arange
            
           // var fakeName = "LOL";
            var fakeEventProvider = GetSampleEventProviders()[0];
            var fakeEventProviderDTO = new EventProviderListedResult
            {
                Name = fakeEventProvider.Name,
            };
            var fakeDTO = new GetAllEventProvidersQuery()
            {
                pageNumber = 1,
                pageSize = 10,
                searchTarget = "LOL",
                isVerified = true,

            };
            List<EventProviderListedResult> listFake = new List<EventProviderListedResult>();
            listFake.Add(fakeEventProviderDTO);

            List<EventProvider> list = new List<EventProvider>();
            list.Add(fakeEventProvider);

            mock.Mock<IEventProviderRepo>()
                .Setup(repo => repo.GetAll(fakeDTO))
                .Returns(Task.FromResult(list));

            mock.Mock<IMapper>()
                .Setup(mapper => mapper.Map<List<EventProviderListedResult>>(list))
                .Returns(listFake);

            var eventProviderService = mock.Create<EventProviderService>();

            var expectedResponse = new OutputResponse<List<EventProviderListedResult>>
            {
                Success = true,
                StatusCode = HttpStatusCode.Accepted,
                Message = ResponseMessages.Success,
                Model = listFake,
                //_mapper.Map<List<EventProviderListedResult>>(eventProviders)
            };
            //Act
            var actualResponse = await eventProviderService.GetAll(fakeDTO);

            //Assert
            mock.Mock<IEventProviderRepo>()
                .Verify(repo => repo.GetAll(fakeDTO), Times.Once);

            mock.Mock<IMapper>()
                .Verify(mapper => mapper.Map<List<EventProviderListedResult>>(list), Times.Once);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model[0].Name, expectedResponse.Model[0].Name);
        }
        // GetSingle tests
        [Fact]
        public async void GetSingle_RecordExists()
        {
            using var mock = AutoMock.GetLoose();
            //Arange
            var fakeName = "LOL";
            var fakeEventProvider = GetSampleEventProviders()[0];
            var fakeEventProviderDTO = new EventProviderSingleResult
            {
                Name = fakeEventProvider.Name,
            };

            mock.Mock<IEventProviderRepo>()
                .Setup(repo => repo.GetSingle(fakeName))
                .Returns(Task.FromResult(fakeEventProvider));
            
            mock.Mock<IMapper>()
                .Setup(mapper => mapper.Map<EventProviderSingleResult>(fakeEventProvider))
                .Returns(fakeEventProviderDTO);

            var eventProviderService = mock.Create<EventProviderService>();

            var expectedResponse = new OutputResponse<EventProviderSingleResult>
            {
                Success = true,
                StatusCode = HttpStatusCode.Accepted,
                Message = ResponseMessages.Success,
                Model = fakeEventProviderDTO,
            };
            //Act
            var actualResponse = await eventProviderService.GetSingle(fakeName);

            //Assert
            mock.Mock<IEventProviderRepo>()
                .Verify(repo => repo.GetSingle(fakeName), Times.Once);

            mock.Mock<IMapper>()
                .Verify(mapper => mapper.Map<EventProviderSingleResult>(fakeEventProvider), Times.Once);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model.Name, expectedResponse.Model.Name);
        }

        [Fact]
        [ExpectedException(typeof(System.NullReferenceException))]
        public async void GetSingle_RecordDoesNotExist()
        {
            using var mock = AutoMock.GetLoose();
            //Arange
            var fakeName = "LOL";
            var fakeEventProvider = GetSampleEventProviders()[0];
            var fakeEventProviderDTO = new EventProviderSingleResult
            {
                Name = fakeEventProvider.Name,
            };

            mock.Mock<IEventProviderRepo>()
                .Setup(repo => repo.GetSingle(fakeName))
                //.Returns<EventProvider>(null); //fix
                .Returns(Task.FromResult((EventProvider) null));

            mock.Mock<IMapper>()
                .Setup(mapper => mapper.Map<EventProviderSingleResult>(fakeEventProvider))
                .Returns(fakeEventProviderDTO);

            var eventProviderService = mock.Create<EventProviderService>(); //return null

            var expectedResponse = new OutputResponse<EventProviderSingleResult>
            {
                Success = false,
                StatusCode = HttpStatusCode.NotFound,
                Message = ResponseMessages.Failure,
                Model = null,
            };
            //Act
            var actualResponse = await eventProviderService.GetSingle(fakeName);

            //Assert
            mock.Mock<IEventProviderRepo>()
                .Verify(repo => repo.GetSingle(fakeName), Times.Once);

            mock.Mock<IMapper>()
                .Verify(mapper => mapper.Map<EventProviderSingleResult>(fakeEventProvider), Times.Never);

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
            string fakeName = null;
            var fakeEventProvider = GetSampleEventProviders()[0];
            var fakeEventProviderDTO = new EventProviderSingleResult
            {
                Name = fakeEventProvider.Name,
            };

            mock.Mock<IEventProviderRepo>()
                .Setup(repo => repo.GetSingle(fakeName))
                .Returns(Task.FromResult(fakeEventProvider));

            mock.Mock<IMapper>()
                .Setup(mapper => mapper.Map<EventProviderSingleResult>(fakeEventProvider))
                .Returns(fakeEventProviderDTO);

            var eventProviderService = mock.Create<EventProviderService>();

            var expectedResponse = new OutputResponse<EventProviderSingleResult>
            {
                Success = false,
                StatusCode = HttpStatusCode.UnprocessableEntity,
                Message = ResponseMessages.UnprocessableEntity,
                Model = null,
            };
            //Act
            var actualResponse = await eventProviderService.GetSingle(fakeName);

            //Assert
            mock.Mock<IEventProviderRepo>()
                .Verify(repo => repo.GetSingle(fakeName), Times.Never);

            mock.Mock<IMapper>()
                .Verify(mapper => mapper.Map<EventProviderSingleResult>(fakeEventProvider), Times.Never);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model, expectedResponse.Model);
        }

        private List<EventProvider> GetSampleEventProviders()
        {
            var sample = new List<EventProvider>();
            sample.Add(new EventProvider
            {
                Name = "LOL",
            });
            sample.Add(new EventProvider
            {
                Name = "Mostafa",
            });
            sample.Add(new EventProvider
            {
                Name = "Tarek",
            });
            sample.Add(new EventProvider
            {
                Name = "Shosh",
            });

            return sample;
        }
    }
}
