﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MedicinePlanner.Core.Exceptions;
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
        private readonly IMapper _mapper;
        public MedicineSchedulesController(IMapper mapper, IMedicineScheduleService medicineScheduleService, IFoodScheduleService foodScheduleService)
        {
            _medicineScheduleService = medicineScheduleService;
            _foodScheduleService = foodScheduleService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicineScheduleReadDto>>> GetAll([FromQuery] string name = null)
        {
            Guid userId = User.GetUserId();
            return Ok(_mapper.Map<IEnumerable<MedicineScheduleReadDto>>(await _medicineScheduleService.GetAllByMedicineAndUserIdAsync(name, userId)));
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<MedicineScheduleReadDto>>> Post([FromBody]FoodScheduleAddDto foodAndMedicineSchedules)
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
        public async Task<IActionResult> Put([FromRoute]Guid id, [FromBody]MedicineScheduleEditDto medicineSchedule)
        {
            medicineSchedule.Id = id;
            if (ModelState.IsValid)
            {
                try
                {
                    Guid userId = User.GetUserId();
                    medicineSchedule.UserId = userId;
                    await _medicineScheduleService.EditAsync(_mapper.Map<MedicineSchedule>(medicineSchedule));
                    return NoContent();
                }
                catch(ApiException ex)
                {
                    return StatusCode(ex.StatusCode, new { error = true, message = ex.Message });
                }
            }
            return BadRequest();            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute]Guid id)
        {
            try
            {
                await _medicineScheduleService.DeleteAsync(id);
                return NoContent();
            }
            catch (ApiException ex)
            {
                return StatusCode(ex.StatusCode, new { error = true, message = ex.Message });
            }
        }

        [HttpGet("GetForMedicineSchedule/{medicineScheduleId}")]
        public async Task<ActionResult<IEnumerable<FoodScheduleReadDto>>> GetAllFoodSchedules([FromRoute]Guid medicineScheduleId)
        {
            return Ok(_mapper.Map<IEnumerable<FoodScheduleReadDto>>(await _foodScheduleService.GetAllByMedicineScheduleIdAsync(medicineScheduleId)));
        }

        [HttpGet("GetForMedicineSchedule/{medicineScheduleId}/SearchFoodSchedule")]
        public async Task<ActionResult<FoodScheduleReadDto>> SearchFoodSchedule([FromQuery]DateTimeOffset date, [FromRoute]Guid medicineScheduleId)
        {
            return Ok(_mapper.Map<FoodScheduleReadDto>(await _foodScheduleService.GetByDateAsync(date, medicineScheduleId)));
        }

        [HttpPost("FoodSchedules/MakeDefault")]
        public async Task<ActionResult> MakeDefault([FromBody]FoodScheduleEditDto foodScheduleDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Guid userId = User.GetUserId();
                    FoodSchedule foodSchedule = _mapper.Map<FoodSchedule>(foodScheduleDto);

                    await _foodScheduleService.EditAllBasedOnFoodScheduleAsync(foodSchedule, userId);

                    return NoContent();
                }
                catch (ApiException ex)
                {
                    return StatusCode(ex.StatusCode, new { error = true, message = ex.Message });
                }
            }
            return BadRequest();
        }

        [HttpPut("FoodSchedules/{id}")]
        public async Task<ActionResult> EditFoodSchedule([FromRoute]Guid id, [FromBody]FoodScheduleEditDto foodScheduleEditDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    foodScheduleEditDto.Id = id;
                    Guid userId = User.GetUserId();
                    FoodSchedule foodSchedule = _mapper.Map<FoodSchedule>(foodScheduleEditDto);

                    await _foodScheduleService.EditAsync(foodSchedule, userId);

                    return NoContent();
                }
                catch (ApiException ex)
                {
                    return StatusCode(ex.StatusCode, new { error = true, message = ex.Message });
                }
            }
            return BadRequest();
        }

        [HttpDelete("FoodSchedules/{id}")]
        public async Task<IActionResult> DeleteFoodSchedule([FromRoute]Guid id)
        {
            try
            {
                await _foodScheduleService.DeleteAsync(id);
                return NoContent();
            }
            catch (ApiException ex)
            {
                return StatusCode(ex.StatusCode, new { error = true, message = ex.Message });
            }
        }

    }
}
