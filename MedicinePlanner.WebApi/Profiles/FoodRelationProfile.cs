using AutoMapper;
using MedicinePlanner.Data.Models;
using MedicinePlanner.WebApi.Dtos;

namespace MedicinePlanner.WebApi.Profiles
{
    public class FoodRelationProfile : Profile
    {
        public FoodRelationProfile()
        {
            CreateMap<FoodRelation, FoodRelationDto>();
            CreateMap<FoodRelationCreateDto, FoodRelation>();
            CreateMap<FoodRelationDto, FoodRelation>();
        }
    }
}
