using System;
using SistemaVentas.DTO;
using SistemaVentas.DAL;
using System.Collections.Generic;
using Sistema_Completo_De_Ventas;
using System.Linq;

namespace SistemaVentas.BLL
{
    // Clase que maneja la lógica de negocio relacionada con las ventas
    public class VentaService
    {
        // Instancia del DAO para acceder a la base de datos
        private VentaDAO ventaDAO = new VentaDAO();

        // Método para registrar una venta junto con sus detalles
        public int RegistrarVenta(VentaDTO venta, List<VentaDetalleDTO> detalles)
        {
            try
            {
                decimal subtotal = 0;

                // Calcula el subtotal sumando cada detalle
                foreach (var d in detalles)
                {
                    subtotal += d.PrecioUnitario * d.Cantidad;
                }

                // Calcula impuesto (13%) y total
                decimal impuesto = subtotal * 0.13m;
                decimal total = subtotal + impuesto;

                // Asigna valores a la venta
                venta.Subtotal = subtotal;
                venta.Impuesto = impuesto;
                venta.Total = total;
                venta.Fecha = DateTime.Now;

                // Guarda la venta y obtiene el ID generado
                int ventaId = ventaDAO.GuardarVenta(venta);

                // Guarda cada detalle de la venta
                foreach (var d in detalles)
                {
                    d.VentaId = ventaId;
                    ventaDAO.GuardarDetalleVenta(d);

                    // Busca el producto correspondiente
                    var producto = ProductoDAO.ObtenerProductos()
                        .FirstOrDefault(p => p.Codigo == d.CodigoP);

                    if (producto != null)
                    {
                        // Verifica si hay suficiente stock
                        if (d.Cantidad > producto.Stock)
                        {
                            throw new Exception("Stock insuficiente para el producto: " + producto.Nombre);
                        }

                        // Calcula el nuevo stock
                        int nuevoStock = producto.Stock - d.Cantidad;

                        // Actualiza el stock en la base de datos
                        ProductoDAO.ActualizarStock(producto.Codigo, nuevoStock);
                    }
                }

                // Retorna el ID de la venta registrada
                return ventaId;
            }
            catch (Exception ex)
            {
                // Manejo de errores
                throw new Exception("Error en BLL al registrar venta: " + ex.Message);
            }
        }

        // Obtiene los detalles de una venta específica
        public List<VentaDetalleDTO> ObtenerDetalles(int ventaId)
        {
            try
            {
                VentaDAO dao = new VentaDAO();

                // Llama al DAO para obtener los detalles
                return dao.ObtenerDetallesPorVenta(ventaId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en BLL al obtener detalles: " + ex.Message);
            }
        }

        // Obtiene todas las ventas registradas
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

        // Obtiene la cantidad de ventas agrupadas por día
        public List<(DateTime Fecha, int Cantidad)> ObtenerVentasPorDia()
        {
            return ventaDAO.ObtenerVentasPorDia();
        }

        // Obtiene los productos más vendidos (top productos)
        public List<(int Codigo, int Cantidad)> ObtenerTopProductos()
        {
            return ventaDAO.ObtenerTopProductos();
        }
    }
}