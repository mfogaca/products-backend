using Products.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Products.Core.Interfaces
{
    public interface IProductsService
    {
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(int id);
        List<Product> GetProducts();
        Product GetProduct(int id);
    }
}
