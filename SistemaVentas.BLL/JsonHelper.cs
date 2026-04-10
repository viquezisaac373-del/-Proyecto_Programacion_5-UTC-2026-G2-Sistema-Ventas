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
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string fullPath = Path.Combine(desktopPath, "clientes.json");
                File.WriteAllText(fullPath, json);
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
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string fullPath = Path.Combine(desktopPath, "productos.json");
                File.WriteAllText(fullPath, json);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al exportar productos a JSON: " + ex.Message);
            }
        }
    }
}