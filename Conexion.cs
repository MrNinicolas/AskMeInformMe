using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskMeInform
{
    internal class Conexion
    {
        private MySqlConnection conexion;
        private string server = "localhost";
        private string database = "DBpreguntas";
        private string user = "root";
        private string password = "nico";
        private string cadenaConexion;

        public Conexion()
        {
            cadenaConexion = "Database=" + database + "; Data Source=" + server + "; User Id=" + user + "; Password=" + password + ";";
            // Establecer la conexión en el constructor
            conexion = new MySqlConnection(cadenaConexion);
            conexion.Open();
        }

        public MySqlConnection GetConexion()
        {
            return conexion;
        }

        public void CerrarConexion()
        {
            if (conexion.State == ConnectionState.Open)
            {
                conexion.Close();
            }
        }
    }
}
