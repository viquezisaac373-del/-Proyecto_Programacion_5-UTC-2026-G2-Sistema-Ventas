using System;
using System.Collections.Generic;
using Sistema_Completo_De_Ventas; // Donde vive ProductoDAO y Producto
using SistemaVentas.DTO; // Para mapeos si es necesario, aunque ProductoDAO devuelve Producto nativo.

namespace SistemaVentas.BLL
{
    public class ProductoService
    {
        public List<Producto> ObtenerProductos()
        {
            return ProductoDAO.ObtenerProductos();
        }

        public void InsertarProducto(Producto producto)
        {
            if (producto.Precio <= 0)
                throw new Exception("Regla de negocio: El precio debe ser mayor a 0.");

            if (producto.Stock < 0)
                throw new Exception("Regla de negocio: El stock no puede ser negativo.");

            if (string.IsNullOrWhiteSpace(producto.Nombre))
                throw new Exception("Regla de negocio: El nombre del producto es obligatorio.");

            ProductoDAO.InsertarProducto(producto);
        }

        public void ActualizarProducto(Producto producto)
        {
            if (producto.Precio <= 0)
                throw new Exception("Regla de negocio: El precio debe ser mayor a 0.");

            if (producto.Stock < 0)
                throw new Exception("Regla de negocio: El stock no puede ser negativo.");

            if (string.IsNullOrWhiteSpace(producto.Nombre))
                throw new Exception("Regla de negocio: El nombre del producto es obligatorio.");

            ProductoDAO.ActualizarProducto(producto);
        }

        public void EliminarProducto(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new Exception("Regla de negocio: Se requiere el código para eliminar.");

            ProductoDAO.EliminarProducto(codigo);
        }
    }
}
