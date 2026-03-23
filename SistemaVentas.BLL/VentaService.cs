using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVentas.DTO;
using SistemaVentas.DAL;

namespace SistemaVentas.BLL
{
    public class VentaService
    {
        private VentaDAO ventaDAO = new VentaDAO();

        public int RegistrarVenta(VentaDTO venta, List<VentaDetalleDTO> detalles)
        {
            int ventaId = ventaDAO.GuardarVenta(venta);

            foreach (var d in detalles)
            {
                d.VentaId = ventaId;
                ventaDAO.GuardarDetalleVenta(d);
            }

            return ventaId; // devolvemos el ID real
        }

        public List<VentaDetalleDTO> ObtenerDetalles(int ventaId)
        {
            VentaDAO dao = new VentaDAO();

            // Llamamos al DAO para obtener los detalles
            return dao.ObtenerDetallesPorVenta(ventaId);
        }

        public List<VentaDTO> ObtenerVentas()
        {
            return ventaDAO.ObtenerVentas();
        }
    }
}