using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using TicketsBooking.Application.Common.Responses;
using TicketsBooking.Application.Components.Authentication;
using TicketsBooking.Application.Components.Purchases.DTOs.Commands;
using TicketsBooking.Application.Components.Purchases.DTOs.RepoDTO;
using TicketsBooking.Application.Components.Purchases.DTOs.Results;
using TicketsBooking.Application.Components.Purchases.Validators;
using TicketsBooking.Crosscut.Constants;
using TicketsBooking.Domain.Entities;
using TicketsBooking.Integration.Email;

namespace TicketsBooking.Application.Components.Purchases
{
    public class PurchaseService:IPurchaseService
    {
        private readonly IPurchaseRepo _purchaseRepo;
        //private readonly IEventProviderRepo _eventProviderRepo;
        private readonly IMapper _mapper;
        private readonly ITokenManager _tokenManager;
        private readonly IMailService _mailSerivce;
        private readonly AbstractValidator<CreateNewPurchaseCommand> _createPurchaseCommandValidator;
        //private readonly AbstractValidator<GetAllEventsQuery> _getAllQueryValidator;
        //private readonly AbstractValidator<AuthCreds> _authCredsValidator;
        public PurchaseService(IPurchaseRepo purchaseRepo, ITokenManager tokenManager,
                            IMapper mapper, IMailService mailService)
        {
            _purchaseRepo = purchaseRepo;
            //_eventProviderRepo = eventProviderRepo;
            _mapper = mapper;
            _tokenManager = tokenManager;
            _mailSerivce = mailService;
            _createPurchaseCommandValidator = new CreatePurchaseCommandValidator();
            //_getAllQueryValidator = new GetAllEventQueryValidator();
            //_authCredsValidator = new AuthCredsValidator();
        }

        public async Task<OutputResponse<PurchaseSingleResult>> CreateNewPurchase(CreateNewPurchaseCommand command)
        {
            var isValid = _createPurchaseCommandValidator.Validate(command).IsValid;
            if (!isValid)
            {
                return new OutputResponse<PurchaseSingleResult>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.UnprocessableEntity,
                    Message = ResponseMessages.UnprocessableEntity,
                };
            }
            //string NewEventID = command.ProviderName + command.Title;
            // event already exists
            /*if (await _eventRepo.GetSingle(NewEventID) != null)
            {
                return new OutputResponse<bool>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = ResponseMessages.Failure,
                };
            }*/
            var purchase = await _purchaseRepo.CreateNewPurchase(command);
           // var singleResult = _mapper.Map<PurchaseSingleResult>(purchase);
            PurchaseSingleResult singleResult = new PurchaseSingleResult
            {
                
                PurchaseID = purchase.PurchaseID,
                ReservationDate = purchase.ReservationDate,
                TicketCount = purchase.TicketsCount,
                SingleTicketCost = purchase.SingleTicketCost
            };
           // var eventProvider = await _eventProviderRepo.GetSingleByName(command.ProviderName);
            //await SendPendingRequestEmail(eventProvider.Email);
            return new OutputResponse<PurchaseSingleResult>
            {
                Success = true,
                StatusCode = HttpStatusCode.Created,
                Message = ResponseMessages.Success,
                Model = singleResult,
            };
        }

        public async Task<OutputResponse<List<PurchaseSingleResult>>> GetAll(string customerID)
        {
            var isValid = !string.IsNullOrEmpty(customerID);
            if (!isValid)
            {
                return new OutputResponse<List<PurchaseSingleResult>>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.UnprocessableEntity,
                    Message = ResponseMessages.UnprocessableEntity,
                };
            }
            var ListDTO = await _purchaseRepo.GetAll(customerID);

            if(ListDTO == null)
            {
                return new OutputResponse<List<PurchaseSingleResult>>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = ResponseMessages.Failure,
                    Model = null,
                };
            }
            List<PurchaseSingleResult> psrl = new List<PurchaseSingleResult>();
            foreach(var item in ListDTO)
            {
                psrl.Add(new PurchaseSingleResult { 
                    PurchaseID = item.PurchaseID,
                    CustomerID = customerID,
                    EventID = item.EventID,
                    TicketCount = item.TicketsCount,
                    ReservationDate = item.ReservationDate,
                    SingleTicketCost = item.SingleTicketCost
                });
            }
            return new OutputResponse<List<PurchaseSingleResult>>
            {
                Success = true,
                StatusCode = HttpStatusCode.Accepted,
                Message = ResponseMessages.Success,
                Model = psrl,
            };
        }

        public async Task<OutputResponse<PurchaseSingleResult>> GetSingle(string purchaseID)
        {
            if (string.IsNullOrEmpty(purchaseID))
            {
                return new OutputResponse<PurchaseSingleResult>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.UnprocessableEntity,
                    Message = ResponseMessages.UnprocessableEntity,
                    Model = null,
                };
            }

            PurchaseRepoDTO purchase = await _purchaseRepo.GetSingle(purchaseID);
            if (purchase == null)
            {
                return new OutputResponse<PurchaseSingleResult>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = ResponseMessages.Failure,
                    Model = null,
                };
            }
            PurchaseSingleResult singleResult = new PurchaseSingleResult
            {

                PurchaseID = purchase.PurchaseID,
                ReservationDate = purchase.ReservationDate,
                TicketCount = purchase.TicketsCount,
                SingleTicketCost = purchase.SingleTicketCost,
                CustomerID = purchase.CustomerID,
                EventID = purchase.EventID,
            };
            //var result = _mapper.Map<EventSingleResult>(eventRecord);
            //result.Tags.Clear();

            /*foreach (Tag tag in eventRecord.Tags)
            {
                result.Tags.Add(tag.Keyword);
            }*/

            return new OutputResponse<PurchaseSingleResult>
            {
                Success = true,
                StatusCode = HttpStatusCode.Accepted,
                Message = ResponseMessages.Success,
                Model = singleResult,
            };
        }
    }
}
