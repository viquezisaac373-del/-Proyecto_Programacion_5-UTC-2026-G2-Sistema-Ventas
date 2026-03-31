using System;

namespace Sistema_Completo_De_Ventas
{
    public abstract class Persona // Clase abstracta que sirve como base para otras clases como ClienteDTO
    {
        public int Id { get; set; } // Propiedad que almacena el identificador único de la persona
        public string Nombre { get; set; } // Propiedad que almacena el nombre de la persona
        public string Correo { get; set; } // Propiedad que almacena el correo electrónico de la persona

        public Persona(int id, string nombre, string correo) // Constructor que inicializa las propiedades básicas
        {
            Id = id; // Asigna el valor del parámetro id a la propiedad Id
            Nombre = nombre; // Asigna el valor del parámetro nombre a la propiedad Nombre
            Correo = correo; // Asigna el valor del parámetro correo a la propiedad Correo
        }

        public virtual void MostrarInformacion() // Método virtual que permite ser sobrescrito por clases derivadas
        {
            Console.WriteLine($"ID: {Id} | Nombre: {Nombre} | Correo: {Correo}");
        }
    }
}