using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using LINQtoCSV;
using CsvDB.Models;
using CsvHelper;

namespace CsvDB.Controllers
{
    public class HomeController : Controller
    {
        private MyDBEntities db = new MyDBEntities();

        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Index(HttpPostedFileBase upload)
        {

            string path = null;
            List<Products> productsList = new List<Products>();
            try
            {
                if (upload.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(upload.FileName);
                    path = AppDomain.CurrentDomain.BaseDirectory + "upload\\" + fileName;
                    upload.SaveAs(path);

                    var csv = new CsvReader(new StreamReader(path));
                    csv.Read();
                    csv.ReadHeader();
                    var header = csv.Context.HeaderRecord;
                    var producParse = csv.GetRecords<Products>();



                    foreach (var c in producParse)
                    {
                        Products product = new Products();

                        product.SKU = c.SKU;
                        product.Manufacture = c.Manufacture;
                        product.Cost = c.Cost;
                        product.Weight = c.Weight;
                        product.Size = c.Size;
                        product.Title = c.Title;
                        product.Shipping = c.Shipping;
                        product.Color = c.Color;
                        product.Description = c.Description;


                        productsList.Add(product);
                    }
                    

                     ViewBag.Header = header;
                }   
            }
            catch 
            {

                ViewData["Error"] = "Upload filed";
            }

            String val = Request.Form["mapSelect"];
            
            return View();
        }
               
    }
}