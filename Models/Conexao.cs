using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prova_web.Models
{
    public class Conexao : IDisposable
    {      
        public MySqlConnection conn;

        private readonly string _server = "localhost";
        private readonly string _port = "3306";
        private readonly string _database = "bd_aula";
        private readonly string _uid = "root";
        private readonly string _pwd = "15908523"; // senha para acessar o banco de dados

        public Conexao()
        {
            Conectar();
        }

        private void Conectar()
        {
            string _strConn = "Server=" + _server;
            _strConn += "; Port=" + _port;
            _strConn += "; Database=" + _database;
            _strConn += "; Uid=" + _uid;
            _strConn += "; Pwd=" + _pwd;

            conn = new MySqlConnection(_strConn);
            try
            {
                conn.Open();

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void Dispose()
        {
            conn.Close();
            conn.Dispose();
        }
    }
}