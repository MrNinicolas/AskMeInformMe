using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskMeInform
{
    internal class Pregunta
    {
        public int Id { get; }
        public string Texto { get; }
        public List<Opcion> Opciones { get; }
        public int? RespuestaCorrectaId { get; }  // Cambio a int? para permitir valores nulos

        public Pregunta(int id, string texto, List<Opcion> opciones, int? respuestaCorrectaId)
        {
            Id = id;
            Texto = texto;
            Opciones = opciones;
            RespuestaCorrectaId = respuestaCorrectaId;
        }
    }
}
