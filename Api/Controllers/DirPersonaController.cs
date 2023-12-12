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
 
    public class DirPersonaController : BaseApiController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly  IMapper mapper;
        
        public DirPersonaController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<DirPersonaDto>>> Get()
        {
            var data = await unitOfWork.DirPersonas.GetAllAsync();
            return mapper.Map<List<DirPersonaDto>>(data);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DirPersonaDto>> Get(int id)
        {
           var data = await unitOfWork.DirPersonas.GetByIdAsync(id);
           if (data == null){
                return NotFound();
            }
           return mapper.Map<DirPersonaDto>(data);
        }

        [HttpGet("Pagination")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Pager<DirPersonaDto>>> GetPagination([FromQuery] Params dataParams)
        {
           var datos = await unitOfWork.DirPersonas.GetAllAsync(dataParams.PageIndex, dataParams.PageSize, dataParams.Search);
           var listData = mapper.Map<List<DirPersonaDto>>(datos.registros);
            return new Pager<DirPersonaDto>(listData, datos.totalRegistros, dataParams.PageIndex, dataParams.PageSize, dataParams.Search);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DirPersonaDto>> Post(DirPersonaDto dataDto)
        {
            var data = mapper.Map<DirPersona>(dataDto);
            unitOfWork.DirPersonas.Add(data);
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
        public async Task<ActionResult<DirPersonaDto>> Put(int id, [FromBody]DirPersonaDto dataDto)
        {
           if(dataDto== null)
           {
               return NotFound();
           }
           var data = mapper.Map<DirPersona>(dataDto);
           unitOfWork.DirPersonas.Update(data);
           await unitOfWork.SaveAsync();
           return dataDto;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
           var data = await unitOfWork.DirPersonas.GetByIdAsync(id);
           if(data == null)
           {
                return NotFound();
           }
           unitOfWork.DirPersonas.Remove(data);
           await unitOfWork.SaveAsync();
           return NoContent();
        }
    }
}