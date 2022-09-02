using AdoExample.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AdoExample.DAL
{
    public class Product_DAL
    {
        string conString = ConfigurationManager.ConnectionStrings["adoConnectionString"].ToString();

        // get All Products
        public List<Product> GetAllProducts()
        {
            List<Product> productList = new List<Product>();

            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "sp_GetAllProducts";

                SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                DataTable dtProducts = new DataTable();

                connection.Open();
                sqlDA.Fill(dtProducts);
                connection.Close();

                foreach (DataRow dr in dtProducts.Rows)
                {
                    productList.Add(new Product
                    {
                        ProductId = Convert.ToInt32(dr["ProductId"]),
                        ProductName = dr["ProductName"].ToString(),
                        Price = Convert.ToDecimal(dr["Price"]),
                        Qty = Convert.ToInt32(dr["Qty"]),
                        Remarks = dr["Remarks"].ToString()
                    });
                }
            }

            return productList;
        }

        //Insert Products
        public bool InsertProduct(Product product)
        {
            int id = 0;
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = new SqlCommand("sp_InsertProducts",connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProductName", product.ProductName);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@Qty", product.Qty);
                command.Parameters.AddWithValue("@Remarks", product.Remarks);

                connection.Open();
                id = command.ExecuteNonQuery();
                connection.Close();

                return id > 0;

                
            }
        }

        // get Product by Id
        public List<Product> GetAllProductById(int productId)
        {
            List<Product> productList = new List<Product>();

            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "sp_GetProductById";
                command.Parameters.AddWithValue("@ProductId", productId);
                SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                DataTable dtProducts = new DataTable();

                connection.Open();
                sqlDA.Fill(dtProducts);
                connection.Close();

                foreach (DataRow dr in dtProducts.Rows)
                {
                    productList.Add(new Product
                    {
                        ProductId = Convert.ToInt32(dr["ProductId"]),
                        ProductName = dr["ProductName"].ToString(),
                        Price = Convert.ToDecimal(dr["Price"]),
                        Qty = Convert.ToInt32(dr["Qty"]),
                        Remarks = dr["Remarks"].ToString()
                    });
                }
            }

            return productList;
        }

        //Update Products
        public bool UpdateProduct(Product product)
        {
            int i = 0;
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = new SqlCommand("sp_UpdateProduct", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProductId", product.ProductId);
                command.Parameters.AddWithValue("@ProductName", product.ProductName);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@Qty", product.Qty);
                command.Parameters.AddWithValue("@Remarks", product.Remarks);

                connection.Open();
                i = command.ExecuteNonQuery();
                connection.Close();

                return i > 0;


            }
        }

        //Delete Product 
        public string DeleteProduct(int productId)
        {
            string result = "";

            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand command = new SqlCommand("sp_DeleteProduct", con);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@productid", productId);
                command.Parameters.Add("@ReturnMessage",SqlDbType.VarChar,50).Direction = ParameterDirection.Output;

                con.Open();
                command.ExecuteNonQuery();
                result = command.Parameters["@ReturnMessage"].Value.ToString();
                con.Close();
            }

            return result;
        }
    }
}