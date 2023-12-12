using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Dtos;
using Api.Helpers.Errors;
using AutoMapper;
using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]

    public class RolController : BaseApiController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public RolController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<RolDto>>> Get()
        {
            var datos = await unitOfWork.Roles.GetAllAsync();
            return mapper.Map<List<RolDto>>(datos);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RolDto>> Get(int id)
        {
            var data = await unitOfWork.Roles.GetByIdAsync(id);
            if (data == null)
            {
                return NotFound();
            }
            return mapper.Map<RolDto>(data);
        }

        [HttpGet("Pagination")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Pager<RolDto>>> GetPagination([FromQuery] Params dataParams)
        {
            var datos = await unitOfWork.Roles.GetAllAsync(dataParams.PageIndex, dataParams.PageSize, dataParams.Search);
            var listData = mapper.Map<List<RolDto>>(datos.registros);
            return new Pager<RolDto>(listData, datos.totalRegistros, dataParams.PageIndex, dataParams.PageSize, dataParams.Search);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Rol>> Post(RolDto rolDto)
        {
            var data = mapper.Map<Rol>(rolDto);
            unitOfWork.Roles.Add(data);
            await unitOfWork.SaveAsync();
            if (data == null)
            {
                return BadRequest();
            }
            rolDto.Id = data.Id;
            return CreatedAtAction(nameof(Post), new { id = rolDto.Id }, rolDto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RolDto>> Put(int id, [FromBody] RolDto rolDto)
        {
            if (rolDto == null)
            {
                return NotFound();
            }
            var data = mapper.Map<Rol>(rolDto);
            unitOfWork.Roles.Update(data);
            await unitOfWork.SaveAsync();
            return rolDto;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await unitOfWork.Roles.GetByIdAsync(id);
            if (data == null)
            {
                return NotFound();
            }
            unitOfWork.Roles.Remove(data);
            await unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}