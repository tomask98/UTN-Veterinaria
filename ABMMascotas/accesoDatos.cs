using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace ABMMascotas
{
    internal class accesoDatos //clase para conectar la DB 
    {

        SqlConnection conexion;
        SqlCommand comando;
        SqlDataReader lector;
        string cadenaConexion;
        

        public accesoDatos()
        {
            cadenaConexion = @"Data Source=TOMAS;Initial Catalog=Veterinaria;Integrated Security=True";
            conexion = new SqlConnection(cadenaConexion);
            comando = new SqlCommand();

        }

        private void conectar() //metodo conectar
        {
            conexion.Open();
            comando.Connection = conexion;
            comando.CommandType = CommandType.Text;

        }
        private void desconectar()
        {
            conexion.Close();
        }
        public DataTable consultarBD(string consultaSQL) //metodo para consultar a la base de datos
        {
            DataTable tabla = new DataTable();
            conectar();
            comando.CommandText = consultaSQL;
            tabla.Load(comando.ExecuteReader());

            desconectar();
            return tabla;
        }

        public int actualizarBD(string consultaSQL)
        {
            int filasAfectadas;
            conectar();
            comando.CommandText = consultaSQL;
           filasAfectadas = comando.ExecuteNonQuery();
            desconectar();

            return filasAfectadas;
        }
    }

}
