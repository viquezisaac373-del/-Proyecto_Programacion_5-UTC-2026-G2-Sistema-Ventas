using MySqlConnector;
using Sistema_Completo_De_Ventas;
using SistemaVentas.DTO;
using System;
using System.Collections.Generic;

public static class ClienteDAO
{
    public static void InsertarCliente(ClienteDTO cliente)
    {
        Conexion conexionDB = new Conexion();

        try
        {
            if (ExisteCorreo(cliente.Correo))
            {
                throw new Exception("El correo ya está registrado.");
            }

            using (MySqlConnection conn = conexionDB.ObtenerConexion())
            {
                string query = @"INSERT INTO clientes (nombre, correo, telefono)
                             VALUES (@nombre, @correo, @telefono)";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nombre", cliente.Nombre);
                    cmd.Parameters.AddWithValue("@correo", cliente.Correo);
                    cmd.Parameters.AddWithValue("@telefono", cliente.Telefono);

                    cmd.ExecuteNonQuery();
                }
            }

            Console.WriteLine("Cliente guardado en la base de datos correctamente.");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public static List<ClienteDTO> ObtenerClientes()
    {
        List<ClienteDTO> clientes = new List<ClienteDTO>();
        Conexion conexionDB = new Conexion();
        try
        {
            using (MySqlConnection conn = conexionDB.ObtenerConexion())
            {
                string query = "SELECT id, nombre, correo, telefono FROM clientes";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
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
        catch (Exception ex) { Console.WriteLine("Error al obtener clientes: " + ex.Message); }
        return clientes;
    }
    public static bool TieneVentas(int clienteId)
    {
        using (MySqlConnection conn = new Conexion().ObtenerConexion())
        {
            string query = "SELECT COUNT(*) FROM ventas WHERE cliente_id = @id";

            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@id", clienteId);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }
    }
    public static void ActualizarCliente(ClienteDTO cliente)
    {
        Conexion conexionDB = new Conexion();

        try
        {
            if (ExisteCorreo(cliente.Correo, cliente.Id))
            {
                throw new Exception("El correo ya está registrado.");
            }

            using (MySqlConnection conn = conexionDB.ObtenerConexion())
            {
                string query = @"UPDATE clientes 
                             SET nombre = @nombre, 
                                 correo = @correo, 
                                 telefono = @telefono
                             WHERE id = @id";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", cliente.Id);
                    cmd.Parameters.AddWithValue("@nombre", cliente.Nombre);
                    cmd.Parameters.AddWithValue("@correo", cliente.Correo);
                    cmd.Parameters.AddWithValue("@telefono", cliente.Telefono);

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
                    int filas = cmd.ExecuteNonQuery();
                    if (filas > 0) Console.WriteLine("Cliente eliminado correctamente.");
                    else Console.WriteLine("No se encontró un cliente con ese ID.");
                }
            }
        }
        catch (Exception ex) { Console.WriteLine("Error al eliminar cliente: " + ex.Message); }
    }
}