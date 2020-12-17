using Microsoft.Extensions.Logging;
using Products.Core.Entities;
using Products.Core.Exceptions;
using Products.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Products.Core.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IRepository _repository;

        public ProductsService(IRepository repository)
        {
            this._repository = repository;
        }

        public void AddProduct(Product product)
        {
            //Example on how to throw errors for expected behaviors
            if (product.Price > 100000)
            {
                throw new BusinessException("The price of product must be lower then 100,000.00");
            }
            _repository.AddProduct(product);
        }

        public void UpdateProduct(Product product)
        {
            _repository.UpdateProduct(product);
        }

        public void DeleteProduct(int id)
        {
            _repository.DeleteProduct(id);
        }

        public List<Product> GetProducts()
        {
            return _repository.GetProducts();
        }

        public Product GetProduct(int id)
        {
            return _repository.GetProduct(id);
        }
    }
}

