using System;
using System.Threading.Tasks;
using Google.Apis.Calendar.v3.Data;
using MedicinePlanner.Core.Services.GoogleCalendar;
using MedicinePlanner.WebApi.Auth.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicinePlanner.WebApi.Controllers
{
    //controller for test purposes

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GoogleCalendarController : ControllerBase
    {
        private readonly IGoogleCalendarService _googleCalendarService;
        public GoogleCalendarController(IGoogleCalendarService googleCalendarService)
        {
            _googleCalendarService = googleCalendarService;
        }

        [HttpGet("GetEvents")]
        public async Task<ActionResult<Events>> GetEvents()
        {
            Guid userId = User.GetUserId();

            await _googleCalendarService.SetEvents( userId );
            return Ok();
        }
    }
}
