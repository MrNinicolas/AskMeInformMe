using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskMeInform
{
    internal class Opcion
    {
        public int Id { get; }
        public string Texto { get; }

        public Opcion(int id, string texto)
        {
            Id = id;
            Texto = texto;
        }
    }
}
