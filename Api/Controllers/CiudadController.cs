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

    public class CiudadController : BaseApiController
    {
       private readonly IUnitOfWork unitOfWork;
       private readonly  IMapper mapper;
       
       public CiudadController(IUnitOfWork unitOfWork, IMapper mapper)
       {
           this.unitOfWork = unitOfWork;
           this.mapper = mapper;
       }

       [HttpGet]
       [MapToApiVersion("1.0")]
       [ProducesResponseType(StatusCodes.Status200OK)]
       [ProducesResponseType(StatusCodes.Status400BadRequest)]
       public async Task<ActionResult<IEnumerable<CiudadDto>>> Get()
       {
           var data = await unitOfWork.Ciudades.GetAllAsync();
           return mapper.Map<List<CiudadDto>>(data);
       }

       [HttpGet("{id}")]
       [ProducesResponseType(StatusCodes.Status200OK)]
       [ProducesResponseType(StatusCodes.Status400BadRequest)]
       public async Task<ActionResult<CiudadDto>> Get(int id)
       {
          var data = await unitOfWork.Ciudades.GetByIdAsync(id);
          if (data == null){
            return NotFound();
        }
          return mapper.Map<CiudadDto>(data);
       }

       [HttpGet("Pagination")]
       [ProducesResponseType(StatusCodes.Status200OK)]
       [ProducesResponseType(StatusCodes.Status400BadRequest)]
       public async Task<ActionResult<Pager<CiudadDto>>> GetPagination([FromQuery] Params dataParams)
       {
          var datos = await unitOfWork.Ciudades.GetAllAsync(dataParams.PageIndex, dataParams.PageSize, dataParams.Search);
          var listData = mapper.Map<List<CiudadDto>>(datos.registros);
        return new Pager<CiudadDto>(listData, datos.totalRegistros, dataParams.PageIndex, dataParams.PageSize, dataParams.Search);
       }

       [HttpPost]
       [ProducesResponseType(StatusCodes.Status201Created)]
       [ProducesResponseType(StatusCodes.Status400BadRequest)]
       public async Task<ActionResult<CiudadDto>> Post(CiudadDto dataDto)
       {
        var data = mapper.Map<Ciudad>(dataDto);
        unitOfWork.Ciudades.Add(data);
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
       public async Task<ActionResult<CiudadDto>> Put(int id, [FromBody]CiudadDto dataDto)
       {
          if(dataDto== null)
          {
              return NotFound();
          }
          var data = mapper.Map<Ciudad>(dataDto);
          unitOfWork.Ciudades.Update(data);
          await unitOfWork.SaveAsync();
          return dataDto;
       }

       [HttpDelete("{id}")]
       [ProducesResponseType(StatusCodes.Status204NoContent)]
       [ProducesResponseType(StatusCodes.Status404NotFound)]
       public async Task<IActionResult> Delete(int id)
       {
          var data = await unitOfWork.Ciudades.GetByIdAsync(id);
          if(data == null)
          {
                return NotFound();
          }
          unitOfWork.Ciudades.Remove(data);
          await unitOfWork.SaveAsync();
          return NoContent();
       }
    }
}