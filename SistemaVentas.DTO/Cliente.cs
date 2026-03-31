using System;

namespace Sistema_Completo_De_Ventas
{
    // Esta es la clase de dominio Cliente, hereda de Persona
    public class Cliente : Persona
    {
        public string Telefono { get; set; }

        public Cliente(int id, string nombre, string correo, string telefono) : base(id, nombre, correo)
        {
            Telefono = telefono;
        }

        public override void MostrarInformacion()
        {
            Console.WriteLine($"Cliente: {Id} | {Nombre} | Correo: {Correo} | Tel: {Telefono}");
        }
    }
}