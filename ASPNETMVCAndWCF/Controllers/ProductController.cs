using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ASPNETMVCAndWCF.Models;
using ASPNETMVCAndWCF.ProductService;

namespace ASPNETMVCAndWCF.Controllers
{
    public class ProductController : Controller
    {

        private ProductServiceClient psc = new ProductServiceClient();

        ProductService.DetailProduct dp = new ProductService.DetailProduct();


        public ActionResult Index()
        {
            ViewBag.listProducts = psc.findAll();
            return View();
        }

        public ActionResult Tambah()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Tambah(FormCollection form)
        {
            if(form["Id"] != null && form["Name"] != null && form["Price"] != null && form["Quantity"] != null )
            {
                dp.Name = form["Name"];
                dp.Price = Convert.ToDecimal(form["Price"]);
                dp.Quantity = Convert.ToInt32(form["Quantity"]);
                dp.CreationDate = Convert.ToDateTime(form["CreationDate"]);
                dp.Id = Convert.ToInt32(form["Id"]);

                psc.InsertProductDetail(dp);

                return RedirectToAction("Index");
            }

            return View();
        }

        public ActionResult Search(FormCollection fc)
        {
            string condition = fc["condition"];
            string keyword = fc["keyword"];
            if (condition.Equals("byID"))
                ViewBag.listProducts = psc.find(Convert.ToInt32(keyword));
            else
                ViewBag.listProducts = psc.findByDate(Convert.ToDateTime(keyword));
            return View("Index");
        }

        public ActionResult Cari(FormCollection fc)
        {
            string condition = fc["condition"];
            string keyword = fc["keyword"];
            if (condition.Equals("byID"))
                ViewBag.listProducts = psc.find(Convert.ToInt32(keyword));
            else
                ViewBag.listProducts = psc.findByDate(Convert.ToDateTime(keyword));
            return View("Dashboard");
        }

        public Product GetByName(string name)
        {
            Product product = new Product();
            foreach(var s in psc.GetProductDetails(name).ToList())
            {
                product.Id = s.Id;
                product.Name = s.Name;
                product.Price = s.Price;
                product.Quantity = s.Quantity;
                product.CreationDate = s.CreationDate;
            }
            return product;
        }

        public ActionResult Delete(string name)
        {
            return View(GetByName(name));
        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {
            psc.DeleteProduct(Id);
            return RedirectToAction("Index");
        }

        public ActionResult Update(string name)
        {
            if( name != null)
            {
                return View(GetByName(name));
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Update(FormCollection form)
        {
            if ( form["Name"] != null && form["Price"] != null && form["Quantity"] != null && form["CreationDate"] != null)
            {
                dp.Name = form["Name"];
                dp.Price = Convert.ToDecimal(form["Price"]);
                dp.Quantity = Convert.ToInt32(form["Quantity"]);
                dp.CreationDate = Convert.ToDateTime(form["CreationDate"]);
                dp.Id = Convert.ToInt32(form["Id"]);
                psc.UpdateProduct(dp);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Dashboard()
        {
            ViewBag.listProducts = psc.findAll();
            return View();
        }


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(tblLogin model ,string returnUrl)
        {
            LoginEntities db = new LoginEntities();
            var dataItem = db.tblLogins.Where(x => x.username == model.username && x.password == model.password).First();
            if(dataItem != null)
            {
                FormsAuthentication.SetAuthCookie(dataItem.username, false);
                if(Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/") && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    if(dataItem.role == "Admin")
                    {
                        return RedirectToAction("Dashboard");
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid user/pass");
                return View();
            }
        }

        [Authorize]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Product");
        }

    }
}