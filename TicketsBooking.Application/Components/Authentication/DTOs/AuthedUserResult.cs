using System;
using AutoMapper;
using TicketsBooking.Application.Common.Mappings;
using TicketsBooking.Domain.Entities;

namespace TicketsBooking.Application.Components.Authentication.DTOs
{
    public class AuthedUserResult: IMapFrom<User>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AuthedUserResult, User>().ReverseMap();
        }
    }
}
