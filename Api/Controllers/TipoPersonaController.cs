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
    public class TipoPersonaController : BaseApiController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly  IMapper mapper;
        
        public TipoPersonaController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<TipoPersonaDto>>> Get()
        {
            var data = await unitOfWork.TipoPersonas.GetAllAsync();
            return mapper.Map<List<TipoPersonaDto>>(data);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TipoPersonaDto>> Get(int id)
        {
           var data = await unitOfWork.TipoPersonas.GetByIdAsync(id);
           if (data == null){
                return NotFound();
            }
           return mapper.Map<TipoPersonaDto>(data);
        }

        [HttpGet("Pagination")]
        [MapToApiVersion("1.1")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Pager<TipoPersonaDto>>> GetPagination([FromQuery] Params dataParams)
        {
           var datos = await unitOfWork.TipoPersonas.GetAllAsync(dataParams.PageIndex, dataParams.PageSize, dataParams.Search);
           var listData = mapper.Map<List<TipoPersonaDto>>(datos.registros);
            return new Pager<TipoPersonaDto>(listData, datos.totalRegistros, dataParams.PageIndex, dataParams.PageSize, dataParams.Search);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TipoPersonaDto>> Put(int id, [FromBody]TipoPersonaDto dataDto)
        {
           if(dataDto== null)
           {
               return NotFound();
           }
           var data = mapper.Map<TipoPersona>(dataDto);
           unitOfWork.TipoPersonas.Update(data);
           await unitOfWork.SaveAsync();
           return dataDto;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TipoPersonaDto>> Post(TipoPersonaDto dataDto)
        {
            var data = mapper.Map<TipoPersona>(dataDto);
            unitOfWork.TipoPersonas.Add(data);
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
           var data = await unitOfWork.TipoPersonas.GetByIdAsync(id);
           if(data == null)
           {
                return NotFound();
           }
           unitOfWork.TipoPersonas.Remove(data);
           await unitOfWork.SaveAsync();
           return NoContent();
        }
    }
}