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

    public class CategoriaPersonaController : BaseApiController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly  IMapper mapper;
        
        public CategoriaPersonaController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<CategoriaPersona>>> Get()
        {
            var datos = await unitOfWork.CategoriaPersonas.GetAllAsync();
            return Ok(datos);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CategoriaPersonaDto>> Get(int id)
        {
           var data = await unitOfWork.CategoriaPersonas.GetByIdAsync(id);
           if (data == null){
                return NotFound();
            }
           return mapper.Map<CategoriaPersonaDto>(data);
        }
        [HttpGet("Pagination")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Pager<CategoriaPersonaDto>>> GetPagination([FromQuery] Params dataParams)
        {
           var datos = await unitOfWork.CategoriaPersonas.GetAllAsync(dataParams.PageIndex, dataParams.PageSize, dataParams.Search);
           var listData = mapper.Map<List<CategoriaPersonaDto>>(datos.registros);
            return new Pager<CategoriaPersonaDto>(listData, datos.totalRegistros, dataParams.PageIndex, dataParams.PageSize, dataParams.Search);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CategoriaPersonaDto>> Post(CategoriaPersonaDto dataDto)
        {
            var data = mapper.Map<CategoriaPersona>(dataDto);
            unitOfWork.CategoriaPersonas.Add(data);
            await unitOfWork.SaveAsync();
            if(data == null)
            {
            return BadRequest();
            }
            dataDto.Id = data.Id;
            return CreatedAtAction(nameof(Post), new {id =dataDto.Id}, dataDto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoriaPersonaDto>> Put(int id, [FromBody]CategoriaPersonaDto dataDto)
        {
           if(dataDto== null)
           {
               return NotFound();
           }
           var data = mapper.Map<CategoriaPersona>(dataDto);
           unitOfWork.CategoriaPersonas.Update(data);
           await unitOfWork.SaveAsync();
           return dataDto;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
           var data = await unitOfWork.CategoriaPersonas.GetByIdAsync(id);
           if(data == null)
           {
                return NotFound();
           }
           unitOfWork.CategoriaPersonas.Remove(data);
           await unitOfWork.SaveAsync();
           return NoContent();
        }
    }
}