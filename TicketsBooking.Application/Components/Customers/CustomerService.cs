using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsBooking.Application.Common.Responses;
using TicketsBooking.Application.Components.Authentication;
using TicketsBooking.Application.Components.Authentication.DTOs;
using TicketsBooking.Application.Components.Customers.DTOs.Command;
using TicketsBooking.Application.Components.Customers.DTOs.Query;
using TicketsBooking.Application.Components.Customers.DTOs.Results;
using TicketsBooking.Domain.Entities;
using FluentValidation;
using TicketsBooking.Application.Components.Authentication.Vlidators;
using TicketsBooking.Application.Components.Customers.Validators;
using TicketsBooking.Integration.Email;
using System.Net;
using TicketsBooking.Crosscut.Constants;
using TicketsBooking.Integration.Email.Models;

using BC = BCrypt.Net.BCrypt;

namespace TicketsBooking.Application.Components.Customers
{

    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepo _customerRepo;
        private readonly IMapper _mapper;
        private readonly ITokenManager _tokenManager;
        private readonly IMailService _mailSerivce;
        private readonly AbstractValidator<GetAllUsersQuery> _getAllQueryValidator;
        private readonly AbstractValidator<RegisterCustomerCommand> _RegisterValidator;
        private readonly AbstractValidator<AuthCreds> _authCredsValidator;
        public CustomerService(ICustomerRepo eventProviderRepo, ITokenManager tokenManager,
                                    IMapper mapper, IMailService mailService)
        {
            _customerRepo = eventProviderRepo;
            _mapper = mapper;
            _tokenManager = tokenManager;
            _mailSerivce = mailService;
            _getAllQueryValidator = new GetAllQueryValidator();
            _RegisterValidator = new RegisterCommandValidator();
            _authCredsValidator = new AuthCredsValidator();
        }
        public async Task<OutputResponse<AuthedUserResult>> Authenticate(AuthCreds authCreds)
        {
            var isValid = _authCredsValidator.Validate(authCreds).IsValid;
            if (!isValid)
            {
                return new OutputResponse<AuthedUserResult>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.UnprocessableEntity,
                    Message = ResponseMessages.UnprocessableEntity,
                    Model = null,
                };
            }

            var customer = await _customerRepo.GetSingleByEmail(authCreds.Email);

            if (customer != null &&
                BC.Verify(authCreds.Password, customer.Password) &&
                customer.Accepted)
            {
                var authUserResult = _mapper.Map<AuthedUserResult>(customer);
                authUserResult.Token = _tokenManager.GenerateToken(customer, Roles.Customer);

                return new OutputResponse<AuthedUserResult>
                {
                    Success = true,
                    StatusCode = HttpStatusCode.Accepted,
                    Message = ResponseMessages.Success,
                    Model = authUserResult,
                };
            }

            return new OutputResponse<AuthedUserResult>
            {
                Success = false,
                StatusCode = HttpStatusCode.Unauthorized,
                Message = ResponseMessages.Unauthorized,
                Model = null,
            };
        }
        public async Task<OutputResponse<bool>> Approve(string Email)
        {
            if (string.IsNullOrEmpty(Email))
            {
                return new OutputResponse<bool>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.UnprocessableEntity,
                    Message = ResponseMessages.UnprocessableEntity,
                };
            }

            var customer = await _customerRepo.GetSingleByEmail(Email);
            if (customer == null)
            {
                return new OutputResponse<bool>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = ResponseMessages.Failure,
                    Model = false,
                };
            }
            await _customerRepo.Approve(Email);
            return new OutputResponse<bool>
            {
                Success = true,
                StatusCode = HttpStatusCode.Accepted,
                Message = ResponseMessages.Success,
                Model = true,
            };
        }
        public async Task<OutputResponse<bool>> Delete(string Email)
        {
            if (string.IsNullOrEmpty(Email))
            {
                return new OutputResponse<bool>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.UnprocessableEntity,
                    Message = ResponseMessages.UnprocessableEntity,
                };
            }
            var customer = await _customerRepo.GetSingleByEmail(Email);
            if (customer == null)
            {
                return new OutputResponse<bool>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = ResponseMessages.Failure,
                };
            }

            await _customerRepo.Delete(Email);
            return new OutputResponse<bool>
            {
                Success = true,
                StatusCode = HttpStatusCode.Accepted,
                Message = ResponseMessages.Success,
            };
        }

        public async Task<OutputResponse<List<GetAllUserListedResult>>> GetAll(GetAllUsersQuery query)
        {
            var isValid = _getAllQueryValidator.Validate(query).IsValid;
            if (!isValid)
            {
                return new OutputResponse<List<GetAllUserListedResult>>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.UnprocessableEntity,
                    Message = ResponseMessages.UnprocessableEntity,
                };
            }
            var customers = await _customerRepo.GetAll(query);
            return new OutputResponse<List<GetAllUserListedResult>>
            {
                Success = true,
                StatusCode = HttpStatusCode.Accepted,
                Message = ResponseMessages.Success,
                Model = _mapper.Map<List<GetAllUserListedResult>>(customers),
            };
        }

        public async Task<OutputResponse<GetSingleUserResult>> GetSingle(string Email)
        {
            if (string.IsNullOrEmpty(Email))
            {
                return new OutputResponse<GetSingleUserResult>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.UnprocessableEntity,
                    Message = ResponseMessages.UnprocessableEntity,
                    Model = null,
                };
            }

            var customer = await _customerRepo.GetSingleByEmail(Email);

            if (customer == null)
            {
                return new OutputResponse<GetSingleUserResult>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = ResponseMessages.Failure,
                    Model = null,
                };
            }

            return new OutputResponse<GetSingleUserResult>
            {
                Success = true,
                StatusCode = HttpStatusCode.Accepted,
                Message = ResponseMessages.Success,
                Model = _mapper.Map<GetSingleUserResult>(customer),
            };
        }

        public async Task<OutputResponse<bool>> Register(RegisterCustomerCommand command)
        {
            var isValid = _RegisterValidator.Validate(command).IsValid;
            if (!isValid)
            {
                return new OutputResponse<bool>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.UnprocessableEntity,
                    Message = ResponseMessages.UnprocessableEntity,
                };
            }
            // 1 account per email 
            if (await _customerRepo.GetSingleByEmail(command.Email) != null)
            {
                return new OutputResponse<bool>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = ResponseMessages.Failure,
                };
            }

            await _customerRepo.Register(command);
            await sendActivateAccountEmail(command.Email);
            return new OutputResponse<bool>
            {
                Success = true,
                StatusCode = HttpStatusCode.Created,
                Message = ResponseMessages.Success,
                Model = true,
            };
        }
        public Task<OutputResponse<Customer>> UpdateInfo()
        {
            throw new NotImplementedException();
        }
        private async Task sendActivateAccountEmail(string destinationEmail)
        {
            var mailModel = new MailModel
            {
                ToEmail = destinationEmail,
                Subject = "Tazkara account activation mail",
                Body = "Your account in Tazkara has been created and is pending your activation...\n" +
                "Please click on the link bellow to activate your account",
                // WHERE IS THE LINK !!!?
            };
            await _mailSerivce.SendEmailAsync(mailModel);
        }

    }
}
