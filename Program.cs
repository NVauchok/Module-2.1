using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main()
        {
            string connectionString = @"Data Source=EPBYMINW0324\SQLEXPRESS;Initial Catalog=Northwind;Integrated Security=True";
            //connection object
            using (var myConnection = new SqlConnection())
            {
                Console.WriteLine("Connection object:" + myConnection.GetType().Name);
                myConnection.ConnectionString = connectionString;

                //open connection
                try
                {
                    myConnection.Open();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Cannot connect to DataBase. Details: " + e.ToString());
                }

                SqlDataReader myReader = null;
                //INSERT command
                try
                {
                    //myReader = null;
                    SqlCommand insertCommand = new SqlCommand("Insert into Employees(LastName, FirstName, City) values ('Cayman', 'Joe', 'Minsk')", myConnection);
                    //myReader = insertCommad.ExecuteReader();
                    //myReader.Close();
                    insertCommand.ExecuteNonQuery();
                }

                catch (Exception e)
                {
                    Console.WriteLine("Cannot execute  INSERT command: " + e.ToString());
                }

                //SELECT command
                try
                {
                    myReader = null;
                    SqlCommand selectCommand = new SqlCommand("Select top 5 * from Employees order by EmployeeID desc", myConnection);
                    myReader = selectCommand.ExecuteReader();
                    Console.WriteLine("-----Results of SELECT command-----");
                    while (myReader.Read())
                    {
                        
                        Console.WriteLine("EmployeeID: " + myReader["EmployeeID"].ToString());
                        Console.WriteLine("First Name: " + myReader["FirstName"].ToString());
                        Console.WriteLine("Last Name: " + myReader["LastName"].ToString());
                        Console.WriteLine("City: " + myReader["City"].ToString());
                        Console.WriteLine("\n");

                    }
                    myReader.Close();


                }
                catch (Exception e)
                {
                    Console.WriteLine("Cannot execute  SELECT command: " + e.ToString());
                }

                //DELETE command
                int numberOfAffectedRows = 0;
                try
                {
                    SqlCommand deleteCommand = new SqlCommand("Delete from Employees where LastName = 'Cayman' ", myConnection);
                    numberOfAffectedRows = deleteCommand.ExecuteNonQuery();
                    Console.WriteLine("-----Results of SELECT command-----");
                    Console.WriteLine("Number of affected rows: " + numberOfAffectedRows);
                    Console.WriteLine("\n");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Cannot execute  DELETE command: " + e.ToString());
                }

                //Stored Procedure
                var Employees = new DataTable();
                try
                {
                    myReader = null;
                    SqlCommand storedProcedure = new SqlCommand("SalesByCategory", myConnection);
                    storedProcedure.CommandType = CommandType.StoredProcedure;
                    SqlParameter param1 = new SqlParameter("@CategoryName", SqlDbType.NVarChar,15);
                    SqlParameter param2 = new SqlParameter("@OrdYear", SqlDbType.NVarChar,4);
                    param1.Value = "Produce";
                    param2.Value = "1996";
                    storedProcedure.Parameters.Add(param1);
                    storedProcedure.Parameters.Add(param2);
                    myReader = storedProcedure.ExecuteReader();
                   // Employees.Load(myReader);
                    Console.WriteLine("-----Results of Stored procedure-----");
                     while (myReader.Read())
                    {

                        Console.WriteLine("Product Name: " + myReader["ProductName"].ToString());
                        Console.WriteLine("Total Purchase: " + myReader["TotalPurchase"].ToString());
                        Console.WriteLine("\n");
                    }
                     myReader.Close();

                }
                catch (Exception e)
                {
                    Console.WriteLine("Cannot execute  Stored Procedure: " + e.ToString());
                }


                //close connection
                try
                {
                    myConnection.Close();

                    Console.ReadKey();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Connection was not closed. Details: " + e.ToString());
                }
            }
        }
    }
}
