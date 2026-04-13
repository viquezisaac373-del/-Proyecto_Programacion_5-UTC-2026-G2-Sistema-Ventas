using System;
using System.Collections.Generic;
using System.Linq;
using Sistema_Completo_De_Ventas;
using SistemaVentas.DTO;

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

        public bool ExisteProducto(int codigo)
        {
            var productos = ObtenerProductos();
            return productos.Any(p => p.Codigo == codigo);
        }

        public void ActualizarProducto(Producto producto)
        {
            if (producto.Precio <= 0)
                throw new Exception("Regla de negocio: El precio debe ser mayor a 0.");

            if (producto.Stock < 0)
                throw new Exception("Regla de negocio: El stock no puede ser negativo.");

            if (string.IsNullOrWhiteSpace(producto.Nombre))
                throw new Exception("Regla de negocio: El nombre del producto es obligatorio.");

            if (producto.Codigo <= 0)
                throw new Exception("Regla de negocio: Código inválido.");

            ProductoDAO.ActualizarProducto(producto);
        }

        public void EliminarProducto(int codigo)
        {
            if (codigo <= 0)
                throw new Exception("Regla de negocio: Código inválido.");

            if (ProductoDAO.TieneVentas(codigo))
                throw new Exception("No se puede eliminar el producto porque tiene ventas registradas.");

            ProductoDAO.EliminarProducto(codigo);
        }
    }
}