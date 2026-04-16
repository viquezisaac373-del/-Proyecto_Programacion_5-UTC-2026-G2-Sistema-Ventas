using System;
using System.Collections.Generic;
using System.Linq;
using Sistema_Completo_De_Ventas;
using SistemaVentas.DTO;

namespace SistemaVentas.BLL
{
    // Clase que contiene la lógica de negocio para los productos
    public class ProductoService
    {
        // Obtiene la lista de productos desde la capa de datos (DAO)
        public List<Producto> ObtenerProductos()
        {
            return ProductoDAO.ObtenerProductos();
        }

        // Inserta un nuevo producto en la base de datos
        public void InsertarProducto(Producto producto)
        {
            // Valida que el precio sea mayor a 0
            if (producto.Precio <= 0)
                throw new Exception("Regla de negocio: El precio debe ser mayor a 0.");

            // Valida que el stock no sea negativo
            if (producto.Stock < 0)
                throw new Exception("Regla de negocio: El stock no puede ser negativo.");

            // Valida que el nombre no esté vacío
            if (string.IsNullOrWhiteSpace(producto.Nombre))
                throw new Exception("Regla de negocio: El nombre del producto es obligatorio.");

            // Llama al DAO para insertar el producto
            ProductoDAO.InsertarProducto(producto);
        }

        // Verifica si un producto existe por su código
        public bool ExisteProducto(int codigo)
        {
            var productos = ObtenerProductos();

            // Busca si existe un producto con ese código
            return productos.Any(p => p.Codigo == codigo);
        }

        // Actualiza la información de un producto
        public void ActualizarProducto(Producto producto)
        {
            // Validaciones de negocio
            if (producto.Precio <= 0)
                throw new Exception("Regla de negocio: El precio debe ser mayor a 0.");

            if (producto.Stock < 0)
                throw new Exception("Regla de negocio: El stock no puede ser negativo.");

            if (string.IsNullOrWhiteSpace(producto.Nombre))
                throw new Exception("Regla de negocio: El nombre del producto es obligatorio.");

            // Valida que el código sea correcto
            if (producto.Codigo <= 0)
                throw new Exception("Regla de negocio: Código inválido.");

            // Llama al DAO para actualizar el producto
            ProductoDAO.ActualizarProducto(producto);
        }

        // Elimina un producto por su código
        public void EliminarProducto(int codigo)
        {
            // Valida que el código sea correcto
            if (codigo <= 0)
                throw new Exception("Regla de negocio: Código inválido.");

            // Verifica si el producto tiene ventas asociadas
            if (ProductoDAO.TieneVentas(codigo))
                throw new Exception("No se puede eliminar el producto porque tiene ventas registradas.");

            // Llama al DAO para eliminar el producto
            ProductoDAO.EliminarProducto(codigo);
        }
    }
}
