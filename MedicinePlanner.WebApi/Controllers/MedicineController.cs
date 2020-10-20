using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MedicinePlanner.Data.Models;
using MedicinePlanner.Core.Services.Interfaces;
using MedicinePlanner.WebApi.Dtos;
using AutoMapper;
using MedicinePlanner.Core.Exceptions;

namespace MedicinePlanner.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<ActionResult<IEnumerable<MedicineReadDto>>> GetAll()
        {
            return Ok(_mapper.Map<IEnumerable<MedicineReadDto>>(await _medicineService.GetAll()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MedicineReadDto>> Get(Guid id)
        {
            MedicineReadDto medicine = _mapper.Map<MedicineReadDto>(await _medicineService.GetById(id));

            if (medicine == null)
            {
                return NotFound();
            }

            return Ok(medicine);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, Medicine medicine)
        {
            //add edit code
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<MedicineReadDto>> Post(MedicineCreateDto medicine)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    MedicineReadDto newMedicine = _mapper.Map<MedicineReadDto>(await _medicineService.Add(_mapper.Map<Medicine>(medicine)));
                    return CreatedAtAction("Get", new { id = newMedicine.Id }, newMedicine);
                }
                catch(ApiException ex)
                {
                    return StatusCode(ex.StatusCode, new { error = true, message = ex.Message });
                }
            }
            return BadRequest(); 
        }

        //[HttpDelete("{id}")]
        //public async Task<ActionResult<Medicine>> DeleteMedicine(Guid id)
        //{
        //    return NoContent();
        //}

    }
}
