using System;
using System.Text.Json;

namespace Sistema_Completo_De_Ventas
{
    public class Cliente : Persona // Cliente hereda de Persona (herencia)
    {
        public string Telefono { get; set; } // Propiedad para almacenar el teléfono del cliente

        public Cliente(int id, string nombre, string correo, string telefono) : base(id, nombre, correo) // Llama al constructor de Persona
        {
            // Validación del nombre
            if (string.IsNullOrWhiteSpace(nombre))
                throw new Exception("El nombre no puede estar vacío.");

            // Validación del correo
            if (string.IsNullOrWhiteSpace(correo) || !correo.Contains("@"))
                throw new Exception("El correo no es válido.");

            // Validación del teléfono
            if (string.IsNullOrWhiteSpace(telefono))
                throw new Exception("El teléfono no puede estar vacío.");

            Telefono = telefono; // Asigna el teléfono recibido a la propiedad
        }

        public override void MostrarInformacion() // Sobrescribe el método de Persona (polimorfismo)
        {
            Console.WriteLine($"Cliente: {Id} | {Nombre} | Correo: {Correo} | Tel: {Telefono}");
        }
    }
}
