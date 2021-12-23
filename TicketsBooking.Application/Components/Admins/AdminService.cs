using System;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using TicketsBooking.Application.Common.Responses;
using TicketsBooking.Application.Components.Authentication;
using TicketsBooking.Application.Components.Authentication.DTOs;
using TicketsBooking.Application.Components.Authentication.Vlidators;
using TicketsBooking.Crosscut.Constants;

namespace TicketsBooking.Application.Components.Admins
{
    public class AdminService: IAdminService
    {
        private readonly IAdminRepo _adminRepo;
        private readonly ITokenManager _tokenManager;
        private readonly IMapper _mapper;
        private readonly AbstractValidator<AuthCreds> _validator;

        public AdminService(IAdminRepo adminRepo,
                                    ITokenManager tokenManager,
                                    IMapper mapper)
        {
            _adminRepo = adminRepo;
            _tokenManager = tokenManager;
            _mapper = mapper;
            _validator = new AuthCredsValidator();
        }

        public async Task<OutputResponse<AuthedUserResult>> Authenticate(AuthCreds authCreds)
        {
            var isValid = _validator.Validate(authCreds).IsValid;
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

            var eventProvider = await _adminRepo.GetSingle(authCreds.Email);

            if (eventProvider != null && authCreds.Password == eventProvider.Password)
            {
                _validator.Validate(authCreds);
                var authUserResult = _mapper.Map<AuthedUserResult>(eventProvider);
                authUserResult.Token = _tokenManager.GenerateToken(eventProvider, Roles.Admin);

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
                Message = ResponseMessages.Unauthenticated,
                Model = null,
            };
        }
    }
}
