
using Microsoft.EntityFrameworkCore;
using Products.Core.Entities;
using Products.Core.Exceptions;
using Products.Core.Services;
using Products.Infrastructure.Data;
using System;
using Xunit;

namespace Products.Tests
{
    public class ProductTestCase
    {
        /// <summary>
        /// Test if the business rule that verify the max price of a product is working
        /// </summary>
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

        /// <summary>
        /// Test if the business rule that verify if a product exists on database before the update is working
        /// </summary>
        [Fact]
        public void Test_Persistence_Exception_Product_Not_Found_On_Update()
        {
            //create In Memory Database
            var options = new DbContextOptionsBuilder<ERPContext>()
            .UseInMemoryDatabase(databaseName: "ERP")
            .Options;

            using (var context = new ERPContext(options))
            {
                var repository = new EFRepository(context);

                var productService = new ProductsService(repository);

                //Insert an product
                productService.AddProduct(new Product
                {
                    Id = 1,
                    Name = "Ball",
                    Price = 1000,
                    Image = "base64Image"
                });

                Assert.Throws<PersistenceException>(() => productService.UpdateProduct(new Product
                {
                    Id = 2,
                    Name = "Ball",
                    Price = 2000,
                    Image = "base64Image"
                }));
            }
        }

        /// <summary>
        /// Test if the business rule that verify the max size of product image
        /// </summary>
        [Fact]
        public void Test_Business_Exception_Too_Large_Image_File()
        {
            // Create a fake base64file
            string fakeBase64File = new string('*', 1048577); //1MB
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
                    Price = 100,
                    Image = fakeBase64File
                }));
            }
        }
    }
}
