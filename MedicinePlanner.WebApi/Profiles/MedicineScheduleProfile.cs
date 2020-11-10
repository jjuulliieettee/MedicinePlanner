using AutoMapper;
using MedicinePlanner.Data.Models;
using MedicinePlanner.WebApi.Dtos;

namespace MedicinePlanner.WebApi.Profiles
{
    public class MedicineScheduleProfile : Profile
    {
        public MedicineScheduleProfile()
        {
            CreateMap<MedicineSchedule, MedicineScheduleReadDto>();
            CreateMap<MedicineScheduleAddDto, MedicineSchedule>();
            CreateMap<MedicineScheduleEditDto, MedicineSchedule>();
        }
    }
}
