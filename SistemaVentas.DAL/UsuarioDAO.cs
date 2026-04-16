using MySqlConnector;
using Sistema_Completo_De_Ventas;
using SistemaVentas.DTO;
using System;
using System.Collections.Generic;

namespace SistemaVentas.DAL
{
    public static class UsuarioDAO
    {
        // 1. EL LOGIN DE TU COMPAÑERO (Adaptado para traer el nombre del rol)
        public static UsuarioDTO? Login(string username, string password)
        {
            UsuarioDTO? usuario = null;
            Conexion conexionDB = new Conexion();
            try
            {
                using (MySqlConnection conn = conexionDB.ObtenerConexion())
                {
                    string query = @"SELECT u.id, u.usuario, u.id_rol, r.nombre as nombre_rol 
                                     FROM usuarios u 
                                     INNER JOIN roles r ON u.id_rol = r.id 
                                     WHERE u.usuario = @user AND u.password = @pass";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@user", username);
                        cmd.Parameters.AddWithValue("@pass", password);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
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

        // 2. OBTENER TODOS LOS USUARIOS
        public static List<UsuarioDTO> ObtenerTodos()
        {
            List<UsuarioDTO> lista = new List<UsuarioDTO>();
            Conexion conexionDB = new Conexion();
            try
            {
                using (MySqlConnection conn = conexionDB.ObtenerConexion())
                {
                    string query = @"SELECT u.id, u.usuario, u.id_rol, r.nombre as nombre_rol 
                                     FROM usuarios u 
                                     INNER JOIN roles r ON u.id_rol = r.id";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
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

        // 3. INSERTAR USUARIO
        public static void Insertar(UsuarioDTO u)
        {
            Conexion conexionDB = new Conexion();
            using (MySqlConnection conn = conexionDB.ObtenerConexion())
            {
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

        // 4. ELIMINAR USUARIO
        public static void Eliminar(int id)
        {
            Conexion conexionDB = new Conexion();
            using (MySqlConnection conn = conexionDB.ObtenerConexion())
            {
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