using AutoMapper;
using MedicinePlanner.Data.Models;
using MedicinePlanner.WebApi.Dtos;

namespace MedicinePlanner.WebApi.Profiles
{
    public class PharmaceuticlaFormProfile : Profile
    {
        public PharmaceuticlaFormProfile()
        {
            CreateMap<PharmaceuticalForm, PharmaceuticalFormDto>();
            CreateMap<PharmaceuticalFormCreateDto, PharmaceuticalForm>();
            CreateMap<PharmaceuticalFormDto, PharmaceuticalForm>();
        }
    }
}
