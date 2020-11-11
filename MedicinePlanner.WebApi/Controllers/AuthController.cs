using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using MedicinePlanner.Core.Exceptions;
using MedicinePlanner.Core.Services.Interfaces;
using MedicinePlanner.Data.Models;
using MedicinePlanner.WebApi.Auth.Dtos;
using MedicinePlanner.WebApi.Auth.Extensions;
using MedicinePlanner.WebApi.Auth.Services.Interfaces;
using MedicinePlanner.WebApi.Configs;
using MedicinePlanner.WebApi.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace MedicinePlanner.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IOptions<GoogleSecretsOptions> _options;
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        public AuthController(IOptions<GoogleSecretsOptions> options, IUserService userService, 
            IAuthService authService, IMapper mapper)
        {
            _options = options;
            _userService = userService;
            _authService = authService;
            _mapper = mapper;
        }

        public HttpResponseMessage Options()
        {
            return new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
        }

        [HttpPost("Google")]
        public async Task<ActionResult<LoginResponseDto>> Google(GoogleAuthDto googleAuth)
        {
            Payload payload;
            try
            {
                payload = await ValidateAsync(googleAuth.IdToken, new ValidationSettings
                {
                    Audience = new[] { _options.Value.clientId }
                });

                UserReadDto userReadDto = _mapper.Map<UserReadDto>(await _userService.GetByEmailAsync(payload.Email));
                if(userReadDto != null)
                {
                    return Ok(_authService.Login(userReadDto));
                }
                UserCreateDto userToCreate = new UserCreateDto 
                { 
                    Email = payload.Email, 
                    Name = payload.GivenName, 
                    Surname = payload.FamilyName,
                    Photo = payload.Picture
                };
                
                User newUser = await _userService.AddAsync(_mapper.Map<User>(userToCreate));
                UserReadDto newUserDto = _mapper.Map<UserReadDto>(newUser);

                return Ok(_authService.Login(newUserDto));
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

        [HttpGet("Me")]
        [Authorize]
        public async Task<ActionResult<UserReadDto>> Me()
        {
            Guid userId = User.GetUserId();
            return Ok(_mapper.Map<UserReadDto>(await _userService.GetByIdAsync(userId)));
        }
    }
}
