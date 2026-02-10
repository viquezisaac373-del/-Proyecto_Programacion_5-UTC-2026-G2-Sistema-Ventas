using System;

namespace SistemaVentas
{
    public abstract class Persona
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }

        public Persona(int id, string nombre, string correo)
        {
            Id = id;
            Nombre = nombre;
            Correo = correo;
        }

        public virtual void MostrarInformacion()
        {
            Console.WriteLine($"ID: {Id} | Nombre: {Nombre}");
        }
    }
}
