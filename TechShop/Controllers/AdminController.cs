using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TechShop.DAL;
using TechShop.Models;
using Newtonsoft.Json;
using PagedList.Mvc;
using System.IO;
using System.Data.Entity;
using TechShop.ViewModel;
using System.Net;
using PagedList;
using TechShop.Models.Home;

namespace TechShop.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        MyAppContext db = new MyAppContext();
        //GET: Admin
        public List<SelectListItem> GetCategory()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var cat = db.Categories.ToList();
            foreach (var item in cat)
            {
                list.Add(new SelectListItem { Value = item.Category_Id.ToString(), Text = item.Category_Name });
            }
            return list;

        }
      

        // Admin Add+Update category


        public ActionResult Categories()
        {
            List<Category> categories = db.Categories.ToList();

            return View(categories);
        }
        [HttpGet]
        public ActionResult AddCategory()
        {
            return View();
        }

        public ActionResult UpdateCategory(int catID)
        {
            var id = db.Categories.Find(catID);
            return View();
        }

        //AddOrUpdate Category
        [HttpPost]
        public ActionResult AddOrEditCatgory(Category category)
        {
            if (ModelState.IsValid)
            {
                Category obj = new Category();
                var isExist = db.Categories.Where(m => m.Category_Name == category.Category_Name).Any();

                obj.Category_Id = category.Category_Id;
                obj.Category_Name = category.Category_Name;
                if (category.Category_Id == 0)
                    if (!isExist)
                    {
                        db.Categories.Add(obj);
                        db.SaveChanges();
                        return Redirect("Categories");

                    }
                    else
                    {
                        ViewBag.error = String.Format("This Category is Exist!");
                    }
                else
                    if (!isExist)
                {
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                    return Redirect("Categories");

                }
                else
                {
                    ViewBag.error = String.Format("This Category is Exist!");
                }
            }
            if (category.Category_Id == 0)
            {
                return View("AddCategory", category);
            }
            return View("UpdatCategory", category);


        }


        public ActionResult DeleteCategory(int catId)
        {
            var catDel = db.Categories.Where(c => c.Category_Id == catId).FirstOrDefault();
            db.Categories.Remove(catDel);
            db.SaveChanges();

            return Redirect("Categories");

        }



        //Amidn Add+Update product
        public ActionResult Product()
        {
            List<Product> products = db.Products.ToList();
            return View(products);
        }
       
        [HttpGet]
        public ActionResult AddProduct()
        {

            ViewBag.CategoryList = GetCategory();
            return View();

        }
        [HttpPost]
        public ActionResult AddOrEditProduct(Product product, HttpPostedFileBase file)
        {
            string pic = null;
            if (file != null)
            {
                pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/FileImage/ProductImage/"), pic);
                file.SaveAs(path);
            }
            product.Image = pic;



            if (ModelState.IsValid)
            {
                Product obj = new Product();
                var isExist = db.Products.Where(m => m.Product_Name == product.Product_Name).Any();

                obj.Image = pic;
                obj.Quantity = product.Quantity;
                obj.Product_Id = product.Product_Id;
                obj.Product_Name = product.Product_Name;
                obj.Describe = product.Describe;
                obj.Price = product.Price;
                obj.Category_Id = product.Category_Id;
                if (product.Product_Id == 0)
                    if (!isExist)
                    {
                        db.Products.Add(obj);
                        db.SaveChanges();
                        return Redirect("Product");

                    }
                    else
                    {
                        ViewBag.error = String.Format("This Product is Exist!");
                    }
                else
                {
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                    return Redirect("Product");

                }
            }
            if (product.Product_Id == 0)
            {
                return View("AddProduct", product);
            }
            return View("UpdateProduct", product);

        }
        public ActionResult UpdateProduct(int proID, HttpPostedFileBase file, Product product)
        {
            var pr = db.Products.Where(m => m.Product_Id == proID).FirstOrDefault();
            Product obj = new Product();
            string pic = null;
             obj.Image = pic;
          
            
            if (file != null)
            {
                pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/FileImage/ProductImage/"), pic);
                file.SaveAs(path);
            }
            product.Image = pic;
            db.Products.Add(obj);
            //db.SaveChanges();

            return View(pr);
        }

        public ActionResult DeleteProduct(int proID)
        {
            var proDel = db.Products.Where(c => c.Product_Id == proID).FirstOrDefault();
            db.Products.Remove(proDel);
            db.SaveChanges();

            return Redirect("Product");

        }



        public List<SelectListItem> RoleList()
        {
            List<SelectListItem> role = new List<SelectListItem>();
            var roleList = db.Roles.ToList();
            foreach (var item in roleList)
            {
                role.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Name });
            }
            return role;
        }
        //Admin Manage Account
        public ActionResult Account(int? page)
        {
            
            var role = db.Roles.ToList();
            ViewBag.role = role;
            IPagedList<Account_Cus> acc = db.Account_Cuss.ToList().ToPagedList(page ?? 1, 10);
            return View(acc);
        }
        public ActionResult AccountCreate()
        {
            ViewBag.Roles = RoleList();
            return View();
        }

      
        //POST: Create Account
        [HttpPost]
        public ActionResult AccountCreate(Account_Cus account)
        {
            ViewBag.Roles = RoleList();
            if (ModelState.IsValid)
            {
                Account_Cus accObj = new Account_Cus();

                accObj.Cus_Id = account.Cus_Id;
                accObj.RoleName =account.RoleName;
                accObj.RoleId = account.RoleId;
                accObj.Username = account.Username;
                accObj.Password = account.Password;
                accObj.Name = account.Name;
                accObj.DOB = account.DOB;
                accObj.Tel = account.Tel;
                accObj.Email = account.Email;

                ////upload avatar for account
                //string img = Path.GetFileName(avatar.FileName);
                //string pathImg = Path.Combine(Server.MapPath("~/Files/Avatar/"), img);
                //avatar.SaveAs(pathImg);
                //accObj.Avatar = img;

                var usernameExist = db.Account_Cuss.Where(m => m.Username == account.Username).Any();
                var emailExist = db.Account_Cuss.Where(m => m.Email == account.Email).Any();
                if (!usernameExist && !emailExist)
                {
                    db.Account_Cuss.Add(accObj);
                    db.SaveChanges();
                }
                else if (emailExist)
                {
                    ViewBag.emailErr = String.Format("This Email is already exist!");
                    return View();
                }
                else if (usernameExist)
                {
                    ViewBag.usernameErr = String.Format("This Username is already exist!");
                    return View();
                }
                return Redirect("Account");
            }

            return View(account);
        }

        //GET: Edit Account page
       
        public ActionResult AccountEdit(int accountId)
        {
            AdminEditAccViewModel model = new AdminEditAccViewModel();

            var obj = db.Account_Cuss.Where(m => m.Cus_Id == accountId).FirstOrDefault();
            model.Cus_Id = obj.Cus_Id;
            //model.NewRole = obj.RoleId;
            model.Username = obj.Username;
            model.NewPassword = obj.Password;

            ViewBag.Roles = RoleList();


            return View(model);
        }

        //POST: Edit Account
        [HttpPost]
        public ActionResult AccountEdit(AdminEditAccViewModel model)
        {
            ViewBag.Roles = RoleList();
            if (ModelState.IsValid)
            {
                var obj = db.Account_Cuss.Where(m => m.Cus_Id == model.Cus_Id).FirstOrDefault();
                obj.RoleId = model.NewRole;
                obj.Password = model.NewPassword;

                db.Entry(obj).State = EntityState.Modified;
                db.SaveChanges();

                return Redirect("Account");
            }
            return View(model);
        }

        //delete Account
        public ActionResult AccountDelete(int accId)
        {
            var accDel = db.Account_Cuss.Where(a => a.Cus_Id == accId).FirstOrDefault();
            db.Account_Cuss.Remove(accDel);
            db.SaveChanges();

            return Redirect("Account");
        }


        //Show the list order

        HomeModels model = new HomeModels();
        public ActionResult Order(string searchString, int? page )
        {
            ViewBag.searchString = searchString;
            var statusName = db.Statuses.ToList();
            ViewBag.stsList = statusName;         
            //IPagedList<Order> acc = db.Orders.Where(m => m.Name.Contains(searchString) || searchString == null).ToList().ToPagedList(page ?? 1, 10);
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



        //Show the detail of each orders
        public ActionResult Details (int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order = db.Orders.Find(id);
            ViewBag.GetStatus = GetStatus();

            var coupon = db.Coupons.Where(m => m.VoucherCode == order.Voucher).FirstOrDefault();
            ViewBag.coupon = coupon;

            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        [HttpPost]
        public ActionResult SaveStatus(Order order)
        {
            var obj = db.Orders.FirstOrDefault(m => m.Order_Id == order.Order_Id);

            obj.Status_Id = order.Status_Id;

            //db.Entry(obj).State = EntityState.Modified;
            db.SaveChanges();
            return Redirect("Order");
        }

     


        // Select proccess of each order
        [HttpPost]
        public ActionResult select1(OrderDetail orderDetail)
        {

            ViewBag.StatusList = GetStatus();
            OrderDetail xyz = new OrderDetail();
            xyz.Status_Id = orderDetail.Status_Id;
            db.orderDetails.Add(xyz);
            db.SaveChanges();
            return View();

        }
        public ActionResult savechange(OrderDetail orderDetail)
        {
            OrderDetail xyz = new OrderDetail();
            xyz.Status.Status_Id = orderDetail.Status.Status_Id;
            db.orderDetails.Add(xyz);
            db.SaveChanges();
            return View();
        }

        public List<SelectListItem> GetStatus()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var sta = db.Statuses.ToList();
            foreach (var item in sta)
            {
                list.Add(new SelectListItem { Value = item.Status_Id.ToString(), Text = item.Status_Name });
            }
            return list;

        }
        //Admin get commnent
        public ActionResult AdminComment(CommentViewModel model)
        {
            if (ModelState.IsValid)
            {
                Comment obj = new Comment();
                obj.Contents = model.Message;
                obj.Admin_Id = model.Admin_Id;
                obj.Product_Id = model.Product_Id;
             

                db.Comments.Add(obj);
                db.SaveChanges();

                return RedirectToAction("Details", new { contriId = model.Product_Id, numb = model.Number });
            }
            return View();
        }
        //Admin get Dashboard
        public ActionResult Dashboard()
        {
            var prod = db.Products.ToList();
            List<SellestViewModel> list = new List<SellestViewModel>();
            foreach (var item in prod)
            {
                var orders = db.orderDetails.Where(m => m.Product_Id == item.Product_Id).ToList();
                list.Add(new SellestViewModel { name = orders.Where(m => m.Product_Id == item.Product_Id).Select(m => m.Product.Product_Name).FirstOrDefault(),Quantity = orders.Sum(m => m.Quantity)});
            }

            List<string> listName = new List<string>();
            List<int> listQnty = new List<int>();

            foreach (var item in list.OrderByDescending(m => m.Quantity).Take(5))
            {
                listName.Add(item.name);
                listQnty.Add(item.Quantity);
            }

            ViewBag.name = JsonConvert.SerializeObject(listName);
            ViewBag.qnty = JsonConvert.SerializeObject(listQnty);

            var orderMonth = new List<string>();
            var orderCount = new List<int>();
            List<string> month = new List<string>();
            month.Add("None");
            month.Add("January");
            month.Add("February");
            month.Add("March");
            month.Add("April");
            month.Add("May");
            month.Add("June");
            month.Add("July");
            month.Add("August");
            month.Add("September");
            month.Add("October");
            month.Add("November");
            month.Add("December");
            for (int i = 1; i < month.Count; i++)
            {
                orderMonth.Add(month.ElementAt(i));
                orderCount.Add(db.Orders.Where(m => m.SubmitDate.Month == i).Count());
            }

            ViewBag.orderMonth = JsonConvert.SerializeObject(orderMonth);
            ViewBag.orderCount = JsonConvert.SerializeObject(orderCount);

            
            return View();

        }





    }
}