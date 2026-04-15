using SistemaVentas.DAL;
using SistemaVentas.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaVentas.BLL
{
    public class UsuarioService
    {
        private UsuarioDAO dao = new UsuarioDAO();

        public List<UsuarioDTO> ObtenerUsuarios()
        {
            return dao.ObtenerUsuarios();
        }

        public void InsertarUsuario(UsuarioDTO usuario)
        {
            dao.InsertarUsuario(usuario);
        }

        public void ActualizarUsuario(UsuarioDTO usuario)
        {
            dao.ActualizarUsuario(usuario);
        }

        public void EliminarUsuario(int id)
        {
            dao.EliminarUsuario(id);
        }
    }
}
