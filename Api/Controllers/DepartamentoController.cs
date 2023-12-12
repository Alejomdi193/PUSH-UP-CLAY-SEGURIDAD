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
   
    public class DepartamentoController :BaseApiController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly  IMapper mapper;
        
        public DepartamentoController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<DepartamentoDto>>> Get()
        {
            var data = await unitOfWork.Departamentos.GetAllAsync();
            return mapper.Map<List<DepartamentoDto>>(data);
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DepartamentoDto>> Get(int id)
        {
           var data = await unitOfWork.Departamentos.GetByIdAsync(id);
           if (data == null){
                return NotFound();
            }
           return mapper.Map<DepartamentoDto>(data);
        }

        [HttpGet("Pagination")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Pager<DepartamentoDto>>> GetPagination([FromQuery] Params dataParams)
        {
           var datos = await unitOfWork.Departamentos.GetAllAsync(dataParams.PageIndex, dataParams.PageSize, dataParams.Search);
           var listData = mapper.Map<List<DepartamentoDto>>(datos.registros);
            return new Pager<DepartamentoDto>(listData, datos.totalRegistros, dataParams.PageIndex, dataParams.PageSize, dataParams.Search);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DepartamentoDto>> Put(int id, [FromBody]DepartamentoDto dataDto)
        {
           if(dataDto== null)
           {
               return NotFound();
           }
           var data = mapper.Map<Departamento>(dataDto);
           unitOfWork.Departamentos.Update(data);
           await unitOfWork.SaveAsync();
           return dataDto;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DepartamentoDto>> Post(DepartamentoDto dataDto)
        {
            var data = mapper.Map<Departamento>(dataDto);
            unitOfWork.Departamentos.Add(data);
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
           var data = await unitOfWork.Departamentos.GetByIdAsync(id);
           if(data == null)
           {
                return NotFound();
           }
           unitOfWork.Departamentos.Remove(data);
           await unitOfWork.SaveAsync();
           return NoContent();
        }
    }
}