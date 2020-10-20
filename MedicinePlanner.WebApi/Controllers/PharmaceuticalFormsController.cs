﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MedicinePlanner.Data.Models;
using MedicinePlanner.Core.Services.Interfaces;
using AutoMapper;
using MedicinePlanner.WebApi.Dtos;
using MedicinePlanner.Core.Exceptions;

namespace MedicinePlanner.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PharmaceuticalFormsController : ControllerBase
    {
        private readonly IPharmaceuticalFormService _pharmaceuticalFormService;
        private readonly IMapper _mapper;

        public PharmaceuticalFormsController(IPharmaceuticalFormService pharmaceuticalFormService, IMapper mapper)
        {
            _pharmaceuticalFormService = pharmaceuticalFormService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PharmaceuticalFormDto>>> GetAll()
        {
            return Ok(_mapper.Map<IEnumerable<PharmaceuticalFormDto>>(await _pharmaceuticalFormService.GetAll()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PharmaceuticalFormDto>> Get(Guid id)
        {
            PharmaceuticalFormDto pharmaceuticalForm = _mapper.Map<PharmaceuticalFormDto>(await _pharmaceuticalFormService.GetById(id));

            if (pharmaceuticalForm == null)
            {
                return NotFound();
            }

            return Ok(pharmaceuticalForm);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, PharmaceuticalFormDto pharmaceuticalForm)
        {
            pharmaceuticalForm.Id = id;
            try
            {
                return Ok(_mapper.Map<PharmaceuticalFormDto>(
                    await _pharmaceuticalFormService.Edit(_mapper.Map<PharmaceuticalForm>(pharmaceuticalForm))
                    ));
            }
            catch (ApiException ex)
            {
                return StatusCode(ex.StatusCode, new { error = true, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<PharmaceuticalFormDto>> Post(PharmaceuticalFormCreateDto pharmaceuticalForm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    PharmaceuticalFormDto newPharmaceuticalForm = _mapper.Map<PharmaceuticalFormDto>(
                        await _pharmaceuticalFormService.Add(_mapper.Map<PharmaceuticalForm>(pharmaceuticalForm))
                        );
                    return CreatedAtAction("Get", new { id = newPharmaceuticalForm.Id }, newPharmaceuticalForm);
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
                await _pharmaceuticalFormService.Delete(id);
                return NoContent();
            }
            catch (ApiException ex)
            {
                return StatusCode(ex.StatusCode, new { error = true, message = ex.Message });
            }
            
        }

    }
}
