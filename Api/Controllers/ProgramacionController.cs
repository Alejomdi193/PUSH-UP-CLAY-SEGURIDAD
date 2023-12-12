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

    public class ProgramacionController : BaseApiController
    {
       private readonly IUnitOfWork unitOfWork;
       private readonly  IMapper mapper;
       
       public ProgramacionController(IUnitOfWork unitOfWork, IMapper mapper)
       {
           this.unitOfWork = unitOfWork;
           this.mapper = mapper;
       }

       [HttpGet]
       [MapToApiVersion("1.0")]
       [ProducesResponseType(StatusCodes.Status200OK)]
       [ProducesResponseType(StatusCodes.Status400BadRequest)]
       public async Task<ActionResult<IEnumerable<ProgramacionDto>>> Get()
       {
           var data = await unitOfWork.Programaciones.GetAllAsync();
           return mapper.Map<List<ProgramacionDto>>(data);
       }

       [HttpGet("{id}")]
       [ProducesResponseType(StatusCodes.Status200OK)]
       [ProducesResponseType(StatusCodes.Status400BadRequest)]
       public async Task<ActionResult<ProgramacionDto>> Get(int id)
       {
          var data = await unitOfWork.Programaciones.GetByIdAsync(id);
          if (data == null){
            return NotFound();
        }
          return mapper.Map<ProgramacionDto>(data);
       }

       [HttpGet("Pagination")]
       [ProducesResponseType(StatusCodes.Status200OK)]
       [ProducesResponseType(StatusCodes.Status400BadRequest)]
       public async Task<ActionResult<Pager<ProgramacionDto>>> GetPagination([FromQuery] Params dataParams)
       {
          var datos = await unitOfWork.Programaciones.GetAllAsync(dataParams.PageIndex, dataParams.PageSize, dataParams.Search);
          var listData = mapper.Map<List<ProgramacionDto>>(datos.registros);
        return new Pager<ProgramacionDto>(listData, datos.totalRegistros, dataParams.PageIndex, dataParams.PageSize, dataParams.Search);
       }

       [HttpPost]
       [ProducesResponseType(StatusCodes.Status201Created)]
       [ProducesResponseType(StatusCodes.Status400BadRequest)]
       public async Task<ActionResult<ProgramacionDto>> Post(ProgramacionDto dataDto)
       {
        var data = mapper.Map<Programacion>(dataDto);
        unitOfWork.Programaciones.Add(data);
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
       public async Task<ActionResult<ProgramacionDto>> Put(int id, [FromBody]ProgramacionDto dataDto)
       {
          if(dataDto== null)
          {
              return NotFound();
          }
          var data = mapper.Map<Programacion>(dataDto);
          unitOfWork.Programaciones.Update(data);
          await unitOfWork.SaveAsync();
          return dataDto;
       }

       [HttpDelete("{id}")]
       [ProducesResponseType(StatusCodes.Status204NoContent)]
       [ProducesResponseType(StatusCodes.Status404NotFound)]
       public async Task<IActionResult> Delete(int id)
       {
          var data = await unitOfWork.Programaciones.GetByIdAsync(id);
          if(data == null)
          {
                return NotFound();
          }
          unitOfWork.Programaciones.Remove(data);
          await unitOfWork.SaveAsync();
          return NoContent();
       }
    }
}