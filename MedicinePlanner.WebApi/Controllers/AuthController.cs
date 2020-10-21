using System.Threading.Tasks;
using AutoMapper;
using MedicinePlanner.Core.Exceptions;
using MedicinePlanner.Core.Services.Interfaces;
using MedicinePlanner.Data.Models;
using MedicinePlanner.WebApi.Configs;
using MedicinePlanner.WebApi.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace MedicinePlanner.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IOptions<GoogleOptions> _options;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public AuthController(IOptions<GoogleOptions> options, IUserService userService, IMapper mapper)
        {
            _options = options;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<UserReadDto>> Google(GoogleAuthDto googleAuth)
        {
            Payload payload;
            try
            {
                payload = await ValidateAsync(googleAuth.IdToken, new ValidationSettings
                {
                    Audience = new[] { _options.Value.clientId }
                });

                UserReadDto userReadDto = _mapper.Map<UserReadDto>(await _userService.GetByEmail(payload.Email));
                if(userReadDto != null)
                {
                    return Ok(userReadDto);
                }
                UserCreateDto userToCreate = new UserCreateDto { Email = payload.Email };
                return Ok(_mapper.Map<UserReadDto>(await _userService.Add(_mapper.Map<User>(userToCreate))));
            }
            catch(ApiException ex)
            {
                return StatusCode(ex.StatusCode, new { error = true, message = ex.Message });
            }
            catch
            {
                return StatusCode(401, new { error = true, message = "Invalid token!" });
            }
        }
    }
}
