using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio.Entities;

namespace Dominio.Interfaces
{
    public interface IPersona : IGeneric<Persona>
    {
        Task<IEnumerable<object>> ListarEmpleados();
        Task<IEnumerable<object>> ListarVigilantes();
        Task<IEnumerable<object>> ListarNumerosDeContactoVigilantes();
        Task<IEnumerable<object>> ListarClientesEnBucaramanga();
        Task<IEnumerable<object>> ListarEmpleadosEnPiedecuestaOYGiron();
        Task<IEnumerable<object>> ListarClientesConMasDe5AnosAntiguedad();
    }
}