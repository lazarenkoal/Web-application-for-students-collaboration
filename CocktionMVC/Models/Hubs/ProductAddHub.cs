using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using CocktionMVC.Models.DAL;
using Microsoft.AspNet.Identity.EntityFramework;
namespace CocktionMVC.Models.Hubs
{
    public class ProductAddHub : Hub
    {

        public void AddProductToTheAuction(string name)
        {
            Clients.All.addNewBidToTheAuction(name);
        }

        public void SaveInTheDb(string name, string description, string category, HttpPostedFileBase file)
        {
            Product product = new Product();

            product.Name = name;
            product.Description = description;
            product.Category = category;

            Photo photo = new Photo();
            if (file != null)
            {
                string pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine("http://localhost:2274/Images/", pic);
                // file is uploaded
                file.SaveAs(path);
                photo.FileName = pic;
                photo.FilePath = path;
                photo.Product = product;
            }
            product.Photos.Add(photo);

            var db = new CocktionContext();
            db.Products.Add(product);
            db.SaveChanges();
        }
    }
}