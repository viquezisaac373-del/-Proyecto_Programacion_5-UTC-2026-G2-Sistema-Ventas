using MySqlConnector;
using SistemaVentas.DTO;
using Sistema_Completo_De_Ventas;
using System;
using System.Collections.Generic;

namespace SistemaVentas.DAL
{
    public static class UsuarioDAO
    {
        // -------------------------------------------------------------
        // MÉTODO DE LOGIN (El que ya teníamos, sin cambios)
        // -------------------------------------------------------------
        public static UsuarioDTO? Login(string username, string password)
        {
            UsuarioDTO? usuario = null;
            Conexion conexionDB = new Conexion();

            try
            {
                using (MySqlConnection conn = conexionDB.ObtenerConexion())
                {
                    string query = "SELECT idUsuario, nombreUsuario, rol FROM usuarios WHERE nombreUsuario = @user AND clave = @pass";
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
                                    IdUsuario = reader.GetInt32("idUsuario"),
                                    NombreUsuario = reader.GetString("nombreUsuario"),
                                    Rol = reader.GetString("rol")
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la conexión a BD: " + ex.Message);
            }
            return usuario;
        }

        // -------------------------------------------------------------
        // NUEVO: OBTENER TODOS LOS USUARIOS (Para llenar la tabla)
        // -------------------------------------------------------------
        public static List<UsuarioDTO> ObtenerTodos()
        {
            List<UsuarioDTO> lista = new List<UsuarioDTO>();
            Conexion conexionDB = new Conexion();

            try
            {
                using (MySqlConnection conn = conexionDB.ObtenerConexion())
                {
                    // No traemos la contraseña (clave) por seguridad, solo mostramos los datos públicos
                    string query = "SELECT idUsuario, nombreUsuario, rol FROM usuarios";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lista.Add(new UsuarioDTO
                                {
                                    IdUsuario = reader.GetInt32("idUsuario"),
                                    NombreUsuario = reader.GetString("nombreUsuario"),
                                    Rol = reader.GetString("rol")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de usuarios: " + ex.Message);
            }
            return lista;
        }

        // -------------------------------------------------------------
        // NUEVO: INSERTAR USUARIO (Botón Guardar - Nuevo)
        // -------------------------------------------------------------
        public static void Insertar(UsuarioDTO usuario)
        {
            Conexion conexionDB = new Conexion();
            try
            {
                using (MySqlConnection conn = conexionDB.ObtenerConexion())
                {
                    string query = "INSERT INTO usuarios (nombreUsuario, clave, rol) VALUES (@user, @pass, @rol)";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@user", usuario.NombreUsuario);
                        cmd.Parameters.AddWithValue("@pass", usuario.Clave);
                        cmd.Parameters.AddWithValue("@rol", usuario.Rol);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar el nuevo usuario en BD: " + ex.Message);
            }
        }

        // -------------------------------------------------------------
        // NUEVO: ACTUALIZAR USUARIO (Botón Editar -> Guardar)
        // -------------------------------------------------------------
        public static void Actualizar(UsuarioDTO usuario)
        {
            Conexion conexionDB = new Conexion();
            try
            {
                using (MySqlConnection conn = conexionDB.ObtenerConexion())
                {
                    // Nota: En un sistema real, si la clave viene vacía, no se actualiza. Aquí la sobreescribimos para simplificar.
                    string query = "UPDATE usuarios SET nombreUsuario = @user, clave = @pass, rol = @rol WHERE idUsuario = @id";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", usuario.IdUsuario);
                        cmd.Parameters.AddWithValue("@user", usuario.NombreUsuario);
                        cmd.Parameters.AddWithValue("@pass", usuario.Clave);
                        cmd.Parameters.AddWithValue("@rol", usuario.Rol);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar los datos del usuario: " + ex.Message);
            }
        }

        // -------------------------------------------------------------
        // NUEVO: ELIMINAR USUARIO (Botón Eliminar)
        // -------------------------------------------------------------
        public static void Eliminar(int idUsuario)
        {
            Conexion conexionDB = new Conexion();
            try
            {
                using (MySqlConnection conn = conexionDB.ObtenerConexion())
                {
                    string query = "DELETE FROM usuarios WHERE idUsuario = @id";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", idUsuario);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar borrar el usuario de la BD: " + ex.Message);
            }
        }
    }
}