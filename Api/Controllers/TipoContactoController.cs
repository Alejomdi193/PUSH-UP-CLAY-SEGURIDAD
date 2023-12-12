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

    public class TipoContactoController : BaseApiController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly  IMapper mapper;
        
        public TipoContactoController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<TipoContactoDto>>> Get()
        {
            var data = await unitOfWork.TipoContactos.GetAllAsync();
            return mapper.Map<List<TipoContactoDto>>(data);
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TipoContactoDto>> Get(int id)
        {
           var data = await unitOfWork.TipoContactos.GetByIdAsync(id);
           if (data == null){
                return NotFound();
            }
           return mapper.Map<TipoContactoDto>(data);
        }

        [HttpGet("Pagination")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Pager<TipoContactoDto>>> GetPagination([FromQuery] Params dataParams)
        {
           var datos = await unitOfWork.TipoContactos.GetAllAsync(dataParams.PageIndex, dataParams.PageSize, dataParams.Search);
           var listData = mapper.Map<List<TipoContactoDto>>(datos.registros);
            return new Pager<TipoContactoDto>(listData, datos.totalRegistros, dataParams.PageIndex, dataParams.PageSize, dataParams.Search);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TipoContactoDto>> Put(int id, [FromBody]TipoContactoDto dataDto)
        {
           if(dataDto== null)
           {
               return NotFound();
           }
           var data = mapper.Map<TipoContacto>(dataDto);
           unitOfWork.TipoContactos.Update(data);
           await unitOfWork.SaveAsync();
           return dataDto;
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TipoContactoDto>> Post(TipoContactoDto dataDto)
        {
            var data = mapper.Map<TipoContacto>(dataDto);
            unitOfWork.TipoContactos.Add(data);
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
           var data = await unitOfWork.TipoContactos.GetByIdAsync(id);
           if(data == null)
           {
                return NotFound();
           }
           unitOfWork.TipoContactos.Remove(data);
           await unitOfWork.SaveAsync();
           return NoContent();
        }
    }
}