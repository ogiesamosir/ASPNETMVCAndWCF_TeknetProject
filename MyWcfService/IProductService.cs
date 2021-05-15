using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace MyWcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IProductService" in both code and config file together.
    [ServiceContract]
    public interface IProductService
    {
        [OperationContract]
        List<Product> findAll();

        [OperationContract]
        List<Product> find(int id);

        [OperationContract]
        List<DetailProduct> GetProductDetails(string ProductName);

        [OperationContract]
        List<Product> findByDate(DateTime CreationDate);

        [OperationContract]
        string InsertProductDetail(DetailProduct ProductDetail);

        [OperationContract]
        bool UpdateProduct(DetailProduct ProductID);

        [OperationContract]
        bool DeleteProduct(int ProductInfo);


    }

    [DataContract]
    public class DetailProduct
    {
        int id , quantity;
        string name = string.Empty;
        decimal price;
        DateTime creationDate;
        
        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [DataMember]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [DataMember]
        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        [DataMember]
        public decimal Price
        {
            get { return price; }
            set { price = value; }
        }

        [DataMember]
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
        }


    }


}
