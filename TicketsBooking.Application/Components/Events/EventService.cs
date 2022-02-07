using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using TicketsBooking.Application.Common.Responses;
using TicketsBooking.Application.Components.Authentication;
using TicketsBooking.Application.Components.Authentication.Vlidators;
using TicketsBooking.Application.Components.EventProviders;
using TicketsBooking.Application.Components.Events.DTOs.Commands;
using TicketsBooking.Application.Components.Events.DTOs.Queries;
using TicketsBooking.Application.Components.Events.DTOs.Results;
using TicketsBooking.Application.Components.Events.Validators;
using TicketsBooking.Crosscut.Constants;
using TicketsBooking.Domain.Entities;
using TicketsBooking.Integration.Email;
using TicketsBooking.Integration.Email.Models;

namespace TicketsBooking.Application.Components.Events
{
    public class EventService : IEventService
    {
        private readonly IEventRepo _eventRepo;
        private readonly IEventProviderRepo _eventProviderRepo;
        private readonly IMapper _mapper;
        private readonly ITokenManager _tokenManager;
        private readonly IMailService _mailSerivce;
        private readonly AbstractValidator<CreateNewEventCommand> _createEventCommandValidator;
        private readonly AbstractValidator<GetAllEventsQuery> _getAllQueryValidator;
        private readonly AbstractValidator<AuthCreds> _authCredsValidator;
        public EventService(IEventRepo eventRepo, IEventProviderRepo eventProviderRepo ,ITokenManager tokenManager,
                            IMapper mapper, IMailService mailService)
        {
            _eventRepo = eventRepo;
            _eventProviderRepo = eventProviderRepo;
            _mapper = mapper;
            _tokenManager = tokenManager;
            _mailSerivce = mailService;
            _createEventCommandValidator = new CreateEventCommandValidator();
            _getAllQueryValidator = new GetAllEventQueryValidator();
            _authCredsValidator = new AuthCredsValidator();
        }

        public async Task<OutputResponse<bool>> Create(CreateNewEventCommand command)
        {
            var isValid = _createEventCommandValidator.Validate(command).IsValid;
            if (!isValid)
            {
                return new OutputResponse<bool>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.UnprocessableEntity,
                    Message = ResponseMessages.UnprocessableEntity,
                };
            }
            string NewEventID = command.ProviderName + command.Title;
            // event already exists
            if (await _eventRepo.GetSingle(NewEventID) != null)
            {
                return new OutputResponse<bool>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = ResponseMessages.Failure,
                };
            }

