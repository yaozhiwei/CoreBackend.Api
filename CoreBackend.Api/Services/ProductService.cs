using CoreBackend.Api.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBackend.Api.Services
{
    public class ProductService
    {
        public static ProductService Current { get; } = new ProductService();
        public List<Product> Products { get; }


        private ProductService()
        {
        }
    }
}
