using Microsoft.EntityFrameworkCore;
using Products.Core.Entities;
using Products.Core.Exceptions;
using Products.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Products.Infrastructure.Data
{
    public class EFRepository : IRepository
    {
        protected readonly ERPContext _dbContext;

        public EFRepository(ERPContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddProduct(Product product)
        {
            _dbContext.Set<Product>().Add(product);
            _dbContext.SaveChanges();
        }

        public void DeleteProduct(int id)
        {
            var product = new Product() { Id = id };
            _dbContext.Set<Product>().Remove(product);
            _dbContext.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            var persistedProduct = _dbContext.Product.First(p => p.Id == product.Id);
            if (persistedProduct == null)
            {
                throw new PersistenceException("Product not found, id : " + product.Id);
            }
            //We will update only the name and the price
            persistedProduct.Name = product.Name;
            persistedProduct.Price = product.Price;
            _dbContext.SaveChanges();
        }

        public Product GetProduct(int id)
        {
            var keyValues = new object[] { id };
            return  _dbContext.Set<Product>().Find(keyValues);
        }


        public List<Product> GetProducts()
        {
            return _dbContext.Set<Product>().ToListAsync().Result;
        }
    }
}
