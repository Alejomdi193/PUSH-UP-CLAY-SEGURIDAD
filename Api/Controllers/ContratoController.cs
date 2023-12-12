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

    public class ContratoController : BaseApiController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly  IMapper mapper;
        
        public ContratoController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ContratoDto>>> Get()
        {
            var data = await unitOfWork.Contratos.GetAllAsync();
            return mapper.Map<List<ContratoDto>>(data);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ContratoDto>> Get(int id)
        {
           var data = await unitOfWork.Contratos.GetByIdAsync(id);
           if (data == null){
                return NotFound();
            }
           return mapper.Map<ContratoDto>(data);
        }

        [HttpGet("ListarContratosActivos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<object>>> Get1()
        {
            var data = await unitOfWork.Contratos.ListarContratosActivos();
            return mapper.Map<List<object>>(data);
        }

        [HttpGet("Pagination")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Pager<ContratoDto>>> GetPagination([FromQuery] Params dataParams)
        {
           var datos = await unitOfWork.Contratos.GetAllAsync(dataParams.PageIndex, dataParams.PageSize, dataParams.Search);
           var listData = mapper.Map<List<ContratoDto>>(datos.registros);
            return new Pager<ContratoDto>(listData, datos.totalRegistros, dataParams.PageIndex, dataParams.PageSize, dataParams.Search);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ContratoDto>> Post(ContratoDto dataDto)
        {
            var data = mapper.Map<Contrato>(dataDto);
            unitOfWork.Contratos.Add(data);
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
        public async Task<ActionResult<ContratoDto>> Put(int id, [FromBody]ContratoDto dataDto)
        {
           if(dataDto== null)
           {
               return NotFound();
           }
           var data = mapper.Map<Contrato>(dataDto);
           unitOfWork.Contratos.Update(data);
           await unitOfWork.SaveAsync();
           return dataDto;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
           var data = await unitOfWork.Contratos.GetByIdAsync(id);
           if(data == null)
           {
                return NotFound();
           }
           unitOfWork.Contratos.Remove(data);
           await unitOfWork.SaveAsync();
           return NoContent();
        }
    }
}