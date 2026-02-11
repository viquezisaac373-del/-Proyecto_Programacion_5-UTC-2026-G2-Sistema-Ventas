using System;

namespace SistemaVentas
{
    // Hereda de Persona
    public class Cliente : Persona
    {
        public string Telefono { get; set; }

        // El constructor pasa datos al padre con 'base'
        public Cliente(int id, string nombre, string correo, string telefono)
            : base(id, nombre, correo)
        {
            Telefono = telefono;
        }

        // 'override' aplica Polimorfismo: cambiamos cómo se muestra la info
        public override void MostrarInformacion()
        {
            Console.WriteLine($"Cliente: {Nombre} | Correo: {Correo} | Tel: {Telefono}");
        }
    }
}
