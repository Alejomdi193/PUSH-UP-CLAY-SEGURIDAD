using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Api.Dtos;
using Api.Helpers.Errors;
using AutoMapper;
using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
  
    public class ContactoPersonaController : BaseApiController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly  IMapper mapper;
        
        public ContactoPersonaController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ContactoPersonaDto>>> Get()
        {
            var data = await unitOfWork.ContactoPersonas.GetAllAsync();
            return mapper.Map<List<ContactoPersonaDto>>(data);
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ContactoPersonaDto>> Get(int id)
        {
           var data = await unitOfWork.ContactoPersonas.GetByIdAsync(id);
           if (data == null){
                return NotFound();
            }
           return mapper.Map<ContactoPersonaDto>(data);
        }

        [HttpGet("Pagination")]
        [MapToApiVersion("1.1")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Pager<ContactoPersonaDto>>> GetPagination([FromQuery] Params dataParams)
        {
           var datos = await unitOfWork.ContactoPersonas.GetAllAsync(dataParams.PageIndex, dataParams.PageSize, dataParams.Search);
           var listData = mapper.Map<List<ContactoPersonaDto>>(datos.registros);
            return new Pager<ContactoPersonaDto>(listData, datos.totalRegistros, dataParams.PageIndex, dataParams.PageSize, dataParams.Search);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ContactoPersonaDto>> Put(int id, [FromBody]ContactoPersonaDto dataDto)
        {
           if(dataDto== null)
           {
               return NotFound();
           }
           var data = mapper.Map<ContactoPersona>(dataDto);
           unitOfWork.ContactoPersonas.Update(data);
           await unitOfWork.SaveAsync();
           return dataDto;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ContactoPersonaDto>> Post(ContactoPersonaDto dataDto)
        {
            var data = mapper.Map<ContactoPersona>(dataDto);
            unitOfWork.ContactoPersonas.Add(data);
            await unitOfWork.SaveAsync();
            if(data == null)
            {
            return BadRequest();
            }
            dataDto.Id = data.Id;
            return CreatedAtAction(nameof(Post), new {id =dataDto.Id}, dataDto);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
           var data = await unitOfWork.ContactoPersonas.GetByIdAsync(id);
           if(data == null)
           {
                return NotFound();
           }
           unitOfWork.ContactoPersonas.Remove(data);
           await unitOfWork.SaveAsync();
           return NoContent();
        }
    }
}