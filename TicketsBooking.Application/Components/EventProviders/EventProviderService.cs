using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using TicketsBooking.Application.Common.Responses;
using TicketsBooking.Application.Components.EventProviders.DTOs.Commands;
using TicketsBooking.Application.Components.EventProviders.DTOs.Queries;
using TicketsBooking.Application.Components.EventProviders.DTOs.Results;
using TicketsBooking.Application.Components.EventProviders.Validators;
using TicketsBooking.Crosscut.Constants;

namespace TicketsBooking.Application.Components.EventProviders
{
    public class EventProviderService: IEventProviderService
    {
        private readonly IEventProviderRepo _eventRepo;
        private readonly IMapper _mapper;
        private readonly AbstractValidator<CreateEventProviderCommand> _createEventProviderCommandValidator;
        private readonly AbstractValidator<SetVerifiedCommand> _setVerifiedCommandValidator;
        private readonly AbstractValidator<GetAllEventProvidersQuery> _getAllQueryValidator;
        public EventProviderService(IEventProviderRepo eventRepo, IMapper mapper)
        {
            _eventRepo = eventRepo;
            _mapper = mapper;
            _createEventProviderCommandValidator = new CreateEventProviderCommandValidator();
            _setVerifiedCommandValidator = new SetVerifiedCommandValidator();
            _getAllQueryValidator = new GetAllQueryValidator();
        }

        public async Task<OutputResponse<bool>> Delete(string name)
        {
            if(string.IsNullOrEmpty(name))
            {
                return new OutputResponse<bool>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.UnprocessableEntity,
                    Message = ResponseMessages.UnprocessableEntity,
                };
            }

            if(await _eventRepo.GetSingle(name) == null)
            {
                return new OutputResponse<bool>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = ResponseMessages.Failure,
                };
            }

            await _eventRepo.Delete(name);

            return new OutputResponse<bool>
            {
                Success = true,
                StatusCode = HttpStatusCode.Accepted,
                Message = ResponseMessages.Success,
            };
        }

        public async Task<OutputResponse<bool>> DoesEventProviderAlreadyExist(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return new OutputResponse<bool>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.UnprocessableEntity,
                    Message = ResponseMessages.UnprocessableEntity,
                };
            }

            var doesExist = await _eventRepo.GetSingle(name) == null;
            return new OutputResponse<bool>
            {
                Success = true,
                StatusCode = HttpStatusCode.Accepted,
                Message = ResponseMessages.Success,
                Model = doesExist,
            };
        } 

        public async Task<OutputResponse<List<EventProviderListedResult>>> GetAll(GetAllEventProvidersQuery query)
        {
            var isValid = _getAllQueryValidator.Validate(query).IsValid;
            if (!isValid)
            {
                return new OutputResponse<List<EventProviderListedResult>>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.UnprocessableEntity,
                    Message = ResponseMessages.UnprocessableEntity,
                };
            }
            var eventProviders = await _eventRepo.GetAll(query);
            return new OutputResponse<List<EventProviderListedResult>>
            {
                Success = true,
                StatusCode = HttpStatusCode.Accepted,
                Message = ResponseMessages.Success,
                Model = _mapper.Map<List<EventProviderListedResult>>(eventProviders),
            };
        }

        public async Task<OutputResponse<EventProviderSingleResult>> GetSingle(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return new OutputResponse<EventProviderSingleResult>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.UnprocessableEntity,
                    Message = ResponseMessages.UnprocessableEntity,
                    Model = null,
                };
            }

            var eventProvider = await _eventRepo.GetSingle(name);

            if(eventProvider == null)
            {
                return new OutputResponse<EventProviderSingleResult>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = ResponseMessages.Failure,
                    Model = null,
                };
            }

            return new OutputResponse<EventProviderSingleResult>
            {
                Success = true,
                StatusCode = HttpStatusCode.Accepted,
                Message = ResponseMessages.Success,
                Model = _mapper.Map<EventProviderSingleResult>(eventProvider),
            };
        }

        public async Task<OutputResponse<bool>> Register(CreateEventProviderCommand command)
        {
            var isValid = _createEventProviderCommandValidator.Validate(command).IsValid;
            if(!isValid)
            {
                return new OutputResponse<bool>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.UnprocessableEntity,
                    Message = ResponseMessages.UnprocessableEntity,
                };
            }

            if (await _eventRepo.GetSingle(command.Name) != null)
            {
                return new OutputResponse<bool>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = ResponseMessages.Failure,
                };
            }

            await _eventRepo.Create(command);
            return new OutputResponse<bool>
            {
                Success = true,
                StatusCode = HttpStatusCode.Created,
                Message = ResponseMessages.Success,
                Model = true,
            };
        }

        public async Task<OutputResponse<bool>> UpdateVerified(SetVerifiedCommand command)
        {
            var isValid = _setVerifiedCommandValidator.Validate(command).IsValid;
            if (!isValid)
            {
                return new OutputResponse<bool>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.UnprocessableEntity,
                    Message = ResponseMessages.UnprocessableEntity,
                };
            }

            bool doesExist = await _eventRepo.UpdateVerified(command);
            return new OutputResponse<bool>
            {
                Success = true,
                StatusCode = HttpStatusCode.Accepted,
                Message = ResponseMessages.Success,
                Model = doesExist,
            };
        }
    }
}
