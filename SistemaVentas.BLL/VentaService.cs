using System;
using SistemaVentas.DTO;
using SistemaVentas.DAL;
using System.Collections.Generic;
using Sistema_Completo_De_Ventas;
using System.Linq;

namespace SistemaVentas.BLL
{
    public class VentaService
    {
        private VentaDAO ventaDAO = new VentaDAO();

        public int RegistrarVenta(VentaDTO venta, List<VentaDetalleDTO> detalles)
        {
            try
            {
                decimal subtotal = 0;

                foreach (var d in detalles)
                {
                    subtotal += d.PrecioUnitario * d.Cantidad;
                }

                decimal impuesto = subtotal * 0.13m;
                decimal total = subtotal + impuesto;

                venta.Subtotal = subtotal;
                venta.Impuesto = impuesto;
                venta.Total = total;
                venta.Fecha = DateTime.Now;

                int ventaId = ventaDAO.GuardarVenta(venta);

                foreach (var d in detalles)
                {
                    d.VentaId = ventaId;
                    ventaDAO.GuardarDetalleVenta(d);

                    var producto = ProductoDAO.ObtenerProductos()
                        .FirstOrDefault(p => p.Codigo == d.CodigoP);

                    if (producto != null)
                    {
                        if (d.Cantidad > producto.Stock)
                        {
                            throw new Exception("Stock insuficiente para el producto: " + producto.Nombre);
                        }

                        int nuevoStock = producto.Stock - d.Cantidad;
                        ProductoDAO.ActualizarStock(producto.Codigo, nuevoStock);
                    }
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

        public List<(int Codigo, int Cantidad)> ObtenerTopProductos()
        {
            return ventaDAO.ObtenerTopProductos();
        }
    }
}