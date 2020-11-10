using AutoMapper;
using MedicinePlanner.Data.Models;
using MedicinePlanner.WebApi.Dtos;

namespace MedicinePlanner.WebApi.Profiles
{
    public class FoodScheduleProfile : Profile
    {
        public FoodScheduleProfile()
        {
            CreateMap<FoodSchedule, FoodScheduleReadDto>();
            CreateMap<FoodScheduleAddDto, FoodSchedule>();
            CreateMap<FoodScheduleEditDto, FoodSchedule>();
        }
    }
}
