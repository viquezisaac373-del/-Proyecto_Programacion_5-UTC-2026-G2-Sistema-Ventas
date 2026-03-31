using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SistemaVentas.DTO;

namespace SistemaVentas.BLL
{
    public class ClienteService
    {
        public List<ClienteDTO> ObtenerClientes()
        {
            var clientes = ClienteDAO.ObtenerClientes();

            return clientes.Select(c => new ClienteDTO
            {
                Id = c.Id,
                Nombre = c.Nombre,
                // agrega las demás propiedades
            }).ToList();
        }
    }
}

