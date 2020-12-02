using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MedicinePlanner.Data.Models;
using MedicinePlanner.Core.Services.Interfaces;
using AutoMapper;
using MedicinePlanner.WebApi.Dtos;
using MedicinePlanner.Core.Exceptions;
using MedicinePlanner.Core.Resources;
using Microsoft.AspNetCore.Authorization;
using MedicinePlanner.Data.Enums;

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
            return Ok(_mapper.Map<IEnumerable<FoodRelationDto>>(await _foodRelationService.GetAllAsync()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FoodRelationDto>> Get([FromRoute] FoodRelationType id)
        {
            FoodRelationDto foodRelation = _mapper.Map<FoodRelationDto>(await _foodRelationService.GetByIdAsync(id));

            if (foodRelation == null)
            {
                throw new ApiException(MessagesResource.FOOD_RELATION_NOT_FOUND);
            }

            return Ok(foodRelation);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<FoodRelationDto>> Put([FromRoute] FoodRelationType id, [FromBody]FoodRelationDto foodRelation)
        {
            foodRelation.Id = id;
            await _foodRelationService.EditAsync(_mapper.Map<FoodRelation>(foodRelation));
            return Ok(new { message = MessagesResource.SUCCESS_MESSAGE });
        }

        [HttpPost]
        public async Task<ActionResult<FoodRelationDto>> Post([FromBody]FoodRelationCreateDto foodRelation)
        {
            FoodRelationDto newFoodRelation = _mapper.Map<FoodRelationDto>(
                await _foodRelationService.AddAsync(_mapper.Map<FoodRelation>(foodRelation))
            );
            return CreatedAtAction("Get", new { id = newFoodRelation.Id }, newFoodRelation);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] FoodRelationType id)
        {
            await _foodRelationService.DeleteAsync(id);
            return Ok(new { message = MessagesResource.SUCCESS_MESSAGE });
        }

    }
}
