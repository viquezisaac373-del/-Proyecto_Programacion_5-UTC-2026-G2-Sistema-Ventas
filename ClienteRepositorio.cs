using MySqlConnector;
using Sistema_Completo_De_Ventas;
using System;
using System.Collections.Generic;

// Esta clase se encarga de realizar las operaciones CRUD
// de la entidad Cliente directamente en la base de datos.
public static class ClienteRepositorio
{
    // Método para insertar un cliente en la base de datos
    public static void InsertarCliente(Cliente cliente)
    {
        // Creamos una instancia de la clase Conexion
        Conexion conexionDB = new Conexion();

        try
        {
            // using abre la conexión y la cierra automáticamente
            using (MySqlConnection conn = conexionDB.ObtenerConexion())
            {
                // Consulta SQL para insertar un cliente
                string query = @"INSERT INTO clientes (id, nombre, correo, telefono)
                                 VALUES (@id, @nombre, @correo, @telefono)";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    // Se agregan los parámetros para evitar SQL Injection
                    cmd.Parameters.AddWithValue("@id", cliente.Id);
                    cmd.Parameters.AddWithValue("@nombre", cliente.Nombre);
                    cmd.Parameters.AddWithValue("@correo", cliente.Correo);
                    cmd.Parameters.AddWithValue("@telefono", cliente.Telefono);

                    // Ejecuta el INSERT en la base de datos
                    cmd.ExecuteNonQuery();
                }
            }

            Console.WriteLine("Cliente guardado en la base de datos correctamente.");
        }
        catch (Exception ex)
        {
            // En caso de error se muestra el mensaje
            Console.WriteLine("Error al insertar cliente en la base de datos: " + ex.Message);
        }
    }

    // Método para obtener todos los clientes desde la base de datos
    public static List<Cliente> ObtenerClientes()
    {
        // Lista donde se almacenarán los clientes obtenidos
        List<Cliente> clientes = new List<Cliente>();

        Conexion conexionDB = new Conexion();

        try
        {
            using (MySqlConnection conn = conexionDB.ObtenerConexion())
            {
                // Consulta SQL para traer todos los clientes
                string query = "SELECT id, nombre, correo, telefono FROM clientes";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Mientras existan registros en el resultado
                        while (reader.Read())
                        {
                            // Se crea un objeto Cliente con los datos de la BD
                            Cliente cliente = new Cliente(
                                reader.GetInt32("id"),
                                reader.GetString("nombre"),
                                reader.GetString("correo"),
                                reader.GetString("telefono")
                            );

                            // Se agrega a la lista
                            clientes.Add(cliente);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al obtener clientes: " + ex.Message);
        }

        // Retorna la lista obtenida
        return clientes;
    }

    // Actualiza los datos de un cliente en la base de datos
    public static void ActualizarCliente(Cliente cliente)
    {
        Conexion conexionDB = new Conexion();

        try
        {
            using (MySqlConnection conn = conexionDB.ObtenerConexion())
            {
                string query = @"UPDATE clientes 
                             SET nombre = @nombre, correo = @correo, telefono = @telefono
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

            Console.WriteLine("Cliente actualizado correctamente en la base de datos.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al actualizar cliente: " + ex.Message);
        }
    }

    // Elimina un cliente de la base de datos según su ID
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