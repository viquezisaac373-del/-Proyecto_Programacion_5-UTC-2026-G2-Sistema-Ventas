using MySqlConnector;
using Sistema_Completo_De_Ventas;
using SistemaVentas.DTO;
using System;
using System.Collections.Generic;

namespace SistemaVentas.DAL
{
    // Clase estática que maneja el acceso a datos de la tabla "usuarios"
    public static class UsuarioDAO
    {
        // Método que valida las credenciales del usuario en la base de datos.
        // Retorna un objeto UsuarioDTO con sus datos si el login es exitoso, o null si no existe.
        public static UsuarioDTO? Login(string username, string password)
        {
            UsuarioDTO? usuario = null;
            Conexion conexionDB = new Conexion();
            try
            {
                // Se abre la conexión a la base de datos
                using (MySqlConnection conn = conexionDB.ObtenerConexion())
                {
                    // Se busca el usuario que coincida con las credenciales ingresadas,
                    // haciendo JOIN con la tabla roles para obtener el nombre del rol
                    string query = @"SELECT u.id, u.usuario, u.id_rol, r.nombre as nombre_rol 
                                     FROM usuarios u 
                                     INNER JOIN roles r ON u.id_rol = r.id 
                                     WHERE u.usuario = @user AND u.password = @pass";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Se usan parámetros para evitar inyección SQL
                        cmd.Parameters.AddWithValue("@user", username);
                        cmd.Parameters.AddWithValue("@pass", password);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Si se encuentra un registro, se mapean los datos al DTO
                            if (reader.Read())
                            {
                                usuario = new UsuarioDTO
                                {
                                    Id = reader.GetInt32("id"),
                                    Usuario = reader.GetString("usuario"),
                                    IdRol = reader.GetInt32("id_rol"),
                                    NombreRol = reader.GetString("nombre_rol")
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { throw new Exception("Error en BD: " + ex.Message); }
            return usuario;
        }

        // Método que obtiene la lista completa de usuarios registrados en el sistema.
        // Retorna una lista de objetos UsuarioDTO con los datos de cada usuario.
        public static List<UsuarioDTO> ObtenerTodos()
        {
            List<UsuarioDTO> lista = new List<UsuarioDTO>();
            Conexion conexionDB = new Conexion();
            try
            {
                using (MySqlConnection conn = conexionDB.ObtenerConexion())
                {
                    // Se consultan todos los usuarios junto con el nombre de su rol asignado
                    string query = @"SELECT u.id, u.usuario, u.id_rol, r.nombre as nombre_rol 
                                     FROM usuarios u 
                                     INNER JOIN roles r ON u.id_rol = r.id";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Se recorre cada fila del resultado y se agrega a la lista
                            while (reader.Read())
                            {
                                lista.Add(new UsuarioDTO
                                {
                                    Id = reader.GetInt32("id"),
                                    Usuario = reader.GetString("usuario"),
                                    NombreRol = reader.GetString("nombre_rol")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { throw new Exception("Error al listar: " + ex.Message); }
            return lista;
        }

        // Método que registra un nuevo usuario en la base de datos.
        // Recibe un UsuarioDTO con los datos a insertar (usuario, contraseña y rol).
        public static void Insertar(UsuarioDTO u)
        {
            Conexion conexionDB = new Conexion();
            using (MySqlConnection conn = conexionDB.ObtenerConexion())
            {
                // Se inserta el nuevo registro con los datos proporcionados
                string query = "INSERT INTO usuarios (usuario, password, id_rol) VALUES (@u, @p, @r)";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@u", u.Usuario);
                    cmd.Parameters.AddWithValue("@p", u.Password);
                    cmd.Parameters.AddWithValue("@r", u.IdRol);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Método que elimina un usuario de la base de datos según su ID.
        // Recibe el identificador único del usuario a eliminar.
        public static void Eliminar(int id)
        {
            Conexion conexionDB = new Conexion();
            using (MySqlConnection conn = conexionDB.ObtenerConexion())
            {
                // Se elimina el registro cuyo ID coincida con el recibido como parámetro
                string query = "DELETE FROM usuarios WHERE id = @id";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}