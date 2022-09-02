using AdoExample.DAL;
using AdoExample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdoExample.Controllers
{
    public class ProductController : Controller
    {
        private readonly Product_DAL _product_DAL;

        public ProductController()
        {
            _product_DAL = new Product_DAL();
        }
        // GET: Product
        public ActionResult Index()
        {
            var productList = _product_DAL.GetAllProducts();
            if (productList.Count == 0)
                TempData["InfoMessage"] = "Currently products not available in the Database.";
            return View(productList);
        }

        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Product/Create
        [HttpGet]
        public ActionResult Create()
        {
           
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(Product product)
        {
            try
            {
                bool IsInserted = false;
                if (ModelState.IsValid)
                {
                    IsInserted = _product_DAL.InsertProduct(product);

                    if(IsInserted)
                    {
                        TempData["SuccessMessage"] = "Product Details saved succesfully ... !";
                    } else
                    {
                        TempData["ErrorMessage"] = "Unable to save the product .";
                    }

                }

                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        // GET: Product/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var product = _product_DAL.GetAllProductById(id).FirstOrDefault();
            if (product is null)
            {
                TempData["InfoMessage"] = "Product not available With Id " + id.ToString();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost , ActionName("Edit")]
        public ActionResult Edit(Product product)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    var isUpdated = _product_DAL.UpdateProduct(product);
                    if (isUpdated) {
                        TempData["SuccessMessage"] = "Product details Updated succefully";
                    }
                    else {
                        TempData["ErrorMessage"] = "Product details unable To Update";
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var product = _product_DAL.GetAllProductById(id).FirstOrDefault();
                if (product is null)
                {
                    TempData["InfoMessage"] = "Product not available With Id " + id.ToString();
                    return RedirectToAction("Index");
                }
                return View(product);
            }
            catch (Exception ex)
            {

                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        // POST: Product/Delete/5
        [HttpPost , ActionName("Delete")]
        public ActionResult DeleteConfirmation(int id)
        {
            try
            {
                string result = _product_DAL.DeleteProduct(id);
                if(result.Contains("Deleted"))
                {
                    TempData["SuccessMessage"] = result;
                }
                else
                {
                    TempData["ErrorMessage"] = result;
                }
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }
    }
}
