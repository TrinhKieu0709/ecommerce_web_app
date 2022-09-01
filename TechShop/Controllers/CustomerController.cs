using System;
using TechShop.EmailForm;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TechShop.DAL;
using TechShop.Models;
using TechShop.ViewModel;
using PagedList;
using PagedList.Mvc;

namespace TechShop.Controllers
{
    public class CustomerController : Controller
    {
        //HomeModels model = new HomeModels();
        MyAppContext db = new MyAppContext();
        HomeModels model = new HomeModels();

        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }






        [Authorize]
        public ActionResult CusOrders(string searchString, int? page)
        {
            ViewBag.searchString = searchString;
            var order = db.Orders.Where(m => m.Username == User.Identity.Name).ToList();
            var status = db.Statuses.ToList();
            ViewBag.status = status;
            return View(model.SearchString(searchString, page));

        }

        //Paging
        public IEnumerable<Order> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Order> model = db.Orders;

            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Username.Contains(searchString) || x.Name.Contains(searchString));
            }
            return model.OrderByDescending(x => x.Username).ToPagedList(page, pageSize);

        }


        //Detail Customer's detail
        [Authorize]
        public ActionResult CusOrderDetails(int order_id)
        {
            var order = db.Orders.FirstOrDefault(m => m.Order_Id == order_id);
            var status = db.Statuses.FirstOrDefault(m => m.Status_Id == order.Status_Id);
            ViewBag.status = status.Status_Name;
            var coupon = db.Coupons.Where(m => m.VoucherCode == order.Voucher).FirstOrDefault();
            ViewBag.coupon = coupon;
            return View(order);
        }
        // Customer Submit Comment
        [HttpPost]
        public ActionResult CommentSubmit(CommentViewModel model)
        {
            if (ModelState.IsValid)
            {
                Comment obj = new Comment();

                obj.Contents = model.Message;
                obj.Product_Id = model.Product_Id;
                obj.Admin_Id = model.Admin_Id;
                obj.Cus_Id = model.Cus_Id;
                db.Comments.Add(obj);
                db.SaveChanges();

                return RedirectToAction("Details", "Home", new { id = model.Product_Id });
            }
            return View();
        }

        public ActionResult ViewContributionAndComment(int producttId, int numb)
        {
            CommentViewModel model = new CommentViewModel();
            var product = db.Products.Find(producttId);
            var comment = db.Comments.Where(m => m.Product_Id == producttId).ToList();

            model.Number = numb;
            model.Product_Id = product.Product_Id;
            model.Cus_Id = product.Account_Cus.Cus_Id;

            List<Comment> list = new List<Comment>();
            foreach (var item in comment)
            {
                list.Add(item);
            }
            ViewBag.list = list;

            return View(model);
        }
        [HttpPost]
        public ActionResult CommnentProduct(CommentViewModel model)
        {
            if (ModelState.IsValid)
            {
                Comment obj = new Comment();

                obj.Contents = model.Message;
                obj.Product_Id = model.Product_Id;
                obj.Cus_Id = model.Cus_Id;


                db.Comments.Add(obj);
                db.SaveChanges();

                return RedirectToAction("Details", "Home", new { contriId = model.Product_Id, numb = model.Number });
            }
            return View();
        }





    }
}
