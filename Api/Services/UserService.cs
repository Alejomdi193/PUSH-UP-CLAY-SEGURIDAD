using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Api.Dtos;
using Api.Helpers;
using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Api.Services
{
    public class UserService : IUserService
    {
        private readonly JWT _jwt;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher<User> _passwordHasher;
        public UserService(IUnitOfWork unitOfWork, IOptions<JWT> jwt, IPasswordHasher<User> passwordHasher)
        {
            _jwt = jwt.Value;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }
        
        public async Task<string> RegisterAsync(RegisterDto registerDto)
        {
            var user = new User
        {
            Nombre = registerDto.Nombre,
            Email = registerDto.Email
        };
        
        user.Password = _passwordHasher.HashPassword(user, registerDto.Password);
        
        var existingUser = _unitOfWork.Users
                                .Find(u => u.Nombre.ToLower() == registerDto.Nombre.ToLower())
                                .FirstOrDefault();
        
        if (existingUser == null)
        {
            var rolDefault = _unitOfWork.Roles
                                .Find(u => u.Nombre == Authorization.rol_default.ToString())
                                .First();
            try
                {
                    user.Rols.Add(rolDefault);
                    _unitOfWork.Users.Add(user);
                    await _unitOfWork.SaveAsync();
        
                    return $"User  {registerDto.Nombre} has been registered successfully";
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                    return $"Error: {message}";
                }
            }
            else
            {
                return $"User {registerDto.Nombre} already registered.";
            }
        }
        
        public async Task<DataUserDto> GetTokenAsync(LoginDto model)
        {
            DataUserDto dataUserDto = new DataUserDto();
            var user = await _unitOfWork.Users
                .GetByUsernameAsync(model.Nombre);
        
        if (user == null)
        {
            dataUserDto.IsAuthenticated = false;
            dataUserDto.Message = $"User does not exist with username {model.Nombre}.";
            return dataUserDto;
        }
        
        var result = _passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);
        
        if (result == PasswordVerificationResult.Success)
        {
            dataUserDto.IsAuthenticated = true;
            JwtSecurityToken jwtSecurityToken = CreateJwtToken(user);
            dataUserDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            dataUserDto.UserName = user.Nombre;
            dataUserDto.Roles = user.Rols
                            .Select(u => u.Nombre)
                            .ToList();
        
            if (user.RefreshTokens.Any(a => a.IsActive))
            {
                var activeRefreshToken = user.RefreshTokens.Where(a => a.IsActive == true).FirstOrDefault();
                dataUserDto.RefreshToken = activeRefreshToken.Token;
                dataUserDto.RefreshTokenExpiration = activeRefreshToken.Expires;
            }
            else
            {
                var refreshToken = CreateRefreshToken();
                dataUserDto.RefreshToken = refreshToken.Token;
                dataUserDto.RefreshTokenExpiration = refreshToken.Expires;
                user.RefreshTokens.Add(refreshToken);
                _unitOfWork.Users.Update(user);
                await _unitOfWork.SaveAsync();
            }
                return dataUserDto;
            }
            dataUserDto.IsAuthenticated = false;
            dataUserDto.Message = $"Credenciales incorrectas para el usuario {user.Nombre}.";
            return dataUserDto;
        }
        
        public async Task<string> AddRoleAsync(AddRoleDto model)
        {
        
        var user = await _unitOfWork.Users
                            .GetByUsernameAsync(model.Nombre);
        if (user == null)
        {
            return $"User {model.Nombre} does not exists.";
        }
        
        var result = _passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);
        
        if (result == PasswordVerificationResult.Success)
        {
            var rolExists = _unitOfWork.Roles
                                .Find(u => u.Nombre.ToLower() == model.Role.ToLower())
                                .FirstOrDefault();
        
            if (rolExists != null)
            {
                var userHasRole = user.Rols.Any(u => u.Id == rolExists.Id);
        
                if (userHasRole == false)
                {
                    user.Rols.Add(rolExists);
                    _unitOfWork.Users.Update(user);
                    await _unitOfWork.SaveAsync();
                }
                    return $"Role {model.Role} added to user {model.Nombre} successfully.";
                }
                    return $"Role {model.Role} was not found.";
                }
            return $"Invalid Credentials";
        }
        
        public async Task<DataUserDto> RefreshTokenAsync(string refreshToken)
        {
            var dataUserDto = new DataUserDto();
        
            var usuario = await _unitOfWork.Users
                                .GetByRefreshTokenAsync(refreshToken);
        
        if (usuario == null)
        {
            dataUserDto.IsAuthenticated = false;
            dataUserDto.Message = $"Token is not assigned to any user.";
            return dataUserDto;
        }
        
        var refreshTokenBd = usuario.RefreshTokens.Single(x => x.Token == refreshToken);
        
        if (!refreshTokenBd.IsActive)
        {
            dataUserDto.IsAuthenticated = false;
            dataUserDto.Message = $"Token is not active.";
            return dataUserDto;
        }
        refreshTokenBd.Revoked = DateTime.UtcNow;
        var newRefreshToken = CreateRefreshToken();
        usuario.RefreshTokens.Add(newRefreshToken);
        _unitOfWork.Users.Update(usuario);
        await _unitOfWork.SaveAsync();
        dataUserDto.IsAuthenticated = true;
        JwtSecurityToken jwtSecurityToken = CreateJwtToken(usuario);
        dataUserDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        /* dataUserDto.Email = usuario.Email; */
        dataUserDto.UserName = usuario.Nombre;
        dataUserDto.Email = usuario.Email;
        dataUserDto.Roles = usuario.Rols
                                .Select(u => u.Nombre)
                                .ToList();
            dataUserDto.RefreshToken = newRefreshToken.Token;
            dataUserDto.RefreshTokenExpiration = newRefreshToken.Expires;
            return dataUserDto;
        }
        
        private RefreshToken CreateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(randomNumber);
                return new RefreshToken
                    {
                        Token = Convert.ToBase64String(randomNumber),
                        Expires = DateTime.UtcNow.AddDays(10),
                        Created = DateTime.UtcNow
                    };
            }
        }
        
        private JwtSecurityToken CreateJwtToken(User user)
        {
            var roles = user.Rols;
            var roleClaims = new List<Claim>();
            foreach (var role in roles)
            {
                roleClaims.Add(new Claim("roles", role.Nombre));
            }
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Nombre),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id.ToString())
            }
            .Union(roleClaims);
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
            signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }
    }
}