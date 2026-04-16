using MySqlConnector;
using Sistema_Completo_De_Ventas;
using SistemaVentas.DTO;
using System;
using System.Collections.Generic;

// Clase DAO encargada de manejar las operaciones de base de datos para clientes
public static class ClienteDAO
{
    // Inserta un nuevo cliente en la base de datos
    public static void InsertarCliente(ClienteDTO cliente)
    {
        Conexion conexionDB = new Conexion();

        try
        {
            // Verifica si el correo ya existe
            if (ExisteCorreo(cliente.Correo))
            {
                throw new Exception("El correo ya está registrado.");
            }

            // Abre la conexión a la base de datos
            using (MySqlConnection conn = conexionDB.ObtenerConexion())
            {
                // Consulta SQL para insertar cliente
                string query = @"INSERT INTO clientes (nombre, correo, telefono)
                             VALUES (@nombre, @correo, @telefono)";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    // Parámetros para evitar inyección SQL
                    cmd.Parameters.AddWithValue("@nombre", cliente.Nombre);
                    cmd.Parameters.AddWithValue("@correo", cliente.Correo);
                    cmd.Parameters.AddWithValue("@telefono", cliente.Telefono);

                    // Ejecuta la inserción
                    cmd.ExecuteNonQuery();
                }
            }

            Console.WriteLine("Cliente guardado en la base de datos correctamente.");
        }
        catch (Exception ex)
        {
            // Lanza el error
            throw new Exception(ex.Message);
        }
    }

    // Obtiene la lista de clientes desde la base de datos
    public static List<ClienteDTO> ObtenerClientes()
    {
        List<ClienteDTO> clientes = new List<ClienteDTO>();
        Conexion conexionDB = new Conexion();

        try
        {
            using (MySqlConnection conn = conexionDB.ObtenerConexion())
            {
                // Consulta SQL para obtener clientes
                string query = "SELECT id, nombre, correo, telefono FROM clientes";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    // Lee cada registro y lo convierte en objeto ClienteDTO
                    while (reader.Read())
                    {
                        ClienteDTO cliente = new ClienteDTO
                        {
                            Id = reader.GetInt32("id"),
                            Nombre = reader.GetString("nombre"),
                            Correo = reader.GetString("correo"),
                            Telefono = reader.GetString("telefono")
                        };

                        clientes.Add(cliente);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al obtener clientes: " + ex.Message);
        }

        return clientes;
    }

    // Verifica si un cliente tiene ventas asociadas
    public static bool TieneVentas(int clienteId)
    {
        using (MySqlConnection conn = new Conexion().ObtenerConexion())
        {
            string query = "SELECT COUNT(*) FROM ventas WHERE cliente_id = @id";

            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@id", clienteId);

                // Ejecuta la consulta y convierte el resultado a entero
                int count = Convert.ToInt32(cmd.ExecuteScalar());

                // Retorna true si tiene ventas
                return count > 0;
            }
        }
    }

    // Actualiza los datos de un cliente
    public static void ActualizarCliente(ClienteDTO cliente)
    {
        Conexion conexionDB = new Conexion();

        try
        {
            // Verifica si el correo ya existe en otro cliente
            if (ExisteCorreo(cliente.Correo, cliente.Id))
            {
                throw new Exception("El correo ya está registrado.");
            }

            using (MySqlConnection conn = conexionDB.ObtenerConexion())
            {
                // Consulta SQL para actualizar cliente
                string query = @"UPDATE clientes 
                             SET nombre = @nombre, 
                                 correo = @correo, 
                                 telefono = @telefono
                             WHERE id = @id";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    // Parámetros
                    cmd.Parameters.AddWithValue("@id", cliente.Id);
                    cmd.Parameters.AddWithValue("@nombre", cliente.Nombre);
                    cmd.Parameters.AddWithValue("@correo", cliente.Correo);
                    cmd.Parameters.AddWithValue("@telefono", cliente.Telefono);

                    // Ejecuta actualización
                    cmd.ExecuteNonQuery();
                }
            }

            Console.WriteLine("Cliente actualizado correctamente.");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    // Verifica si un correo ya existe en la base de datos
    public static bool ExisteCorreo(string correo, int idExcluir = 0)
    {
        bool existe = false;

        using (var conn = new Conexion().ObtenerConexion())
        {
            string query = @"SELECT COUNT(*) 
                         FROM clientes 
                         WHERE correo = @correo 
                         AND id != @id";

            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@correo", correo);
                cmd.Parameters.AddWithValue("@id", idExcluir);

                int count = Convert.ToInt32(cmd.ExecuteScalar());

                if (count > 0)
                    existe = true;
            }
        }

        return existe;
    }

    // Elimina un cliente por su ID
    public static void EliminarCliente(int id)
    {
        Conexion conexionDB = new Conexion();

        try
        {
            using (MySqlConnection conn = conexionDB.ObtenerConexion())
            {
                string query = "DELETE FROM clientes WHERE id = @id";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    // Ejecuta la eliminación
                    int filas = cmd.ExecuteNonQuery();

                    // Verifica si se eliminó correctamente
                    if (filas > 0)
                        Console.WriteLine("Cliente eliminado correctamente.");
                    else
                        Console.WriteLine("No se encontró un cliente con ese ID.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al eliminar cliente: " + ex.Message);
        }
    }
}