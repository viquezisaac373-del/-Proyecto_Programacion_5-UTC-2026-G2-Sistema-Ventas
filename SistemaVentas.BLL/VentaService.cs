using System;
using SistemaVentas.DTO;
using SistemaVentas.DAL;
using System.Collections.Generic;

namespace SistemaVentas.BLL
{
    public class VentaService
    {
        private VentaDAO ventaDAO = new VentaDAO();

        public int RegistrarVenta(VentaDTO venta, List<VentaDetalleDTO> detalles)
        {
            try
            {
                int ventaId = ventaDAO.GuardarVenta(venta);

                foreach (var d in detalles)
                {
                    d.VentaId = ventaId;
                    ventaDAO.GuardarDetalleVenta(d);
                }

                return ventaId;
            }
            catch (Exception ex)
            {
                throw new Exception("Error en BLL al registrar venta: " + ex.Message);
            }
        }

        public List<VentaDetalleDTO> ObtenerDetalles(int ventaId)
        {
            try
            {
                VentaDAO dao = new VentaDAO();
                return dao.ObtenerDetallesPorVenta(ventaId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en BLL al obtener detalles: " + ex.Message);
            }
        }

        public List<VentaDTO> ObtenerVentas()
        {
            try
            {
                return ventaDAO.ObtenerVentas();
            }
            catch (Exception ex)
            {
                throw new Exception("Error en BLL al obtener ventas: " + ex.Message);
            }
        }

        public List<(DateTime Fecha, int Cantidad)> ObtenerVentasPorDia()
        {
            return ventaDAO.ObtenerVentasPorDia();
        }

        public List<(string Codigo, int Cantidad)> ObtenerTopProductos()
        {
            return ventaDAO.ObtenerTopProductos();
        }
    }
}