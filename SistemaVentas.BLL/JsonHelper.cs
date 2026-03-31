using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using SistemaVentas.DTO;

namespace SistemaVentas.BLL
{
    public class JsonHelper
    {
        public static void ExportarClientes(List<ClienteDTO> clientes)
        {
            try
            {
                string json = JsonSerializer.Serialize(clientes, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText("clientes.json", json);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al exportar clientes a JSON: " + ex.Message);
            }
        }

        public static void ExportarProductos(List<ProductoDTO> productos)
        {
            try
            {
                string json = JsonSerializer.Serialize(productos, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText("productos.json", json);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al exportar productos a JSON: " + ex.Message);
            }
        }
    }
}