using AutoMapper;
using Guldtand.Data.Entities;
using Guldtand.Domain.Models.DTOs;

namespace Guldtand.Domain.Helpers
{
    class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();
        }
    }
}
