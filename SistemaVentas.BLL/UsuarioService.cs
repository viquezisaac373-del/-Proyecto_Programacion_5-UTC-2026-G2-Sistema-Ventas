using System;
using System.Collections.Generic;
using SistemaVentas.DAL;
using SistemaVentas.DTO;

namespace SistemaVentas.BLL
{
    public class UsuarioService
    {
        // -------------------------------------------------------------
        // MÉTODO DE LOGIN 
        // -------------------------------------------------------------
        public UsuarioDTO? IniciarSesion(string user, string pass)
        {
            if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(pass))
                throw new Exception("El usuario y la contraseña son obligatorios.");

            UsuarioDTO? usuario = UsuarioDAO.Login(user, pass);

            if (usuario == null)
                throw new Exception("Credenciales incorrectas. Verifique su usuario y contraseña.");

            return usuario;
        }

        // -------------------------------------------------------------
        // NUEVO: OBTENER LISTA DE USUARIOS (Para llenar la tabla)
        // -------------------------------------------------------------
        public List<UsuarioDTO> ListarUsuarios()
        {
            return UsuarioDAO.ObtenerTodos();
        }

        // -------------------------------------------------------------
        // NUEVO: GUARDAR O ACTUALIZAR USUARIO
        // -------------------------------------------------------------
        public void GuardarUsuario(UsuarioDTO usuario)
        {
            // Validaciones de seguridad
            if (string.IsNullOrWhiteSpace(usuario.NombreUsuario))
                throw new Exception("El nombre de usuario no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(usuario.Clave))
                throw new Exception("La contraseña es obligatoria para nuevos usuarios.");

            if (string.IsNullOrWhiteSpace(usuario.Rol))
                throw new Exception("Debe asignar un rol (Admin, Cajero, etc.) al usuario.");

            // Lógica: Si el ID es 0 es una inserción, si es mayor es una actualización
            if (usuario.IdUsuario == 0)
            {
                UsuarioDAO.Insertar(usuario);
            }
            else
            {
                UsuarioDAO.Actualizar(usuario);
            }
        }

        // -------------------------------------------------------------
        // NUEVO: ELIMINAR USUARIO
        // -------------------------------------------------------------
        public void EliminarUsuario(int idUsuario)
        {
            if (idUsuario <= 0)
                throw new Exception("Debe seleccionar un usuario válido de la lista.");

            UsuarioDAO.Eliminar(idUsuario);
        }
    }
}