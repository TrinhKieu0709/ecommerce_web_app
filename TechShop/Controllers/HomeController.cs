using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TechShop.DAL;
using TechShop.Models;
using System.Data.Entity;
using TechShop.Models.Home;
using System.Web.Security;
using TechShop.ViewModel;
using System.Configuration;
using Facebook;
using PagedList;
using System.Net;
using System.IO;
using TechShop.EmailForm;
//using System.Web.Security;
using System.Web.UI.WebControls;

namespace TechShop.Controllers
{
    public class HomeController : Controller
    {
        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FaceBookCallBack");
                return uriBuilder.Uri;
            }
        } 
        private MyAppContext db = new MyAppContext();
        private Random random = new Random();
        private HomeModels model = new HomeModels();
        public string RandomString(int length)
        {
            const string chars = "0123456789QWEWRTYUIOPASDFGHJKLLZXCVBNMqwertyuiopasdfghjklzxcvbnm";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
        //Customer Profile
        public ActionResult Profile()
        {
            var user = db.Account_Cuss.Where(m => m.Username == User.Identity.Name).FirstOrDefault();
            return View(user);
        }
        // Get : Customer Change Profile
        [Authorize]
        [HttpGet]
        public ActionResult ChangeProfile()
        {
            UpdateProfileViewModel model = new UpdateProfileViewModel();
            var user = db.Account_Cuss.Where(m => m.Username == User.Identity.Name).FirstOrDefault();
            model.Name = user.Name;
            //model.DOB = user.DOB;
            model.Email = model.Email;
            model.Tel = model.Tel;
            model.Avatar = model.Avatar;
            return View(model);
        }
        [Authorize]
        [HttpPost]
        //Post : Customer Change Profile
        public ActionResult ChangeProfile( UpdateProfileViewModel model, HttpPostedFileBase avatar)
        {
            var user = db.Account_Cuss.Where(m => m.Username == User.Identity.Name).FirstOrDefault();
            if (ModelState.IsValid)
            {
                user.Name = model.Name;
                //user.DOB = model.DOB;
                model.Tel = model.Tel;
                //Customer Change Avater Profile
                if (avatar != null)
                {
                    var img = Path.GetFileName(avatar.FileName);
                    var pathImg = Path.Combine(Server.MapPath("~/FileImage/ProfileImage/"), img);
                    user.Avatar = img;
                }
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();

                return Redirect("Profile");

            }
            user.Avatar = model.Avatar;
            user.Name = model.Name;
            return View("ChangeProfile", model);
        }

        public PartialViewResult CategoryPartial()
        {
            var categoryList = db.Categories.OrderBy(m => m.Category_Name).ToList();
            return PartialView(categoryList);
        }

        public ActionResult SearchProduct(string searchString)
        {
            var cat = db.Categories.ToList();
            ViewBag.cat = cat;

            List<Product> p = db.Products.Where(m => m.Product_Name.Contains(searchString)).ToList();
            return View(p);
        }

        // show the product on index
        public ActionResult Index(int? catId)
        {
            List<Product> p = db.Products.ToList();
            var cat = db.Categories.ToList();
            ViewBag.cat = cat;

            if (catId != null)
            {
                var prod = db.Products.Where(m => m.Category_Id == catId).ToList();
                return View(prod);
            }

            return View(p);
        }
        public ActionResult ShowComment ( int Product_Id)
        {
            return View();
        }
        //Register
        public ActionResult Register()
        {
            return View();
        }

        //GET: Login page
        public ActionResult Login()
        {
            return View();
        }

        //POST: Login
        [HttpPost]
        public ActionResult LoginAccount(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                Account_Cus acc = db.Account_Cuss.Where(a => a.Username == model.Username && a.Password == model.Password).FirstOrDefault();
                if (acc != null)
                {
                    FormsAuthentication.SetAuthCookie(acc.Username, model.rememberMe == false);
                    ViewBag.user = User.Identity.Name;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.err = String.Format("Your Password or Username is incorrect!");
                }
            }
            return View("Login", model);
        }
        //Login with facebook
        public ActionResult LoginFacebook ()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope ="email",
            }) ; 
            return Redirect(loginUrl.AbsoluteUri);
        }
        public ActionResult FaceBookCallBack(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                grant_type = "authorization_code",
                code = code,
                redirect_uri = RedirectUri.AbsoluteUri
            });
            var accessToken = result.access_token;
            //if (!string.IsNullOrEmpty(accessToken))
            //    //get the facebook user information
            //{
            //    fb.AccessToken = accessToken;
            //    dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email");
            //    string email = me.email;
            //    string username = me.email;
            //    string firstname = me.first_name;
            //    string middlename = me.middle_name;
            //    string lastname = me.last_name;

            //    var user = new  Account_Cus();
            //    user.Email = email;
            //    user.Username = email;
            //    user.Name = firstname + " " + middlename+ " " + lastname +" ";
            //    var resultInsert = new HomeController().InsertFacebook(user);
            //    if ( resultInsert > 0)
            //    {
            //        var userSession = new LoginViewModel();
            //        userSession.Username = user.Username;
            //        //Session.Add(lastname, me);
            //    }
 
            //}
        
            return Redirect("/");
        }
        //Insert account facebook
        public long InsertFacebook(Account_Cus entity)
        {
            var user = db.Account_Cuss.SingleOrDefault(x => x.Username == entity.Username);
            if (user == null)
            {
                db.Account_Cuss.Add(entity);
                db.SaveChanges();
                return entity.Cus_Id;
            }
            else
            {
                return user.Cus_Id;
            }
        }


        //Log Out
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("Index");
        }
        //
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
        // Customer Register
        public ActionResult Account(int? page)
        {
            IPagedList<Account_Cus> acc = db.Account_Cuss.ToList().ToPagedList(page ?? 1, 10);

            return View(acc);
        }
        //public ActionResult Register()
        //{
        //    return View();
        //}

        [HttpPost]
        public ActionResult Register(Account_Cus account)
        {
            ViewBag.Roles = RoleList();
            if (ModelState.IsValid)
            {
                Account_Cus accObj = new Account_Cus();

                accObj.Cus_Id = account.Cus_Id;

                accObj.RoleId = account.RoleId;
                accObj.Username = account.Username;
                accObj.Password = account.Password;
                accObj.Name = account.Name;
                accObj.DOB = account.DOB;
                accObj.Tel = account.Tel;
                accObj.Email = account.Email;
               
                



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
                return Redirect("Index");
            }

            return View();

        }

        //GET: ForgotPassword page
        public ActionResult ForgotPassword()
        {

            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var account = db.Account_Cuss.Where(m => m.Email == model.Email).FirstOrDefault();
                if (account == null)
                {
                    ViewBag.err = String.Format("Email is not correct!");
                }

                string code = RandomString(50);
                var callbackUrl = Url.Action("ResetPassword", "Home", new { userId = account.Cus_Id, code = code }, protocol: Request.Url.Scheme);
                string body = string.Empty;
                using (StreamReader reader = new StreamReader(Server.MapPath("~/EmailForm/ForgotPasswordForm.html")))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{ConfirmationLink}", callbackUrl);
                body = body.Replace("{UserName}", model.Email);
                bool IsSendEmail = SendEmail.GmailSend(model.Email, "Confirm your account", body, true);
            }
            return View();
        }

        //GET: ResetPassword page
        public ActionResult ResetPassword(string code, int userId)
        {
            ResetPasswordViewModel model = new ResetPasswordViewModel();
            model.Id = userId;
            return code == null ? View("Error") : View(model);
        }
        //POST:ResetPassword
        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Account_Cuss.Where(m => m.Cus_Id == model.Id).FirstOrDefault();

                user.Password = model.Password;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();

                return Redirect("Login");
            }
            return View(model);
        }


        // Add product to cart
        public ActionResult AddToCart(int productId)
        {
            if (Session["Cart"] == null)
            {
                List<itemCart> cart = new List<itemCart>();
                var product = db.Products.Find(productId);
                cart.Add(new itemCart()
                {
                    Product = product,
                    Quantity = 1
                });
                Session["Cart"] = cart;
            }
            else
            {
                List<itemCart> cart = (List<itemCart>)Session["Cart"];
                var product = db.Products.Find(productId);
                foreach (var item in cart)
                {
                    if (item.Product.Product_Id == productId)
                    {
                        int prevQty = item.Quantity;
                        cart.Remove(item);
                        cart.Add(new itemCart()
                        {
                            Product = product,
                            Quantity = prevQty + 1
                        });
                        break;
                    }
                    else
                    {
                        cart.Add(new itemCart()
                        {
                            Product = product,
                            Quantity = 1
                        });
                    }
                }
                Session["Cart"] = cart;
            }
            return Redirect("Index");
        }

        // View detail product in cart
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
            Product product = db.Products.Find(id);
            var user = db.Account_Cuss.Where(m => m.Username == User.Identity.Name).FirstOrDefault();
            var commentModel = new CommentViewModel
            {
                Product_Id = product.Product_Id,
                Image = product.Image,
                Product_Name = product.Product_Name,
                Price = product.Price,
                Cus_Id = user.Cus_Id,
                Describe =product.Describe,
                Information = product.Information
            };
            var commentList = db.Comments.Where(m => m.Product_Id == product.Product_Id).ToList();
            ViewBag.list = commentList;
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(commentModel);
        }

        [Authorize]
        public ActionResult History(string searchString, int? page)
        {
            ViewBag.searchString = searchString;
            //var order = db.Orders.Where(m => m.Username == User.Identity.Name).ToList();
            var status = db.Statuses.ToList();
            ViewBag.status = status;
            return View(model.HistoryOrder(searchString,page,User.Identity.Name));
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

        [Authorize]
        public ActionResult HistoryDetail(int? id)
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
        public ActionResult Contact()
        {
            return View();
        }
        
    }
   
}

        
       