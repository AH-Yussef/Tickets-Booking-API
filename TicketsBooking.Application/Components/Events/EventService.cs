using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using TicketsBooking.Application.Common.Responses;
using TicketsBooking.Application.Components.Authentication;
using TicketsBooking.Application.Components.Authentication.DTOs;
using TicketsBooking.Application.Components.Authentication.Vlidators;
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
        private readonly IMapper _mapper;
        private readonly ITokenManager _tokenManager;
        private readonly IMailService _mailSerivce;
        private readonly AbstractValidator<CreateNewEventCommand> _createEventCommandValidator;
        private readonly AbstractValidator<SetAcceptedCommand> _SetAcceptedCommandValidator;
        private readonly AbstractValidator<GetAllEventsQuery> _getAllQueryValidator;
        private readonly AbstractValidator<AuthCreds> _authCredsValidator;
        public EventService(IEventRepo eventRepo, ITokenManager tokenManager,
                            IMapper mapper, IMailService mailService)
        {
            _eventRepo = eventRepo;
            _mapper = mapper;
            _tokenManager = tokenManager;
            _mailSerivce = mailService;
            _createEventCommandValidator = new CreateEventCommandValidator();
            //_setVerifiedCommandValidator = new SetVerifiedCommandValidator();
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
            // after verifying that the event doesn't exist before
            // we create the event entry and send an email to the organization
            await _eventRepo.Create(command);
            //var prov = 
            await sendPendingRequestEmail(command.Provider.Email);
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
                // not sure if it works needs to be checked
                // if not, the mapping can be done easily by hand using a simple foreach loop
                // can be replaced by an extra method that takes an event and returns an EventListedResult
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

            Event e = await _eventRepo.GetSingle(EventID);
            if (e == null)
            {
                return new OutputResponse<EventSingleResult>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = ResponseMessages.Failure,
                    Model = null,
                };
            }
            EventSingleResult esr = new EventSingleResult
            {

            };
            return new OutputResponse<EventSingleResult>
            {
                Success = true,
                StatusCode = HttpStatusCode.Accepted,
                Message = ResponseMessages.Success,
                // not sure if it works needs to be checked
                // if not, the mapping can be done easily by hand using a simple foreach loop
                // can be replaced by an extra method that takes an event and returns an EventSingleResult
                // Model = _mapper.Map<EventSingleResult>(e), 
                Model = esr
            };
        }

        public async Task<OutputResponse<bool>> Accept(SetAcceptedCommand command)
        {
            var isValid = _SetAcceptedCommandValidator.Validate(command).IsValid;
            if (!isValid)
            {
                return new OutputResponse<bool>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.UnprocessableEntity,
                    Message = ResponseMessages.UnprocessableEntity,
                };
            }

            var eventID = await _eventRepo.GetSingle(command.ID);
            if (eventID == null)
            {
                return new OutputResponse<bool>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = ResponseMessages.Failure,
                    Model = false,
                };
            }

            await _eventRepo.UpdateAccepted(command);
            await sendApproveEmail(eventID.Provider.Email);
            return new OutputResponse<bool>
            {
                Success = true,
                StatusCode = HttpStatusCode.Accepted,
                Message = ResponseMessages.Success,
                Model = true,
            };
        }

        public async Task<OutputResponse<bool>> Decline(SetAcceptedCommand command)
        {
            if (string.IsNullOrEmpty(command.ID))
            {
                return new OutputResponse<bool>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.UnprocessableEntity,
                    Message = ResponseMessages.UnprocessableEntity,
                };
            }

            var eventID = await _eventRepo.GetSingle(command.ID);
            if (eventID == null)
            {
                return new OutputResponse<bool>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = ResponseMessages.Failure,
                };
            }

            await _eventRepo.Delete(command.ID);
            await sendDeclineEmail(eventID.Provider.Email);
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
        private async Task sendApproveEmail(string destinationEmail)
        {
            var mailModel = new MailModel
            {
                ToEmail = destinationEmail,
                Subject = "Your proposal has been ACCEPTED!",
                Body = "The Event has been created successfully!",
            };
            await _mailSerivce.SendEmailAsync(mailModel);
        }

        private async Task sendDeclineEmail(string destinationEmail)
        {
            var mailModel = new MailModel
            {
                ToEmail = destinationEmail,
                Subject = "Sorry, Your request has been DECLINED",
                Body = "Sadly, Your proposal hasn't met our acceptance standards.",
            };
            await _mailSerivce.SendEmailAsync(mailModel);
        }

        private async Task sendPendingRequestEmail(string destinationEmail)
        {
            var mailModel = new MailModel
            {
                ToEmail = destinationEmail,
                Subject = "Your proposal has been received successfully",
                Body = "Your proposal is being processed..We will get back to you in 24 hours.",
            };
            await _mailSerivce.SendEmailAsync(mailModel);
        }
    }

}
