using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using LINQtoCSV;
using CsvDB.Models;
using CsvHelper;
using System.Data;

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
                    //var producParse = csv.GetRecords<Products>();



                    //foreach (var c in producParse)
                    //{
                    //    Products product = new Products();

                    //    product.SKU = c.SKU;
                    //    product.Manufacture = c.Manufacture;
                    //    product.Cost = c.Cost;
                    //    product.Weight = c.Weight;
                    //    product.Size = c.Size;
                    //    product.Title = c.Title;
                    //    product.Shipping = c.Shipping;
                    //    product.Color = c.Color;
                    //    product.Description = c.Description;


                    //    productsList.Add(product);
                    //}

                    
                    ViewBag.Header = header;
                   

                }   
            }
            catch 
            {

                ViewData["Error"] = "Upload filed";
            }

            //List<int> MapList = new List<int>();
            List<Prod> ProdList = new List<Prod>();
            string val = Request.Form["mapSelect"];
            List<int> selInt = new List<int>();
            if (val != null)
            {
                int[] CombInt = val.Split(',').Select(int.Parse).ToArray();


                for (int x = 0; x < CombInt.Length; x++)
                {
                    if (CombInt[x] < 7)
                    {
                        selInt.Add(CombInt[x]);
                    }
                }


                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                var lines = System.IO.File.ReadAllLines(Server.MapPath("~/upload/file.csv")).Skip(1);
                int i = 1;
                foreach (string item in lines)
                {
                    var values = item.Split(',');

                    try
                    {
                        
                        if (values[selInt[0]] != null && values[selInt[1]] != null && values[selInt[2]] != null)
                        {
                            ProdList.Add(new Prod()
                            {
                                Id = i,
                                SKU = values[selInt[0]],
                                brand = values[selInt[1]],
                                price = Convert.ToDecimal(values[selInt[2]]),
                                weight = Convert.ToDecimal(values[selInt[3]]),
                                feature = values[selInt[4]] + " / " + values[selInt[5]],
                                productParameter = values[selInt[6]] + " / " + values[selInt[7]]

                            });
                            i++;
                        }
                }
                    catch
                {
                    ViewData["ErrorData"] = "Wrong Data";
                }
            }

                ProdList.ForEach(Prod => db.Prods.Add(Prod));


                db.SaveChanges();
                db.Dispose();


            }

            return View();
        }
               
    }
}