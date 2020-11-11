using AutoMapper;
using MedicinePlanner.Data.Models;
using MedicinePlanner.WebApi.Dtos;

namespace MedicinePlanner.WebApi.Profiles
{
    public class MedicineProfile : Profile
    {
        public MedicineProfile()
        {
            CreateMap<Medicine, MedicineReadDto>()
                .ForMember(field => field.FoodRelationName, opt => opt.MapFrom(src => src.FoodRelation.Name))
                .ForMember(field => field.PharmaceuticalFormName, opt => opt.MapFrom(src => src.PharmaceuticalForm.Name));
            CreateMap<MedicineCreateDto, Medicine>();
            CreateMap<MedicineEditDto, Medicine>();
        }
    }
}
