﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MedicinePlanner.Data.Models;
using MedicinePlanner.Core.Services.Interfaces;
using AutoMapper;
using MedicinePlanner.WebApi.Dtos;
using MedicinePlanner.Core.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace MedicinePlanner.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FoodRelationsController : ControllerBase
    {
        private readonly IFoodRelationService _foodRelationService;
        private readonly IMapper _mapper;

        public FoodRelationsController(IFoodRelationService foodRelationService, IMapper mapper)
        {
            _foodRelationService = foodRelationService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FoodRelationDto>>> GetAll()
        {
            return Ok(_mapper.Map<IEnumerable<FoodRelationDto>>(await _foodRelationService.GetAll()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FoodRelationDto>> Get(Guid id)
        {
            FoodRelationDto foodRelation = _mapper.Map<FoodRelationDto>(await _foodRelationService.GetById(id));

            if (foodRelation == null)
            {
                return NotFound();
            }

            return Ok(foodRelation);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<FoodRelationDto>> Put(Guid id, FoodRelationDto foodRelation)
        {
            foodRelation.Id = id;
            try
            {
                return Ok(_mapper.Map<FoodRelationDto>(await _foodRelationService.Edit(_mapper.Map<FoodRelation>(foodRelation))));
            }
            catch (ApiException ex)
            {
                return StatusCode(ex.StatusCode, new { error = true, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<FoodRelationDto>> Post(FoodRelationCreateDto foodRelation)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    FoodRelationDto newFoodRelation = _mapper.Map<FoodRelationDto>(
                        await _foodRelationService.Add(_mapper.Map<FoodRelation>(foodRelation))
                        );
                    return CreatedAtAction("Get", new { id = newFoodRelation.Id }, newFoodRelation);
                }
                catch (ApiException ex)
                {
                    return StatusCode(ex.StatusCode, new { error = true, message = ex.Message });
                }
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _foodRelationService.Delete(id);
                return NoContent();
            }
            catch (ApiException ex)
            {
                return StatusCode(ex.StatusCode, new { error = true, message = ex.Message });
            }

        }

    }
}