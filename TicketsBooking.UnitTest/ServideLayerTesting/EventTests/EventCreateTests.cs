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

namespace TicketsBooking.UnitTest.ServideLayerTesting.EventTests
{
    public  class EventCreateTests
    {
        [Fact]
        public async void Create_Valid()
        {
            using var mock = AutoMock.GetLoose();
            //Arrange
            CreateNewEventCommand command = new CreateNewEventCommand
            {
                ProviderName = "Lol",
                Title = "testTitle",
                Description = "description",
                Location = "location",
                DateTime = System.DateTime.Now,
                AllTickets = 20,
                SingleTicketPrice = 12,
                ReservationDueDate = System.DateTime.Now,
                Category = "category",
                SubCategory = "category",
                Participants = new List<string>(),
                Tags = new List<string>()
            };
            var createNewEventDTO = command;

            var fakeID = createNewEventDTO.ProviderName + createNewEventDTO.Title;
            EventProvider Provider = new EventProvider
            {
                Name = command.ProviderName,
                Email = "test@gmail.com"
            };
            var fakeEvent = new Event
            {
                EventID = fakeID,
                Provider = Provider,
                Title = command.Title,
                Description = command.Description,
                dateTime = command.DateTime,
                AllTickets = command.AllTickets,
                SingleTicketPrice = command.SingleTicketPrice,
                BoughtTickets = 0,
                ReservationDueDate = command.ReservationDueDate,
                Location = command.Location,
                Category = command.Category,
                SubCategory = command.SubCategory,
                Accepted = false,
                Tags = null,
                Participants = null,
            };
            if (command.Tags != null)
            {
                List<Tag> tags = new List<Tag>();
                foreach (string tag in command.Tags)
                {
                    Tag t = new Tag
                    {
                        Keyword = tag,
                    };
                    tags.Add(t);
                }
                fakeEvent.Tags = tags;
            }

            if (command.Participants != null)
            {
                List<Participant> participants = new List<Participant>();
                foreach (string p in command.Participants)
                {
                    Participant participant = new Participant
                    {
                        Name = p,
                    };
                    participants.Add(participant);
                }
                fakeEvent.Participants = participants;
            }

            var expectedResponse = new OutputResponse<bool>
            {
                Success = true,
                StatusCode = HttpStatusCode.Created,
                Message = ResponseMessages.Success,
                Model = true,
            };

            var fakeMailDTO = new MailModel
            {
                ToEmail = "test@email.com",
                Subject = "test",
                Body = "test",
            };

            //Act
            mock.Mock<IEventRepo>()
                .Setup(repo => repo.Create(command))
                .Returns(Task.FromResult(fakeEvent));

            mock.Mock<IEventRepo>()
                .Setup(repo => repo.GetSingle(fakeID))
                .Returns(Task.FromResult((Event)null));

            mock.Mock<IMailService>()
                .Setup(mailService => mailService.SendEmailAsync(fakeMailDTO));

            mock.Mock<IEventProviderRepo>()
                .Setup(repo => repo.GetSingleByName(Provider.Name))
                .Returns(Task.FromResult(Provider));

            var eventService = mock.Create<EventService>();

            var actualResponse = await eventService.Create(command);

            //Assert
            mock.Mock<IEventRepo>()
                .Verify(repo => repo.GetSingle(fakeID), Times.Once);
            mock.Mock<IEventRepo>()
                .Verify(repo => repo.Create(command), Times.Once);
            mock.Mock<IMailService>()
                .Verify(mailService => mailService.SendEmailAsync(fakeMailDTO), Times.Never);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model, expectedResponse.Model);
        }

