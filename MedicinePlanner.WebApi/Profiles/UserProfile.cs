using AutoMapper;
using MedicinePlanner.Data.Models;
using MedicinePlanner.WebApi.Dtos;

namespace MedicinePlanner.WebApi.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserReadDto>();
            CreateMap<UserCreateDto, User>();
        }
    }
}
