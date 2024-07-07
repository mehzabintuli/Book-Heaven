using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LibraryManagement.Models
{
    public class Database
    {
        private SqlConnection Con;
        private SqlCommand Cmd;
        private DataTable Dt;
        private SqlDataAdapter sda;
        private string ConStr;

        public Database()
        {
            ConStr = @"Data Source=DESKTOP-QC5AOMH\SQLEXPRESS;Initial Catalog=BookHeaven;Integrated Security=True";
            Con = new SqlConnection(ConStr);
            Cmd = new SqlCommand();
            Cmd.Connection = Con;
        }

        public DataTable GetData(string Query)
        {
            Dt = new DataTable();
            try
            {
                // Open the connection if it's not already open
                if (Con.State == ConnectionState.Closed)
                {
                    Con.Open();
                }

                sda = new SqlDataAdapter(Query, Con);
                sda.Fill(Dt);
            }
            catch (Exception ex)
            {
                // Log the exception message
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            finally
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
            }
            return Dt;
        }

        public int SetData(string Query)
        {
            int cnt = 0;
            try
            {
                if (Con.State == ConnectionState.Closed)
                {
                    Con.Open();
                }
                Cmd.CommandText = Query;
                cnt = Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // Log the exception message
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            finally
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
            }
            return cnt;
        }
    }
}
