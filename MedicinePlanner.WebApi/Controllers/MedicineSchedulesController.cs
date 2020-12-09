using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MedicinePlanner.Core.Resources;
using MedicinePlanner.Core.Services.GoogleCalendar;
using MedicinePlanner.Core.Services.Interfaces;
using MedicinePlanner.Data.Models;
using MedicinePlanner.WebApi.Auth.Extensions;
using MedicinePlanner.WebApi.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicinePlanner.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MedicineSchedulesController : ControllerBase
    {
        private readonly IMedicineScheduleService _medicineScheduleService;
        private readonly IFoodScheduleService _foodScheduleService;
        private readonly IGoogleCalendarService _googleCalendarService;
        private readonly IMapper _mapper;
        public MedicineSchedulesController(IMapper mapper, IMedicineScheduleService medicineScheduleService, 
            IFoodScheduleService foodScheduleService, IGoogleCalendarService googleCalendarService)
        {
            _medicineScheduleService = medicineScheduleService;
            _foodScheduleService = foodScheduleService;
            _googleCalendarService = googleCalendarService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicineScheduleReadDto>>> GetAll([FromQuery] string name = null)
        {
            Guid userId = User.GetUserId();
            return Ok(_mapper.Map<IEnumerable<MedicineScheduleReadDto>>(
                await _medicineScheduleService.GetAllByMedicineAndUserIdAsync(name, userId)));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MedicineScheduleReadDto>> GetById([FromRoute] Guid id)
        {
            return Ok(_mapper.Map<MedicineScheduleReadDto>(await _medicineScheduleService.GetByIdAsync(id)));
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<MedicineScheduleReadDto>>> Post([FromBody] FoodScheduleAddDto foodAndMedicineSchedules, 
            [FromHeader(Name = "GoogleToken")] string accessToken)
        {
            Guid userId = User.GetUserId();
            IEnumerable<MedicineScheduleAddDto> medicineScheduleAddDtos = foodAndMedicineSchedules.MedicineSchedules;

            IEnumerable<MedicineSchedule> medicineSchedules = await _medicineScheduleService.AddAsync(
                _mapper.Map<IEnumerable<MedicineSchedule>>(medicineScheduleAddDtos), userId);

            await _foodScheduleService.AddAsync(_mapper.Map<FoodSchedule>(foodAndMedicineSchedules), medicineSchedules);

            IEnumerable<MedicineScheduleReadDto> newMedSchedulesDto = _mapper.Map<IEnumerable<MedicineScheduleReadDto>>(medicineSchedules);

            if (!string.IsNullOrEmpty(accessToken))
            {
                await _googleCalendarService.SetEvents(userId, accessToken);
            }

            return CreatedAtAction("GetAll", newMedSchedulesDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] MedicineScheduleEditDto medicineSchedule,
            [FromHeader(Name = "GoogleToken")] string accessToken)
        {
            medicineSchedule.Id = id;
            Guid userId = User.GetUserId();
            medicineSchedule.UserId = userId;
            await _medicineScheduleService.EditAsync(_mapper.Map<MedicineSchedule>(medicineSchedule));

            if (!string.IsNullOrEmpty(accessToken))
            {
                await _googleCalendarService.SetEvents(userId, accessToken);
            }

            return Ok(new { message = MessagesResource.SUCCESS_MESSAGE });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id, [FromHeader(Name = "GoogleToken")] string accessToken)
        {
            await _medicineScheduleService.DeleteAsync(id);

            Guid userId = User.GetUserId();
            if (!string.IsNullOrEmpty(accessToken))
            {
                await _googleCalendarService.SetEvents(userId, accessToken);
            }

            return Ok(new { message = MessagesResource.SUCCESS_MESSAGE });
        }

        [HttpGet("GetFoodSchedules/{medicineScheduleId}")]
        public async Task<ActionResult<IEnumerable<FoodScheduleReadDto>>> GetAllFoodSchedules([FromRoute] Guid medicineScheduleId)
        {
            return Ok(_mapper.Map<IEnumerable<FoodScheduleReadDto>>(
                await _foodScheduleService.GetAllByMedicineScheduleIdAsync(medicineScheduleId)));
        }

        [HttpGet("{medicineScheduleId}/SearchFoodSchedule")]
        public async Task<ActionResult<FoodScheduleReadDto>> SearchFoodSchedule([FromQuery] DateTime date, 
            [FromRoute] Guid medicineScheduleId)
        {
            return Ok(_mapper.Map<FoodScheduleReadDto>(await _foodScheduleService.GetByDateAsync(date, medicineScheduleId)));
        }

        [HttpGet("FoodSchedules/{id}")]
        public async Task<ActionResult<FoodScheduleReadDto>> GetFoodScheduleById([FromRoute] Guid id)
        {
            return Ok(_mapper.Map<FoodScheduleReadDto>(await _foodScheduleService.GetByIdAsync(id)));
        }

        [HttpPost("FoodSchedules/MakeDefault/{id}")]
        public async Task<ActionResult> MakeDefault([FromRoute] Guid id, [FromHeader(Name = "GoogleToken")] string accessToken)
        {
            Guid userId = User.GetUserId();

            await _foodScheduleService.EditAllBasedOnFoodScheduleAsync(id, userId);
            if (!string.IsNullOrEmpty(accessToken))
            {
                await _googleCalendarService.SetEvents(userId, accessToken);
            }

            return Ok(new { message = MessagesResource.SUCCESS_MESSAGE });
        }

        [HttpPut("FoodSchedules/{id}")]
        public async Task<ActionResult> EditFoodSchedule([FromRoute] Guid id, [FromBody]FoodScheduleEditDto foodScheduleEditDto,
            [FromHeader(Name = "GoogleToken")] string accessToken)
        {
            foodScheduleEditDto.Id = id;
            Guid userId = User.GetUserId();
            FoodSchedule foodSchedule = _mapper.Map<FoodSchedule>(foodScheduleEditDto);

            await _foodScheduleService.EditAsync(foodSchedule, userId);

            if (!string.IsNullOrEmpty(accessToken))
            {
                await _googleCalendarService.SetEvents(userId, accessToken);
            }

            return Ok(new { message = MessagesResource.SUCCESS_MESSAGE });
        }

        [HttpDelete("FoodSchedules/{id}")]
        public async Task<IActionResult> DeleteFoodSchedule([FromRoute] Guid id, [FromHeader(Name = "GoogleToken")] string accessToken)
        {
            await _foodScheduleService.DeleteAsync(id);

            Guid userId = User.GetUserId();

            if (!string.IsNullOrEmpty(accessToken))
            {
                await _googleCalendarService.SetEvents(userId, accessToken);
            }

            return Ok(new { message = MessagesResource.SUCCESS_MESSAGE });
        }

    }
}
