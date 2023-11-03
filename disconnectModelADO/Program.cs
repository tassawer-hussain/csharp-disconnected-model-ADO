using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient; // Use for creating table in data base
using System.Data; // Use for Data Table

namespace disconnectModelADO
{
    class Program
    {
        static String connectionString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=c:\users\tassawer\documents\visual studio 2013\Projects\disconnectModelADO\disconnectModelADO\DisconnectedModelDB.mdf;Integrated Security=True";
        static void Main(string[] args)
        {
            // create connection
            SqlConnection connection = new SqlConnection(connectionString);
            Console.WriteLine("Read Tables from DB");

            // Step 1 open data base
            connection.Open();

            // step 2 write query
            string query = "select * from Users";
            SqlCommand command = new SqlCommand(query, connection);

            // The SqlDataAdapter, serves as a bridge between a DataSet and SQL Server for retrieving and saving data.
            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = command;

            DataTable targetTable = new DataTable();
            dataAdapter.Fill(targetTable);

            foreach (DataRow Row in targetTable.Rows)
            {
                Console.WriteLine("RollNumber: {0}, Name: {1}, Password: {2}", Row[0], Row[1], Row[2]);
            }

            // step 3 close connection
            connection.Close();

            // step 4 now play with the duplicated data that is stored in Memory and after that submit changes int the last
            DataRow drow = targetTable.NewRow();
            drow["RollNumber"] = "Mcsf13m001";
            drow["Name"] = "Fatima Sajjad";
            drow["Password"] = "fatima";
            targetTable.Rows.Add(drow);

            // update
            DataRow uprow = targetTable.Rows[1];
            uprow["Name"] = "T.H";
            uprow["Password"] = "TH_cms";

            // Delete
            DataRow delrow = targetTable.Rows[4];
            delrow.Delete();
            //targetTable.Rows.Remove(rowToDelete);

            //the difference b/w delete and remove is that delete will mark this row need to delete when we call update method
            //but remove will immediatly delete this row from the table and hence no footprint left and database row will not
            // remove from the external database.

            // step 5 again open connection
            connection.Open();

            // step 6 update the dataBase 
            SqlCommandBuilder cb = new SqlCommandBuilder(dataAdapter);
            dataAdapter.InsertCommand = cb.GetInsertCommand();
            dataAdapter.Update(targetTable);

            // 7
            connection.Close();
        }
    }
}
