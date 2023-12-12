using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Dtos;
using Api.Helpers.Errors;
using Api.Services;
using AutoMapper;
using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]

    public class UserController : BaseApiController
    {
        private readonly IUserService _userservice;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public UserController(IUserService userService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userservice = userService;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<UserDto>>> Get()
        {
            var datos = await unitOfWork.Users.GetAllAsync();
            return mapper.Map<List<UserDto>>(datos);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> Get(int id)
        {
            var data = await unitOfWork.Users.GetByIdAsync(id);
            if (data == null)
            {
                return NotFound();
            }
            return mapper.Map<UserDto>(data);
        }

        [HttpGet("Pagination")]
        [Authorize(Roles = "Administrador")]
        [MapToApiVersion("1.1")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Pager<UserDto>>> GetPagination([FromQuery] Params dataParams)
        {
            var datos = await unitOfWork.Users.GetAllAsync(dataParams.PageIndex, dataParams.PageSize, dataParams.Search);
            var listData = mapper.Map<List<UserDto>>(datos.registros);
            return new Pager<UserDto>(listData, datos.totalRegistros, dataParams.PageIndex, dataParams.PageSize, dataParams.Search);
        }

        [HttpPost("register")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> RegisterAsync(RegisterDto model)
        {
            var result = await _userservice.RegisterAsync(model);
            return Ok(result);
        }

        [HttpPost("token")]
        public async Task<IActionResult> GetTokenAsync(LoginDto model)
        {
            var result = await _userservice.GetTokenAsync(model);
            SetRefreshTokenInCookie(result.RefreshToken);
            return Ok(result);
        }

        [HttpPost("addrole")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> AddRoleAsync(AddRoleDto model)
        {
            var result = await _userservice.AddRoleAsync(model);
            return Ok(result);
        }

        [HttpPost("refresh-token")]
        [Authorize]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = await _userservice.RefreshTokenAsync(refreshToken);
            if (!string.IsNullOrEmpty(response.RefreshToken))
                SetRefreshTokenInCookie(response.RefreshToken);
            return Ok(response);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> Put(int id, [FromBody] UserDto userDto)
        {
            if (userDto == null)
            {
                return NotFound();
            }
            var data = mapper.Map<User>(userDto);
            unitOfWork.Users.Update(data);
            await unitOfWork.SaveAsync();
            return userDto;
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await unitOfWork.Users.GetByIdAsync(id);
            if (data == null)
            {
                return NotFound();
            }
            unitOfWork.Users.Remove(data);
            await unitOfWork.SaveAsync();
            return NoContent();
        }

        private void SetRefreshTokenInCookie(string refreshToken)
        {
            if (!string.IsNullOrEmpty(refreshToken))
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.UtcNow.AddDays(10),
                };
                Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
            }
            else
            {
                //
            }
        }
    }
}