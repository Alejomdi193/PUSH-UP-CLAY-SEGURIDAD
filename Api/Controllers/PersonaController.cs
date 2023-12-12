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

    public class PersonaController : BaseApiController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public PersonaController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<PersonaDto>>> Get()
        {
            var data = await unitOfWork.Personas.GetAllAsync();
            return mapper.Map<List<PersonaDto>>(data);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PersonaDto>> Get(int id)
        {
            var data = await unitOfWork.Personas.GetByIdAsync(id);
            if (data == null)
            {
                return NotFound();
            }
            return mapper.Map<PersonaDto>(data);
        }

        [HttpGet("ListarEmpleados")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<object>>> Get1()
        {
            var data = await unitOfWork.Personas.ListarEmpleados();
            return mapper.Map<List<object>>(data);
        }




        [HttpGet("ListarVigilante")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<object>>> Get2()
        {
            var data = await unitOfWork.Personas.ListarVigilantes();
            return mapper.Map<List<object>>(data);
        }


        [HttpGet("ListarNumerosDeContactoVigilantes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<object>>> Get3()
        {
            var data = await unitOfWork.Personas.ListarNumerosDeContactoVigilantes();
            return mapper.Map<List<object>>(data);
        }


        [HttpGet("ListarClientesEnBucaramanga")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<object>>> Get4()
        {
            var data = await unitOfWork.Personas.ListarClientesEnBucaramanga();
            return mapper.Map<List<object>>(data);
        }



        [HttpGet("ListarEmpleadosEnPiedecuestaOYGiron")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<object>>> Get5()
        {
            var data = await unitOfWork.Personas.ListarEmpleadosEnPiedecuestaOYGiron();
            return mapper.Map<List<object>>(data);
        }



        [HttpGet("ListarClientesConMasDe5AnosAntiguedad")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<object>>> Get6()
        {
            var data = await unitOfWork.Personas.ListarClientesConMasDe5AnosAntiguedad();
            return mapper.Map<List<object>>(data);
        }

        [HttpGet("Pagination")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Pager<PersonaDto>>> GetPagination([FromQuery] Params dataParams)
        {
            var datos = await unitOfWork.Personas.GetAllAsync(dataParams.PageIndex, dataParams.PageSize, dataParams.Search);
            var listData = mapper.Map<List<PersonaDto>>(datos.registros);
            return new Pager<PersonaDto>(listData, datos.totalRegistros, dataParams.PageIndex, dataParams.PageSize, dataParams.Search);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PersonaDto>> Post(PersonaDto dataDto)
        {
            var data = mapper.Map<Persona>(dataDto);
            unitOfWork.Personas.Add(data);
            await unitOfWork.SaveAsync();
            if (data == null)
            {
                return BadRequest();
            }
            dataDto.Id = data.Id;
            return CreatedAtAction(nameof(Post), new { id = dataDto.Id }, dataDto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PersonaDto>> Put(int id, [FromBody] PersonaDto dataDto)
        {
            if (dataDto == null)
            {
                return NotFound();
            }
            var data = mapper.Map<Persona>(dataDto);
            unitOfWork.Personas.Update(data);
            await unitOfWork.SaveAsync();
            return dataDto;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await unitOfWork.Personas.GetByIdAsync(id);
            if (data == null)
            {
                return NotFound();
            }
            unitOfWork.Personas.Remove(data);
            await unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}