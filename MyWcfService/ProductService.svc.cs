using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace MyWcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ProductService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ProductService.svc or ProductService.svc.cs at the Solution Explorer and start debugging.
    public class ProductService : IProductService
    {
        private MyDemoEntities mde = new MyDemoEntities();

        SqlConnection con = new SqlConnection("Data Source=DESKTOP-RJN5CGS;Initial Catalog=mydemo;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework");

        public bool DeleteProduct(int ProductInfo)
        {
            con.Open();
            string query = "delete from Product where Id = @Id";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Id", ProductInfo);
            int res = cmd.ExecuteNonQuery();
            con.Close();
            if(res == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Product> find(int id)
        {
            return mde.Products.Where(p => p.Id == id).ToList();
        }

        public List<Product> findAll()
        {
            return mde.Products.ToList();
        }

        public List<Product> findByDate(DateTime CreationDate)
        {
            return mde.Products.Where(p=>p.CreationDate.Value == CreationDate).ToList();
        }

        public List<DetailProduct> GetProductDetails(string ProductName)
        {
            List<DetailProduct> productDetails = new List<DetailProduct>();
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select * from Product where Name Like '%'+@Name+'%' ", con);
                cmd.Parameters.AddWithValue("@Name", ProductName);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if(dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DetailProduct productInfo = new DetailProduct();
                        productInfo.Id = Convert.ToInt32(dt.Rows[i]["Id"].ToString());
                        productInfo.Name = dt.Rows[i]["Name"].ToString();
                        productInfo.Price = Convert.ToDecimal(dt.Rows[i]["Price"].ToString());
                        productInfo.Quantity = Convert.ToInt32(dt.Rows[i]["Quantity"].ToString());
                        productInfo.CreationDate = Convert.ToDateTime(dt.Rows[i]["CreationDate"].ToString());

                        productDetails.Add(productInfo);
                    }
                }
                con.Close();
            }
            return productDetails;
        }

        public string InsertProductDetail(DetailProduct ProductDetail)
        {
            string strMessage = string.Empty;
            con.Open();
            SqlCommand cmd = new SqlCommand("insert into Product (Id , Name, Price , Quantity , CreationDate) values (@Id, @Name, @Price, @Quantity, @CreationDate)", con);
            cmd.Parameters.AddWithValue("@Id", ProductDetail.Id);
            cmd.Parameters.AddWithValue("@Name", ProductDetail.Name);
            cmd.Parameters.AddWithValue("@Price", ProductDetail.Price);
            cmd.Parameters.AddWithValue("@Quantity", ProductDetail.Quantity);
            cmd.Parameters.AddWithValue("@CreationDate", ProductDetail.CreationDate);

            int result = cmd.ExecuteNonQuery();
            if(result == 1)
            {
                strMessage = ProductDetail.Name + "sukses";
            }
            else
            {
                strMessage = ProductDetail.Name + "gagal";
            }
            con.Close();
            return strMessage;

        }

        public bool UpdateProduct(DetailProduct ProductID)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("update Product set  Name = @Name , Price = @Price , Quantity = @Quantity , CreationDate = @CreationDate where Id = @Id", con);
            cmd.Parameters.AddWithValue("@Id", ProductID.Id);
            cmd.Parameters.AddWithValue("@Name", ProductID.Name);
            cmd.Parameters.AddWithValue("@Price", ProductID.Price);
            cmd.Parameters.AddWithValue("@Quantity", ProductID.Quantity);
            cmd.Parameters.AddWithValue("@CreationDate", ProductID.CreationDate);

            int res = cmd.ExecuteNonQuery();
            con.Close();
            if(res == 1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
