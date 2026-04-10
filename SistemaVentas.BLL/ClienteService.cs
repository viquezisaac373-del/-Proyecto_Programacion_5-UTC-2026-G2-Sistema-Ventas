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
                Correo = c.Correo,
                Telefono = c.Telefono
            }).ToList();
        }

        public void InsertarCliente(ClienteDTO cliente)
        {
            ValidarCliente(cliente);
            ClienteDAO.InsertarCliente(cliente);
        }

        public void ActualizarCliente(ClienteDTO cliente)
        {
            ValidarCliente(cliente);
            ClienteDAO.ActualizarCliente(cliente);
        }

        public void EliminarCliente(int id)
        {
            if (id <= 0)
                throw new Exception("Regla de negocio: El ID del cliente no es válido.");

            ClienteDAO.EliminarCliente(id);
        }

        private void ValidarCliente(ClienteDTO cliente)
        {
            if (string.IsNullOrWhiteSpace(cliente.Nombre))
                throw new Exception("Regla de negocio: El nombre del cliente es obligatorio.");

            if (string.IsNullOrWhiteSpace(cliente.Telefono))
                throw new Exception("Regla de negocio: El teléfono del cliente es obligatorio.");

            if (string.IsNullOrWhiteSpace(cliente.Correo) || !cliente.Correo.Contains("@"))
                throw new Exception("Regla de negocio: El correo electrónico ingresado no es válido (debe contener '@').");
        }
    }
}

