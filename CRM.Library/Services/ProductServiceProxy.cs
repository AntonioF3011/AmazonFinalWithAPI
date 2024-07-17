using CRM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
using CRM.Library.Utilities;
using CRM.Library.DTO;
namespace CRM.Library.Services
{
    public class ProductServiceProxy
    {
        private List<ProductDTO> products;

        private ProductServiceProxy()
        {
     
            //todo: make a web call
            var response =  new WebRequestHandler().Get("/Inventory").Result;
           products = JsonConvert.DeserializeObject<List<ProductDTO>>(response);//here you are expecting response to
                                                                                //return a list of products
        }


        private static ProductServiceProxy? instance;
        private static readonly object instanceLock = new object();
         
        public static ProductServiceProxy Current
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ProductServiceProxy();
                    }

                    return instance;
                }
            }
        }

        public ReadOnlyCollection<ProductDTO> Products
        {
            get
            {
                return products.AsReadOnly();
            }
        }

        public async Task<IEnumerable<ProductDTO>> Get()
        {
            var result = await new WebRequestHandler().Get("/Inventory");
            var deserializedResult = JsonConvert.DeserializeObject<List<ProductDTO>>(result);
            products = deserializedResult?.ToList() ?? new List<ProductDTO>();
            return products;

        }


        public async Task<ProductDTO> AddOrUpdate(ProductDTO p)
        {
            
            var result = await new WebRequestHandler().Post("/Inventory",p);
            return JsonConvert.DeserializeObject<ProductDTO>(result); 
        }
        
        
        public async Task<ProductDTO> Delete(int id)
        {
            //var productToDelete = Products.FirstOrDefault(p => p.Id == id);
            //if (productToDelete == null)
            //{
            //    return null;
            //}
            //products.Remove(productToDelete);
            var response = await new WebRequestHandler().Delete($"/{id}");
            var productToDelete = JsonConvert.DeserializeObject<ProductDTO>(response);
            return productToDelete;
        }

        //establish markdown
        public void ApplyMarkdown(ProductDTO p)
        {
            if (p.Markdown > 100 || p.Markdown < 0)
            {
                return; 
            }
            p.Price = p.Price * (1 - (p.Markdown/100));

        }
    }
}
