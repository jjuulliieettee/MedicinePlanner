using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MedicinePlanner.Core.Exceptions;
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
            return Ok(_mapper.Map<IEnumerable<MedicineScheduleReadDto>>(await _medicineScheduleService.GetAllByMedicineAndUserIdAsync(name, userId)));
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
            if (ModelState.IsValid)
            {
                try
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
                catch (ApiException ex)
                {
                    return StatusCode(ex.StatusCode, new { error = true, message = ex.Message });
                }
            }
            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] MedicineScheduleEditDto medicineSchedule,
            [FromHeader(Name = "GoogleToken")] string accessToken)
        {
            medicineSchedule.Id = id;
            if (ModelState.IsValid)
            {
                try
                {
                    Guid userId = User.GetUserId();
                    medicineSchedule.UserId = userId;
                    await _medicineScheduleService.EditAsync(_mapper.Map<MedicineSchedule>(medicineSchedule));

                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        await _googleCalendarService.SetEvents(userId, accessToken);
                    }

                    return Ok(new { message = "Success" });
                }
                catch (ApiException ex)
                {
                    return StatusCode(ex.StatusCode, new { error = true, message = ex.Message });
                }
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id, [FromHeader(Name = "GoogleToken")] string accessToken)
        {
            try
            {
                await _medicineScheduleService.DeleteAsync(id);

                Guid userId = User.GetUserId();

                if (!string.IsNullOrEmpty(accessToken))
                {
                    await _googleCalendarService.SetEvents(userId, accessToken);
                }

                return Ok(new { message = "Success" });
            }
            catch (ApiException ex)
            {
                return StatusCode(ex.StatusCode, new { error = true, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = true, message = ex.Message });
            }
        }

        [HttpGet("GetFoodSchedules/{medicineScheduleId}")]
        public async Task<ActionResult<IEnumerable<FoodScheduleReadDto>>> GetAllFoodSchedules([FromRoute] Guid medicineScheduleId)
        {
            return Ok(_mapper.Map<IEnumerable<FoodScheduleReadDto>>(
                await _foodScheduleService.GetAllByMedicineScheduleIdAsync(medicineScheduleId)));
        }

        [HttpGet("{medicineScheduleId}/SearchFoodSchedule")]
        public async Task<ActionResult<FoodScheduleReadDto>> SearchFoodSchedule([FromQuery] DateTimeOffset date, 
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
            try
            {
                Guid userId = User.GetUserId();

                await _foodScheduleService.EditAllBasedOnFoodScheduleAsync(id, userId);

                if (!string.IsNullOrEmpty(accessToken))
                {
                    await _googleCalendarService.SetEvents(userId, accessToken);
                }

                return Ok(new { message = "Success"});
            }
            catch (ApiException ex)
            {
                return StatusCode(ex.StatusCode, new { error = true, message = ex.Message });
            }
        }

        [HttpPut("FoodSchedules/{id}")]
        public async Task<ActionResult> EditFoodSchedule([FromRoute] Guid id, [FromBody]FoodScheduleEditDto foodScheduleEditDto,
            [FromHeader(Name = "GoogleToken")] string accessToken)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    foodScheduleEditDto.Id = id;
                    Guid userId = User.GetUserId();
                    FoodSchedule foodSchedule = _mapper.Map<FoodSchedule>(foodScheduleEditDto);

                    await _foodScheduleService.EditAsync(foodSchedule, userId);

                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        await _googleCalendarService.SetEvents(userId, accessToken);
                    }

                    return Ok(new { message = "Success" });
                }
                catch (ApiException ex)
                {
                    return StatusCode(ex.StatusCode, new { error = true, message = ex.Message });
                }
            }
            return BadRequest();
        }

        [HttpDelete("FoodSchedules/{id}")]
        public async Task<IActionResult> DeleteFoodSchedule([FromRoute] Guid id, [FromHeader(Name = "GoogleToken")] string accessToken)
        {
            try
            {
                await _foodScheduleService.DeleteAsync(id);

                Guid userId = User.GetUserId();

                if (!string.IsNullOrEmpty(accessToken))
                {
                    await _googleCalendarService.SetEvents(userId, accessToken);
                }

                return Ok(new { message = "Success" });
            }
            catch (ApiException ex)
            {
                return StatusCode(ex.StatusCode, new { error = true, message = ex.Message });
            }
        }

    }
}
