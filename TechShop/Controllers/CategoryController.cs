using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TechShop.DAL;

namespace TechShop.Controllers
{
    public class CategoryController : BaseController
    {
        MyAppContext db = new MyAppContext();
        // GET: CategoryPartial
        public ActionResult Index()
        {
            return View();
        }
        //Category
        public PartialViewResult CategoryPartial()
        {
            var categoryList = db.Categories.OrderBy(m => m.Category_Name).ToList();
            return PartialView(categoryList);
        }
    }
}