using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CRM.Models;
namespace Amazon.API.Database
{
    public class Filebase
    {
        private string _root;
        private static Filebase _instance;


        public static Filebase Current
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new Filebase();
                }

                return _instance;
            }
        }
        public int NextProductId
        {
            get
            {
                if (!Products.Any())
                {
                    return 1;
                }

                return Products.Max(p => p.Id) + 1;
            }
        }
        private Filebase()
        {
            _root = @"C:\temp\Products"; 

        }

        public Products AddOrUpdate(Products p)
        {
            //set up a new Id if one doesn't already exist
            if(p.Id<=0)
            {
                p.Id = NextProductId;
            }

            //go to the right place]
            string path = $"{_root}\\{p.Id}.json"; 

            //if the item has been previously persisted
            if(File.Exists(path))
            {
                //blow it up
                File.Delete(path);
            }

            //write the file
            File.WriteAllText(path, JsonConvert.SerializeObject(p));

            //return the item, which now has an id
            return p;
        }

        public List<Products> Products
        {
            get
            {
                var root = new DirectoryInfo(_root);
                var _prods = new List<Products>();
                foreach (var appFile in root.GetFiles())
                {
                    var prod = JsonConvert.DeserializeObject<Products>(File.ReadAllText(appFile.FullName));
                    if (prod != null)
                    {
                        _prods.Add(prod);
                    }
                }
                return _prods;
            }
        }

        public Products Delete( int id)
        {
            //TODO: refer to AddOrUpdate for an idea of how you can implement this.
            throw new NotImplementedException();
        }
    }
}