        [Fact]
        public async void Create_AlreadyExists()
        {
            using var mock = AutoMock.GetLoose();
            //Arrange
            CreateNewEventCommand command = new CreateNewEventCommand
            {
                ProviderName = "Lol",
                Title = "testTitle",
                Description = "description",
                Location = "location",
                DateTime = System.DateTime.Now,
                AllTickets = 20,
                SingleTicketPrice = 12,
                ReservationDueDate = System.DateTime.Now,
                Category = "category",
                SubCategory = "category",
                Participants = new List<string>(),
                Tags = new List<string>()
            };
            var createNewEventDTO = command;

            var fakeID = createNewEventDTO.ProviderName + createNewEventDTO.Title;
            var fakeEvent = new Event
            {
                EventID = fakeID,
                Provider = new EventProvider
                {
                    Name = command.ProviderName
                },
                Title = command.Title,
                Description = command.Description,
                dateTime = command.DateTime,
                AllTickets = command.AllTickets,
                SingleTicketPrice = command.SingleTicketPrice,
                BoughtTickets = 0,
                ReservationDueDate = command.ReservationDueDate,
                Location = command.Location,
                Category = command.Category,
                SubCategory = command.SubCategory,
                Accepted = false,
                Tags = null,
                Participants = null,
            };
            if (command.Tags != null)
            {
                List<Tag> tags = new List<Tag>();
                foreach (string tag in command.Tags)
                {
                    Tag t = new Tag
                    {
                        Keyword = tag,
                    };
                    tags.Add(t);
                }
                fakeEvent.Tags = tags;
            }

            if (command.Participants != null)
            {
                List<Participant> participants = new List<Participant>();
                foreach (string p in command.Participants)
                {
                    Participant participant = new Participant
                    {
                        Name = p,
                    };
                    participants.Add(participant);
                }
                fakeEvent.Participants = participants;
            }

            var expectedResponse = new OutputResponse<bool>
            {
                Success = false,
                StatusCode = HttpStatusCode.BadRequest,
                Message = ResponseMessages.Failure,
                Model = false
            };

            var fakeMailDTO = new MailModel
            {
                ToEmail = "test@email.com",
                Subject = "test",
                Body = "test",
            };

            //Act
            mock.Mock<IEventRepo>()
                .Setup(repo => repo.Create(command))
                .Returns(Task.FromResult(fakeEvent));

            mock.Mock<IEventRepo>()
                .Setup(repo => repo.GetSingle(fakeID))
                .Returns(Task.FromResult(fakeEvent));

            mock.Mock<IMailService>()
                .Setup(mailService => mailService.SendEmailAsync(fakeMailDTO));

            var eventService = mock.Create<EventService>();

            var actualResponse = await eventService.Create(command);

            //Assert
            mock.Mock<IEventRepo>()
                .Verify(repo => repo.GetSingle(fakeID), Times.Once);
            mock.Mock<IEventRepo>()
                .Verify(repo => repo.Create(command), Times.Never);
            mock.Mock<IMailService>()
                .Verify(mailService => mailService.SendEmailAsync(fakeMailDTO), Times.Never);

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model, expectedResponse.Model);
        }
    
        [Theory]
        [ClassData(typeof(CreateInvalidTests))]
        public async void Create_Invalid(CreateNewEventCommand command)
        {
            using var mock = AutoMock.GetLoose();
            //Arrange
            var createNewEventDTO = command;

            var fakeID = createNewEventDTO.ProviderName + createNewEventDTO.Title;
            var fakeEvent = new Event
            {
                EventID = fakeID,
                Provider = new EventProvider
                {
                    Name = command.ProviderName
                },
                Title = command.Title,
                Description = command.Description,
                dateTime = command.DateTime,
                AllTickets = command.AllTickets,
                SingleTicketPrice = command.SingleTicketPrice,
                BoughtTickets = 0,
                ReservationDueDate = command.ReservationDueDate,
                Location = command.Location,
                Category = command.Category,
                SubCategory = command.SubCategory,
                Accepted = false,
                Tags = null,
                Participants = null,
            };
            if(command.Tags != null)
            {
                List<Tag> tags = new List<Tag>();
                foreach(string tag in command.Tags)
                {
                    Tag t = new Tag
                    {
                        Keyword = tag,
                    };
                    tags.Add(t);
                }
                fakeEvent.Tags = tags;
            }

            if (command.Participants != null)
            {
                List<Participant> participants = new List<Participant>();
                foreach (string p in command.Participants)
                {
                    Participant participant = new Participant
                    {
                        Name = p,
                    };
                    participants.Add(participant);
                }
                fakeEvent.Participants = participants;
            }

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
            mock.Mock<IEventRepo>()
                .Setup(repo => repo.Create(command))
                .Returns(Task.FromResult(fakeEvent));

            mock.Mock<IEventRepo>()
                .Setup(repo => repo.GetSingle(fakeID))
                .Returns(Task.FromResult(fakeEvent));

            mock.Mock<IMailService>()
                .Setup(mailService => mailService.SendEmailAsync(fakeMailDTO));

            var eventProviderService = mock.Create<EventService>();

            var actualResponse = await eventProviderService.Create(command);

            //Assert
            mock.Mock<IEventRepo>()
                .Verify(repo => repo.GetSingle(fakeID), Times.Never);
            mock.Mock<IEventRepo>()
                .Verify(repo => repo.Create(command), Times.Never);
            mock.Mock<IMailService>()
                .Setup(mailService => mailService.SendEmailAsync(fakeMailDTO));

            Assert.NotNull(actualResponse);
            Assert.Equal(actualResponse.Success, expectedResponse.Success);
            Assert.Equal(actualResponse.StatusCode, expectedResponse.StatusCode);
            Assert.Equal(actualResponse.Message, expectedResponse.Message);
            Assert.Equal(actualResponse.Model, expectedResponse.Model);
        }
    }
    public class CreateInvalidTests : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            var NullProvider = new CreateNewEventCommand
            {
                ProviderName = null,
                Title = "testTitle",
                Description = "description",
                Location = "location",
                DateTime = System.DateTime.Now,
                AllTickets = 20,
                SingleTicketPrice = 12,
                ReservationDueDate = System.DateTime.Now,
                Category = "category",
                SubCategory = "category",
                Participants = new List<string>(),
                Tags = new List<string>()
            };
            var NullTitle = new CreateNewEventCommand
            {
                ProviderName = "Lol",
                Title = null,
                Description = "description",
                Location = "location",
                DateTime = System.DateTime.Now,
                AllTickets = 20,
                SingleTicketPrice = 12,
                ReservationDueDate = System.DateTime.Now,
                Category = "category",
                SubCategory = "category",
                Participants = new List<string>(),
                Tags = new List<string>()
            };
            var NullDescription = new CreateNewEventCommand
            {
                ProviderName = "Lol",
                Title = "testTitle",
                Description = null,
                Location = "location",
                DateTime = System.DateTime.Now,
                AllTickets = 20,
                SingleTicketPrice = 12,
                ReservationDueDate = System.DateTime.Now,
                Category = "category",
                SubCategory = "category",
                Participants = new List<string>(),
                Tags = new List<string>()
            };
            var NullLocation = new CreateNewEventCommand
            {
                ProviderName = "Lol",
                Title = "testTitle",
                Description = "description",
                Location = null,
                DateTime = System.DateTime.Now,
                AllTickets = 20,
                SingleTicketPrice = 12,
                ReservationDueDate = System.DateTime.Now,
                Category = "category",
                SubCategory = "category",
                Participants = new List<string>(),
                Tags = new List<string>()
            };
            var NullCategory = new CreateNewEventCommand
            {
                ProviderName = "Lol",
                Title = "testTitle",
                Description = "description",
                Location = "location",
                DateTime = System.DateTime.Now,
                AllTickets = 20,
                SingleTicketPrice = 12,
                ReservationDueDate = System.DateTime.Now,
                Category = null,
                SubCategory = "category",
                Participants = new List<string>(),
                Tags = new List<string>()
            };
            var NullSubCategory = new CreateNewEventCommand
            {
                ProviderName = "Lol",
                Title = "testTitle",
                Description = "description",
                Location = "location",
                DateTime = System.DateTime.Now,
                AllTickets = 20,
                SingleTicketPrice = 12,
                ReservationDueDate = System.DateTime.Now,
                Category = "category",
                SubCategory = null,
                Participants = new List<string>(),
                Tags = new List<string>()
            };
            var NullParticipants = new CreateNewEventCommand
            {
                ProviderName = "Lol",
                Title = "testTitle",
                Description = "description",
                Location = "location",
                DateTime = System.DateTime.Now,
                AllTickets = 20,
                SingleTicketPrice = 12,
                ReservationDueDate = System.DateTime.Now,
                Category = "category",
                SubCategory = "category",
                Participants = null,
                Tags = new List<string>()
            };
            var NullTags = new CreateNewEventCommand
            {
                ProviderName = "Lol",
                Title = "testTitle",
                Description = "description",
                Location = "location",
                DateTime = System.DateTime.Now,
                AllTickets = 20,
                SingleTicketPrice = 12,
                ReservationDueDate = System.DateTime.Now,
                Category = "category",
                SubCategory = "category",
                Participants = new List<string>(),
                Tags = null
            };
            var NullCombination = new CreateNewEventCommand
            {
                ProviderName = null,
                Title = "testTitle",
                Description = "description",
                Location = "location",
                DateTime = System.DateTime.Now,
                AllTickets = 20,
                SingleTicketPrice = 12,
                ReservationDueDate = System.DateTime.Now,
                Category = null,
                SubCategory = "category",
                Participants = null,
                Tags = null
            };

            yield return new object[] { NullProvider };
            yield return new object[] { NullTitle };
            yield return new object[] { NullDescription };
            yield return new object[] { NullLocation };
            yield return new object[] { NullCategory };
            yield return new object[] { NullSubCategory };
            yield return new object[] { NullParticipants };
            yield return new object[] { NullTags };
            yield return new object[] { NullCombination };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}