            await _eventRepo.Create(command);
            var eventProvider = await _eventProviderRepo.GetSingleByName(command.ProviderName);
            await SendPendingRequestEmail(eventProvider.Email);
            return new OutputResponse<bool>
            {
                Success = true,
                StatusCode = HttpStatusCode.Created,
                Message = ResponseMessages.Success,
                Model = true,
            };
            
        }

        public async Task<OutputResponse<bool>> Delete(string EventID)
        {
            // verify that the id is not null or empty
            if (string.IsNullOrEmpty(EventID))
            {
                return new OutputResponse<bool>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.UnprocessableEntity,
                    Message = ResponseMessages.UnprocessableEntity,
                    Model = false,
                };
            }
            //verify that it exists
            Event e = await _eventRepo.GetSingle(EventID);
            if (e == null)
            {
                return new OutputResponse<bool>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = ResponseMessages.Failure,
                    Model = false,
                };
            }
            // if it exists then we delete
            await _eventRepo.Delete(EventID);
            return new OutputResponse<bool>
            {
                Success = true,
                StatusCode = HttpStatusCode.Accepted,
                Message = ResponseMessages.Success,
                Model = true,
            };
        }

        public async Task<OutputResponse<List<EventListedResult>>> GetAll(GetAllEventsQuery query)
        {
            var isValid = _getAllQueryValidator.Validate(query).IsValid;
            if (!isValid)
            {
                return new OutputResponse<List<EventListedResult>>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.UnprocessableEntity,
                    Message = ResponseMessages.UnprocessableEntity,
                };
            }
            var e = await _eventRepo.GetAll(query);
            return new OutputResponse<List<EventListedResult>>
            {
                Success = true,
                StatusCode = HttpStatusCode.Accepted,
                Message = ResponseMessages.Success,
                Model = _mapper.Map<List<EventListedResult>>(e),
            };


            throw new NotImplementedException();
        }

        public async Task<OutputResponse<EventSingleResult>> GetSingle(string EventID)
        {
            if (string.IsNullOrEmpty(EventID))
            {
                return new OutputResponse<EventSingleResult>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.UnprocessableEntity,
                    Message = ResponseMessages.UnprocessableEntity,
                    Model = null,
                };
            }

            Event eventRecord = await _eventRepo.GetSingle(EventID);
            if (eventRecord == null)
            {
                return new OutputResponse<EventSingleResult>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = ResponseMessages.Failure,
                    Model = null,
                };
            }

            var result = _mapper.Map<EventSingleResult>(eventRecord);
            result.Tags.Clear();
            foreach(Tag tag in eventRecord.Tags)
            {
                result.Tags.Add(tag.Keyword);
            }

            return new OutputResponse<EventSingleResult>
            {
                Success = true,
                StatusCode = HttpStatusCode.Accepted,
                Message = ResponseMessages.Success,
                Model = result,
            };
        }

        public async Task<OutputResponse<bool>> Accept(string eventId)
        {
            if (string.IsNullOrEmpty(eventId))
            {
                return new OutputResponse<bool>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.UnprocessableEntity,
                    Message = ResponseMessages.UnprocessableEntity,
                };
            }

            var eventEntity = await _eventRepo.GetSingle(eventId);
            if (eventEntity == null)
            {
                return new OutputResponse<bool>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = ResponseMessages.Failure,
                    Model = false,
                };
            }

            var command = new SetAcceptedCommand
            {
                ID = eventId,
                Accepted = true,
            };

            await _eventRepo.UpdateAccepted(command);
            await SendApproveEmail(eventEntity.Provider.Email);
            return new OutputResponse<bool>
            {
                Success = true,
                StatusCode = HttpStatusCode.Accepted,
                Message = ResponseMessages.Success,
                Model = true,
            };
        }

        public async Task<OutputResponse<bool>> Decline(string eventId)
        {
            if (string.IsNullOrEmpty(eventId))
            {
                return new OutputResponse<bool>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.UnprocessableEntity,
                    Message = ResponseMessages.UnprocessableEntity,
                };
            }

            var eventEntity = await _eventRepo.GetSingle(eventId);
            if (eventEntity == null)
            {
                return new OutputResponse<bool>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = ResponseMessages.Failure,
                };
            }

            await _eventRepo.Delete(eventId);
            await SendDeclineEmail(eventEntity.Provider.Email);
            return new OutputResponse<bool>
            {
                Success = true,
                StatusCode = HttpStatusCode.Accepted,
                Message = ResponseMessages.Success,
            };
        }


        public Task<OutputResponse<EventSingleResult>> Update(UpdateEventCommand command)
        {
            throw new NotImplementedException();
        }
        // used for sending emails concerning the proposal result
        private async Task SendApproveEmail(string destinationEmail)
        {
            var mailModel = new MailModel
            {
                ToEmail = destinationEmail,
                Subject = "Your proposal has been ACCEPTED!",
                Body = "The Event has been created successfully!",
            };
            await _mailSerivce.SendEmailAsync(mailModel);
        }

        private async Task SendDeclineEmail(string destinationEmail)
        {
            var mailModel = new MailModel
            {
                ToEmail = destinationEmail,
                Subject = "Sorry, Your request has been DECLINED",
                Body = "Sadly, Your Event proposal hasn't met our acceptance standards.",
            };
            await _mailSerivce.SendEmailAsync(mailModel);
        }

        private async Task SendPendingRequestEmail(string destinationEmail)
        {
            var mailModel = new MailModel
            {
                ToEmail = destinationEmail,
                Subject = "Your proposal has been received successfully",
                Body = "Your proposal is being processed..We will get back to you in 8 hours.",
            };
            await _mailSerivce.SendEmailAsync(mailModel);
        }

        public async Task<OutputResponse<List<EventListedResult>>> Filter(string query)
        {
            var isValid = !string.IsNullOrEmpty(query);
            if (!isValid)
            {
                return new OutputResponse<List<EventListedResult>>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.UnprocessableEntity,
                    Message = ResponseMessages.UnprocessableEntity,
                };
            }
            var e = await _eventRepo.Filter(query);
            return new OutputResponse<List<EventListedResult>>
            {
                Success = true,
                StatusCode = HttpStatusCode.Accepted,
                Message = ResponseMessages.Success,
                Model = _mapper.Map<List<EventListedResult>>(e),
            };


            throw new NotImplementedException();
        }

        public async Task<OutputResponse<List<EventListedResult>>> Search(string query)
        {
            var isValid = !string.IsNullOrEmpty(query);
            if (!isValid)
            {
                return new OutputResponse<List<EventListedResult>>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.UnprocessableEntity,
                    Message = ResponseMessages.UnprocessableEntity,
                };
            }
            var e = await _eventRepo.Search(query);
            return new OutputResponse<List<EventListedResult>>
            {
                Success = true,
                StatusCode = HttpStatusCode.Accepted,
                Message = ResponseMessages.Success,
                Model = _mapper.Map<List<EventListedResult>>(e),
            };


            throw new NotImplementedException();
        }
    }

}
