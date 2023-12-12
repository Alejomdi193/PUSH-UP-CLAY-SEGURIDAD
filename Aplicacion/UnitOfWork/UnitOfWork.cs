using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion.Repository;
using Dominio.Entities;
using Dominio.Interfaces;
using Persistencia;

namespace Aplicacion.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly PushContext context;
        private UserRepository _usuarios;
        private RolRepository _roles;
        private CategoriaPersonaRepository _categoriaPersonas;
        private CiudadRepository ciudad;
        private ContratoRepository _contrato;
        private DepartamentoRepository _departamento;
        private DirPersonaRepository _dirPersona;
        private EstadoRepository _estado;
        private PaisRepository _pais;
        private PersonaRepository _persona;
        private ProgramacionRepository _programacion;
        private TipoContactoRepository _tipoContacto;
        private TipoDireccionRepository _tipoDireccion;
        private TipoPersonaRepository _tipoPersona;
        private TurnoRepository _turno;
        private ContactoPersonaRepository _contactoPersona;


        public UnitOfWork(PushContext _context)
        {
            context = _context;
        }

        public IUser Users
        {
            get
            {
                if (_usuarios == null)
                {
                    _usuarios = new UserRepository(context);
                }
                return _usuarios;
            }
        }

        public IRol Roles
        {
            get
            {
                if (_roles == null)
                {
                    _roles = new RolRepository(context);
                }
                return _roles;
            }
        }
 
        public ICategoriaPersona CategoriaPersonas
        {
            get
            {
                if (_categoriaPersonas == null)
                {
                    _categoriaPersonas = new CategoriaPersonaRepository(context);
                }
                return _categoriaPersonas;
            }
        }

        public ICiudad Ciudades
        {
            get
            {
                if (ciudad == null)
                {
                    ciudad = new CiudadRepository(context);
                }
                return ciudad;
            }
        }

        public IContrato Contratos
        {
            get
            {
                if (_contrato == null)
                {
                    _contrato = new ContratoRepository(context);
                }
                return _contrato;
            }
        }

        public IDepartamento Departamentos
        {
            get
            {
                if (_departamento == null)
                {
                    _departamento = new DepartamentoRepository(context);
                }
                return _departamento;
            }
        }

        public IDirPersona DirPersonas
        {
            get
            {
                if (_dirPersona == null)
                {
                    _dirPersona = new DirPersonaRepository(context);
                }
                return _dirPersona;
            }
        }

        public IEstado Estados
        {
            get
            {
                if (_estado == null)
                {
                    _estado = new EstadoRepository(context);
                }
                return _estado;
            }
        }

        public IPais Paises
        {
            get
            {
                if (_pais == null)
                {
                    _pais = new PaisRepository(context);
                }
                return _pais;
            }
        }

        public IPersona Personas
        {
            get
            {
                if (_persona == null)
                {
                    _persona = new PersonaRepository(context);
                }
                return _persona;
            }
        }

        public IProgramacion Programaciones
        {
            get
            {
                if (_programacion == null)
                {
                    _programacion = new ProgramacionRepository(context);
                }
                return _programacion;
            }
        }

        public ITipoContacto TipoContactos
        {
            get
            {
                if (_tipoContacto == null)
                {
                    _tipoContacto = new TipoContactoRepository(context);
                }
                return _tipoContacto;
            }
        }

        public ITipoDireccion TipoDirecciones
        {
            get
            {
                if (_tipoDireccion == null)
                {
                    _tipoDireccion = new TipoDireccionRepository(context);
                }
                return _tipoDireccion;
            }
        }

        public ITipoPersona TipoPersonas
        {
            get
            {
                if (_tipoPersona == null)
                {
                    _tipoPersona = new TipoPersonaRepository(context);
                }
                return _tipoPersona;
            }
        }

        public ITurnos Turnos
        {
            get
            {
                if (_turno == null)
                {
                    _turno = new TurnoRepository(context);
                }
                return _turno;
            }
        }

        public IContactoPersona ContactoPersonas
        {
            get
            {
                if (_contactoPersona == null)
                {
                    _contactoPersona = new ContactoPersonaRepository(context);
                }
                return _contactoPersona;
            }
        }

       
        public void Dispose()
        {
            if (context != null)
            {
                context.Dispose();
            }
        }

        public async Task<int> SaveAsync()
        {
            return await context.SaveChangesAsync();
        }

        Task<int> IUnitOfWork.SaveAsync()
        {
            throw new NotImplementedException();
        }
    }
}