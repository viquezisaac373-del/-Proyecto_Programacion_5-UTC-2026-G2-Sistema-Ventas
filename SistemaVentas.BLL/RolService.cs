using SistemaVentas.DAL;
using SistemaVentas.DTO;
using System;
using System.Collections.Generic;
using System.Text;

public class RolService
{
    private RolDAO dao = new RolDAO();

    public List<RolDTO> ObtenerRoles()
    {
        return dao.ObtenerRoles();
    }
}