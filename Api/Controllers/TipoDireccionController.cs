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
  
    public class TipoDireccionController : BaseApiController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly  IMapper mapper;
        
        public TipoDireccionController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<TipoDireccionDto>>> Get()
        {
            var data = await unitOfWork.TipoDirecciones.GetAllAsync();
            return mapper.Map<List<TipoDireccionDto>>(data);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TipoDireccionDto>> Get(int id)
        {
           var data = await unitOfWork.TipoDirecciones.GetByIdAsync(id);
           if (data == null){
                return NotFound();
            }
           return mapper.Map<TipoDireccionDto>(data);
        }

        [HttpGet("Pagination")]
        [MapToApiVersion("1.1")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Pager<TipoDireccionDto>>> GetPagination([FromQuery] Params dataParams)
        {
           var datos = await unitOfWork.TipoDirecciones.GetAllAsync(dataParams.PageIndex, dataParams.PageSize, dataParams.Search);
           var listData = mapper.Map<List<TipoDireccionDto>>(datos.registros);
            return new Pager<TipoDireccionDto>(listData, datos.totalRegistros, dataParams.PageIndex, dataParams.PageSize, dataParams.Search);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TipoDireccionDto>> Post(TipoDireccionDto dataDto)
        {
            var data = mapper.Map<TipoDireccion>(dataDto);
            unitOfWork.TipoDirecciones.Add(data);
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
        public async Task<ActionResult<TipoDireccionDto>> Put(int id, [FromBody]TipoDireccionDto dataDto)
        {
           if(dataDto== null)
           {
               return NotFound();
           }
           var data = mapper.Map<TipoDireccion>(dataDto);
           unitOfWork.TipoDirecciones.Update(data);
           await unitOfWork.SaveAsync();
           return dataDto;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
           var data = await unitOfWork.TipoDirecciones.GetByIdAsync(id);
           if(data == null)
           {
                return NotFound();
           }
           unitOfWork.TipoDirecciones.Remove(data);
           await unitOfWork.SaveAsync();
           return NoContent();
        }
    }
}