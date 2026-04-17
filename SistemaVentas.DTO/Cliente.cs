using System;

namespace Sistema_Completo_De_Ventas
{
    // Clase Cliente que hereda de la clase base Persona
    public class Cliente : Persona
    {
        // Propiedad adicional específica del cliente
        public string Telefono { get; set; }

        // Constructor que recibe los datos del cliente
        // Llama al constructor de la clase base (Persona)
        public Cliente(int id, string nombre, string correo, string telefono)
            : base(id, nombre, correo)
        {
            Telefono = telefono; // Inicializa el teléfono
        }

        // Método sobrescrito para mostrar la información del cliente
        public override void MostrarInformacion()
        {
            // Muestra los datos del cliente en consola
            Console.WriteLine($"Cliente: {Id} | {Nombre} | Correo: {Correo} | Tel: {Telefono}");
        }
    }
}