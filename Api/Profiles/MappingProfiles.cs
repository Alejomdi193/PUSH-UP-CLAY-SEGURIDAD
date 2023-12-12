using Api.Dtos;
using AutoMapper;
using Dominio.Entities;

namespace Api.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Rol, RolDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<CategoriaPersona, CategoriaPersonaDto>().ReverseMap();
            CreateMap<ContactoPersona, ContactoPersonaDto>().ReverseMap();
            CreateMap<Ciudad, CiudadDto>().ReverseMap();
            CreateMap<Contrato, ContratoDto>().ReverseMap();
            CreateMap<Departamento, DepartamentoDto>().ReverseMap();
            CreateMap<DirPersona, DirPersonaDto>().ReverseMap();
            CreateMap<Estado, EstadoDto>().ReverseMap();
            CreateMap<Pais, PaisDto>().ReverseMap();
            CreateMap<Persona, PersonaDto>().ReverseMap();
            CreateMap<Programacion, ProgramacionDto>().ReverseMap();
            CreateMap<TipoContacto, TipoContactoDto>().ReverseMap();
            CreateMap<TipoDireccion, TipoDireccionDto>().ReverseMap();
            CreateMap<TipoPersona, TipoPersonaDto>().ReverseMap();
            CreateMap<Turno, TurnoDto>().ReverseMap();

        }
    }
}