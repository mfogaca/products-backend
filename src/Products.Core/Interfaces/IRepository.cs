using System;
using System.Collections.Generic;
using System.Text;
using Products.Core.Entities;

namespace Products.Core.Interfaces
{
    public interface IRepository
    {
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(int id);
        Product GetProduct(int id);
        List<Product> GetProducts();
    }
}
