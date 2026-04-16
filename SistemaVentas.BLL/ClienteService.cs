using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SistemaVentas.DTO;

namespace SistemaVentas.BLL
{
    // Clase que contiene la lógica de negocio relacionada con los clientes
    public class ClienteService
    {
        // Obtiene la lista de clientes desde la capa de datos (DAO)
        public List<ClienteDTO> ObtenerClientes()
        {
            var clientes = ClienteDAO.ObtenerClientes();

            // Convierte los datos obtenidos en objetos ClienteDTO
            return clientes.Select(c => new ClienteDTO
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Correo = c.Correo,
                Telefono = c.Telefono
            }).ToList();
        }

        // Inserta un nuevo cliente en la base de datos
        public void InsertarCliente(ClienteDTO cliente)
        {
            // Valida los datos antes de insertar
            ValidarCliente(cliente);

            ClienteDAO.InsertarCliente(cliente);
        }

        // Actualiza la información de un cliente existente
        public void ActualizarCliente(ClienteDTO cliente)
        {
            // Valida los datos antes de actualizar
            ValidarCliente(cliente);

            ClienteDAO.ActualizarCliente(cliente);
        }

        // Elimina un cliente por su ID
        public void EliminarCliente(int id)
        {
            // Verifica si el cliente tiene ventas asociadas
            if (ClienteDAO.TieneVentas(id))
            {
                throw new Exception("No se puede eliminar el cliente porque tiene ventas registradas.");
            }

            // Valida que el ID sea correcto
            if (id <= 0)
            {
                throw new Exception("Regla de negocio: El ID del cliente no es válido.");
            }

            // Elimina el cliente
            ClienteDAO.EliminarCliente(id);
        }

        // Verifica si un cliente existe en la lista
        public bool ExisteCliente(int id)
        {
            var clientes = ObtenerClientes();

            // Busca si existe un cliente con el ID indicado
            return clientes.Any(c => c.Id == id);
        }

        // Método privado para validar los datos del cliente
        private void ValidarCliente(ClienteDTO cliente)
        {
            // Valida que el nombre no esté vacío
            if (string.IsNullOrWhiteSpace(cliente.Nombre))
                throw new Exception("Regla de negocio: El nombre del cliente es obligatorio.");

            // Valida que el teléfono no esté vacío
            if (string.IsNullOrWhiteSpace(cliente.Telefono))
                throw new Exception("Regla de negocio: El teléfono del cliente es obligatorio.");

            // Valida que el correo tenga un formato básico válido
            if (string.IsNullOrWhiteSpace(cliente.Correo) || !cliente.Correo.Contains("@"))
                throw new Exception("Regla de negocio: El correo electrónico ingresado no es válido (debe contener '@').");
        }
    }
}

