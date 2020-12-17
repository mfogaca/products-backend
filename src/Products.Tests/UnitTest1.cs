
using Microsoft.EntityFrameworkCore;
using Products.Core.Entities;
using Products.Core.Exceptions;
using Products.Core.Services;
using Products.Infrastructure.Data;
using System;
using Xunit;

namespace Products.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test_Business_Exception_High_Price()
        {
            //create In Memory Database
            var options = new DbContextOptionsBuilder<ERPContext>()
            .UseInMemoryDatabase(databaseName: "ERP")
            .Options;

            using (var context = new ERPContext(options))
            {
                var repository = new EFRepository(context);

                var productService = new ProductsService(repository);

                Assert.Throws<BusinessException>(() => productService.AddProduct(new Product
                {
                    Id = 1,
                    Name = "Ball",
                    Price = 10000000,
                    Image = "base64Image"
                }));
            }
        }
    }
}
