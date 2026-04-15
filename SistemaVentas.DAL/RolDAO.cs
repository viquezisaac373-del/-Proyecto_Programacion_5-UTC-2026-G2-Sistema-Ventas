    using global::SistemaVentas.DTO;
    using MySqlConnector;
    using Sistema_Completo_De_Ventas;
    using SistemaVentas.DTO;
    using System.Collections.Generic;

    namespace SistemaVentas.DAL
    {
        public class RolDAO
        {
            private Conexion conexion = new Conexion();

            public List<RolDTO> ObtenerRoles()
            {
                List<RolDTO> lista = new List<RolDTO>();

                using (var conn = conexion.ObtenerConexion())
                {
                    string query = "SELECT id, nombre FROM roles";

                    using (var cmd = new MySqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new RolDTO
                            {
                                Id = reader.GetInt32("id"),
                                Nombre = reader.GetString("nombre")
                            });
                        }
                    }
                }

                return lista;
            }
        }
    }

