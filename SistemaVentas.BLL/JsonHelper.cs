using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using SistemaVentas.DTO;

namespace SistemaVentas.BLL
{
    // Clase auxiliar para exportar datos a archivos JSON
    public class JsonHelper
    {
        // Método para exportar la lista de clientes a un archivo JSON
        public static void ExportarClientes(List<ClienteDTO> clientes)
        {
            try
            {
                // Convierte la lista de clientes a formato JSON con indentación
                string json = JsonSerializer.Serialize(clientes, new JsonSerializerOptions { WriteIndented = true });

                // Obtiene la ruta del escritorio del usuario
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                // Define la ruta completa del archivo
                string fullPath = Path.Combine(desktopPath, "clientes.json");

                // Escribe el archivo JSON en el escritorio
                File.WriteAllText(fullPath, json);
            }
            catch (Exception ex)
            {
                // Manejo de errores en caso de fallo
                throw new Exception("Error al exportar clientes a JSON: " + ex.Message);
            }
        }

        // Método para exportar la lista de productos a un archivo JSON
        public static void ExportarProductos(List<ProductoDTO> productos)
        {
            try
            {
                // Convierte la lista de productos a formato JSON con indentación
                string json = JsonSerializer.Serialize(productos, new JsonSerializerOptions { WriteIndented = true });

                // Obtiene la ruta del escritorio del usuario
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                // Define la ruta completa del archivo
                string fullPath = Path.Combine(desktopPath, "productos.json");

                // Escribe el archivo JSON en el escritorio
                File.WriteAllText(fullPath, json);
            }
            catch (Exception ex)
            {
                // Manejo de errores en caso de fallo
                throw new Exception("Error al exportar productos a JSON: " + ex.Message);
            }
        }
    }
}