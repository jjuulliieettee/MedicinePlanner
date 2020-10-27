using MedicinePlanner.WebApi.Auth.Dtos;
using MedicinePlanner.WebApi.Dtos;

namespace MedicinePlanner.WebApi.Auth.Services.Interfaces
{
    public interface IAuthService
    {
        LoginResponseDto Login(UserReadDto user);
    }
}
