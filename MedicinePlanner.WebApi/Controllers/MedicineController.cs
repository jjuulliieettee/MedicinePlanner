using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MedicinePlanner.Data.Models;
using MedicinePlanner.Core.Services.Interfaces;
using MedicinePlanner.WebApi.Dtos;
using AutoMapper;
using MedicinePlanner.Core.Exceptions;
using MedicinePlanner.Core.Resources;
using Microsoft.AspNetCore.Authorization;

namespace MedicinePlanner.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MedicineController : ControllerBase
    {
        private readonly IMedicineService _medicineService;
        private readonly IMapper _mapper;

        public MedicineController(IMedicineService medicineService, IMapper mapper)
        {
            _medicineService = medicineService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicineReadDto>>> GetAll([FromQuery] string name = null)
        {
            return Ok(_mapper.Map<IEnumerable<MedicineReadDto>>(await _medicineService.GetAllByNameAsync(name)));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MedicineReadDto>> Get([FromRoute] Guid id)
        {
            MedicineReadDto medicine = _mapper.Map<MedicineReadDto>(await _medicineService.GetByIdAsync(id));

            if (medicine == null)
            {
                throw new ApiException(MessagesResource.MEDICINE_NOT_FOUND);
            }

            return Ok(medicine);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] MedicineEditDto medicineEditDto)
        {
            medicineEditDto.Id = id;
            await _medicineService.EditAsync(_mapper.Map<Medicine>(medicineEditDto));
            return Ok(new { message = MessagesResource.SUCCESS_MESSAGE });
        }

        [HttpPost]
        public async Task<ActionResult<MedicineReadDto>> Post([FromBody] MedicineCreateDto medicine)
        {
            MedicineReadDto newMedicine = _mapper.Map<MedicineReadDto>(await _medicineService.AddAsync(_mapper.Map<Medicine>(medicine)));
            return CreatedAtAction("Get", new { id = newMedicine.Id }, newMedicine);
        }

        //[HttpDelete("{id}")]
        //public async Task<ActionResult<Medicine>> DeleteMedicine(Guid id)
        //{
        //    return NoContent();
        //}

    }
}
