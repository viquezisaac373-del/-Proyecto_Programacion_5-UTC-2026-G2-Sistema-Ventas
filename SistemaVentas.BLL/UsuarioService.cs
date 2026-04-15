using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using SistemaVentas.DAL;
using SistemaVentas.DTO;

namespace SistemaVentas.BLL
{
    public class UsuarioService
    {
        // Encriptar la contraseña para que coincida con la base de datos
        private string EncriptarSHA256(string texto)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(texto));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public UsuarioDTO? IniciarSesion(string user, string pass)
        {
            if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(pass))
                throw new Exception("El usuario y la contraseña son obligatorios.");

            // Hasheamos la clave ingresada antes de enviarla a la BD
            string hashPassword = EncriptarSHA256(pass);
            UsuarioDTO? usuario = UsuarioDAO.Login(user, hashPassword);

            if (usuario == null)
                throw new Exception("Credenciales incorrectas.");

            return usuario;
        }

        public List<UsuarioDTO> ListarUsuarios()
        {
            return UsuarioDAO.ObtenerTodos();
        }

        public void GuardarUsuario(UsuarioDTO usuario, string claveSinEncriptar)
        {
            if (string.IsNullOrWhiteSpace(usuario.Usuario)) throw new Exception("El usuario no puede estar vacío.");
            if (usuario.IdRol <= 0) throw new Exception("Debe asignar un rol válido.");

            // Si es un usuario nuevo, la contraseña es obligatoria
            if (usuario.Id == 0 && string.IsNullOrWhiteSpace(claveSinEncriptar))
            {
                throw new Exception("La contraseña es obligatoria para nuevos usuarios.");
            }

            // Encriptamos la clave antes de guardar
            usuario.Password = EncriptarSHA256(claveSinEncriptar);

            UsuarioDAO.Insertar(usuario); // Solo permitimos Insertar por ahora para no complicarlo
        }

        public void EliminarUsuario(int idUsuario)
        {
            if (idUsuario <= 0) throw new Exception("Seleccione un usuario válido.");
            UsuarioDAO.Eliminar(idUsuario);
        }
    }
}