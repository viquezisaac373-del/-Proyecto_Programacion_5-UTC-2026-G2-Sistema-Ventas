using System;

namespace Sistema_Completo_De_Ventas 
{
    public class Cliente : Persona // Cliente hereda de Persona (herencia)
    {
        public string Telefono { get; set; } // Propiedad para almacenar el teléfono del cliente

        public Cliente(int id, string nombre, string correo, string telefono) : base(id, nombre, correo) // Llama al constructor de Persona
        {
            Telefono = telefono; // Asigna el teléfono recibido a la propiedad
        }

        public override void MostrarInformacion() // Sobrescribe el método de Persona (polimorfismo)
        {
            Console.WriteLine($"Cliente: {Id} | {Nombre} | Correo: {Correo} | Tel: {Telefono}"); // Muestra los datos completos del cliente
        }
    }
}