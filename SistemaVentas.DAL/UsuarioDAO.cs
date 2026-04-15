using MySqlConnector;
using Sistema_Completo_De_Ventas;
using SistemaVentas.DTO;
using System;
using System.Collections.Generic;

namespace SistemaVentas.DAL
{
    public class UsuarioDAO
    {
        private Conexion conexion = new Conexion();

        //  Obtener todos los usuarios
        public List<UsuarioDTO> ObtenerUsuarios()
        {
            List<UsuarioDTO> lista = new List<UsuarioDTO>();

            using (var conn = conexion.ObtenerConexion())
            {
                string query = "SELECT u.id, u.usuario, u.password, u.id_rol, r.nombre AS rol\r\nFROM usuarios u\r\nINNER JOIN roles r ON u.id_rol = r.id";

                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new UsuarioDTO
                        {
                            Id = reader.GetInt32("id"),
                            Usuario = reader.GetString("usuario"),
                            Password = reader.GetString("password"),
                            IdRol = reader.GetInt32("id_rol")
                        });
                    }
                }
            }

            return lista;
        }

        //  Insertar usuario
        public void InsertarUsuario(UsuarioDTO usuario)
        {

            using (var conn = conexion.ObtenerConexion())
            {

                string query = @"INSERT INTO usuarios (usuario, password, id_rol) 
                                 VALUES (@usuario, @password, @id_rol)";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@usuario", usuario.Usuario);
                    cmd.Parameters.AddWithValue("@password", usuario.Password);
                    cmd.Parameters.AddWithValue("@id_rol", usuario.IdRol);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        //  Actualizar usuario
        public void ActualizarUsuario(UsuarioDTO usuario)
        {
            using (var conn = conexion.ObtenerConexion())
            {
                string query = @"UPDATE usuarios 
                                 SET usuario = @usuario, 
                                     password = @password, 
                                     id_rol = @id_rol
                                 WHERE id = @id";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", usuario.Id);
                    cmd.Parameters.AddWithValue("@usuario", usuario.Usuario);
                    cmd.Parameters.AddWithValue("@password", usuario.Password);
                    cmd.Parameters.AddWithValue("@id_rol", usuario.IdRol);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        //  Eliminar usuario
        public void EliminarUsuario(int id)
        {
            using (var conn = conexion.ObtenerConexion())
            {
                string query = "DELETE FROM usuarios WHERE id = @id";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

     
    }
}