using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace AskMeInform
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Conexion conexionBD = new Conexion();
            MySqlConnection conexion = conexionBD.GetConexion();

            if (conexion.State == ConnectionState.Open)
            {
                Console.WriteLine("Conexión establecida correctamente.");

                try
                {
                    // Obtener todas las preguntas con sus opciones y respuestas correctas
                    Dictionary<int, Pregunta> preguntas = ObtenerPreguntas(conexion);

                    // Juego de trivia
                    JugarTrivia(preguntas);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al obtener preguntas: {ex.Message}");
                }
                finally
                {
                    // Cerrar la conexión al finalizar
                    conexionBD.CerrarConexion();
                }
            }
            else
            {
                Console.WriteLine("No se pudo establecer la conexión.");
            }
        }

        static Dictionary<int, Pregunta> ObtenerPreguntas(MySqlConnection conexion)
        {
            Dictionary<int, Pregunta> preguntas = new Dictionary<int, Pregunta>();

            string query = "SELECT p.id AS pregunta_id, p.texto AS pregunta_texto, " +
                           "o.id AS opcion_id, o.texto AS opcion_texto, " +
                           "rc.opcion_id AS respuesta_correcta_id " +
                           "FROM Preguntas p " +
                           "LEFT JOIN Opciones o ON p.id = o.pregunta_id " +
                           "LEFT JOIN RespuestasCorrectas rc ON p.id = rc.pregunta_id";

            MySqlCommand cmd = new MySqlCommand(query, conexion);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int preguntaId = reader.GetInt32("pregunta_id");

                    // Si la pregunta no está en el diccionario, la añadimos
                    if (!preguntas.ContainsKey(preguntaId))
                    {
                        string textoPregunta = reader.GetString("pregunta_texto");
                        int? respuestaCorrectaId = null;

                        if (!reader.IsDBNull(reader.GetOrdinal("respuesta_correcta_id")))
                        {
                            respuestaCorrectaId = reader.GetInt32("respuesta_correcta_id");
                        }

                        preguntas[preguntaId] = new Pregunta(preguntaId, textoPregunta, new List<Opcion>(), respuestaCorrectaId);
                    }

                    // Añadir opción si existe
                    if (!reader.IsDBNull(reader.GetOrdinal("opcion_id")))
                    {
                        int opcionId = reader.GetInt32("opcion_id");
                        string textoOpcion = reader.GetString("opcion_texto");
                        preguntas[preguntaId].Opciones.Add(new Opcion(opcionId, textoOpcion));
                    }
                }
            } // El DataReader se cierra automáticamente al salir del bloque using

            return preguntas;
        }

        static void JugarTrivia(Dictionary<int, Pregunta> preguntas)
        {
            int puntos = 0;

            foreach (var pregunta in preguntas.Values)
            {
                Console.WriteLine(pregunta.Texto);

                foreach (var opcion in pregunta.Opciones)
                {
                    Console.WriteLine($"{opcion.Id}. {opcion.Texto}");
                }

                Console.Write("Ingrese el número de la opción correcta: ");
                int respuestaUsuario = Convert.ToInt32(Console.ReadLine());

                if (pregunta.RespuestaCorrectaId.HasValue && respuestaUsuario == pregunta.RespuestaCorrectaId.Value)
                {
                    Console.WriteLine("¡Respuesta correcta!");
                    puntos++;
                }
                else
                {
                    Console.WriteLine("Respuesta incorrecta.");
                }

                Console.WriteLine();
            }

            Console.WriteLine($"Has respondido correctamente {puntos} de {preguntas.Count} preguntas.");
        }
    }
}