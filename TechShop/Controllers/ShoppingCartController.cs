using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using TechShop.ViewModel;
using TechShop.DAL;
using TechShop.Models;
using TechShop.Models.Home;
using PayPal.Api;
using System.IO;
using System.Configuration;
using TechShop.EmailForm;

namespace TechShop.Controllers
{
    public class ShoppingCartController : Controller
    {
        MyAppContext db = new MyAppContext();
        public string strCart = "Cart";
        //GET: ShoppingCart
        public ActionResult Index()
        {
            return View();
        }
        //Apply coupon
        public ActionResult ApplyDiscount(string code)
        {
            var coupon = db.Coupons.Where(m => m.VoucherCode == code).FirstOrDefault();
            if (db.Coupons.Any(m => m.VoucherCode == code))
            {
                string applySuccess = "Coupon code " + code + " applied successfully";

                List<itemCart> carts = (List<itemCart>)Session[strCart];

                var grandTotal = Convert.ToDouble(carts.Sum(m => m.Quantity * m.Product.Price));
                var discountprice = (grandTotal * coupon.discount) / 100;
                if (discountprice > coupon.maxdiscount)
                {
                    discountprice = coupon.maxdiscount;
                }

                ViewBag.discount = discountprice.ToString() + "( " + coupon.discount + "% ) Maximum Upto." + coupon.maxdiscount;

                ViewBag.finalPrice = grandTotal - discountprice;

                ViewBag.status = applySuccess;

                Session["finalPrice"] = ViewBag.finalPrice;
                Session["codeEntered"] = code;

                return View("Index");
            }
            else
            {
                ViewBag.status = "Your code is not exist!!";
                return View("Index");
            }

            //return View();
        }

        // Order product in cart
        public ActionResult OrderNow(int? id , string img )
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            if (Session[strCart] == null)
            {
                List<itemCart> cart = new List<itemCart>();
                var product = db.Products.Where(m => m.Product_Id == id).FirstOrDefault();
                //var Img = db.Products.Where(m => m.Image == img).FirstOrDefault();
                cart.Add(new itemCart() { Product = product, Quantity = 1 });
                Session[strCart] = cart;

            }
            else
            {
                List<itemCart> cart = (List<itemCart>)Session[strCart];
                int check = isExistingCheck(id);
                if (check == -1)
                    cart.Add(new itemCart() { Product = db.Products.Find(id), Quantity = 1 });
                else
                    cart[check].Quantity++;
                Session[strCart] = cart;
            }
            return View("Index");

        }
        private int isExistingCheck(int? id)
        {
            List<itemCart> IsCart = (List<itemCart>)Session[strCart];
            for (int i = 0; i < IsCart.Count; i++)
            {
                if (IsCart[i].Product.Product_Id == id) return i;
            }
            return -1;
        }
        // Delete product in Cart
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            int check = isExistingCheck(id);
            List<itemCart> IsCart = (List<itemCart>)Session[strCart];
            IsCart.RemoveAt(check);
            return View("Index");

        }
        //Update Cart when change quantiy
        public ActionResult UpdateCart(FormCollection frc)
        {

            string[] quantities = frc.GetValues("quantity");
            List<itemCart> istCart = (List<itemCart>)Session[strCart];
            for (int i = 0; i < istCart.Count; i++)
            {
                istCart[i].Quantity = Convert.ToInt32(quantities[i]);

            }
            Session[strCart] = istCart;
            return View("Index");

        }
        //Check out the cart
        public ActionResult CheckOut()
        {
            ViewBag.DeliveryList = GetDelivery();
            var payment = new List<SelectListItem>();
            payment.Add(new SelectListItem { Text = "Cash", Value = "Cash"});
            payment.Add(new SelectListItem { Text = "Paypal", Value = "Paypal" });
            ViewBag.payment = payment;
            return View("CheckOut");

        }


        // select delivert unit
        [HttpPost]
        public ActionResult select(OrderDetail orderDetail)
        {

            ViewBag.DeliveryList = GetDelivery();
            OrderDetail abc = new OrderDetail();
            abc.Delivery_Unit = orderDetail.Delivery_Unit;
            db.orderDetails.Add(abc);
            db.SaveChanges();
            return View();

        }
        public ActionResult savechange(OrderDetail orderDetail)
        {
            OrderDetail abc = new OrderDetail();
            abc.Delivery_Id = orderDetail.Delivery_Id;
            db.orderDetails.Add(abc);
            db.SaveChanges();
            return View();
        }
        //Select delivery unit
        public List<SelectListItem> GetDelivery()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var deli = db.deliveries.ToList();
            foreach (var item in deli)
            {
                list.Add(new SelectListItem { Value = item.Delivery_ID.ToString(), Text = item.Delivery_Name });
            }
            return list;

        }

        // process

        public ActionResult ProcessOrder(FormCollection frc)
        {

            List<itemCart> istCart = (List<itemCart>)Session[strCart];
            // Save order into Order table
            var order = new TechShop.Models.Order()
            {
                Name = frc["cusName"],
                Phone = frc["cusPhone"],
                Address = frc["cusAddress"],
                Email = frc["cusEmail"],
                Payment_Type = frc["cusPayment"],
                Cart_Status = "Processing",
                Username = User.Identity.Name,
                SubmitDate = DateTime.Now
            };

            if (Session["codeEntered"] != null)
            {
                order.Voucher = Session["codeEntered"].ToString();
            }

            db.Orders.Add(order);
            db.SaveChanges();
            // save order detail 
            foreach (itemCart item in istCart)
            {
                OrderDetail orderdetail = new OrderDetail()
                {
                    Order_Id = order.Order_Id,
                    Product_Id = item.Product.Product_Id,
                    Quantity = item.Quantity,
                    Price = item.Product.Price,
                    SubmitDate = DateTime.Now


                };
                db.orderDetails.Add(orderdetail);
                db.SaveChanges();

            }
            string content = System.IO.File.ReadAllText(Server.MapPath("~/EmailForm/Neworders.html"));
            content = content.Replace("{{CustomerName}}", order.Name);
            content = content.Replace("{{Phone}}", order.Phone);
            content = content.Replace("{{Email}}", order.Email);
            content = content.Replace("{{Address}}", order.Address);
            content = content.Replace("{{Paymet_Type}}", order.Payment_Type);
            
            bool isSend = SendEmail.GmailSend(order.Email, "Đơn hàng mới từ OnlineShop", content,true);
           

            if (order.Payment_Type == "Paypal")
            {
                return Redirect("PaymentWithPaypal");
            }
            Session.Remove(strCart);
            return View("OrderSuccess");


        }
        // work with payment
        private Payment payment;
        // create a payment ussing a APIcontext
        private Payment CreatePayment(APIContext apicontext, string redirecrUrl)
        {
            var listItems = new ItemList() { items = new List<PayPal.Api.Item>() };
            List<itemCart> ListCarts = (List<itemCart>)Session[strCart];
            foreach (var cart in ListCarts)
            {
                listItems.items.Add(new PayPal.Api.Item()
                {
                    name = cart.Product.Product_Name,
                    currency = "USD",
                    price = cart.Product.Price.ToString(),
                    quantity = cart.Quantity.ToString(),
                    sku = "sku"
                });
            }
            var payer = new Payer() { payment_method = "paypal" };
            // do th configuration redirectURLs here with redicrectUrl object
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirecrUrl,
                return_url = redirecrUrl
            };
            // create detail object
            var details = new Details()
            {
                tax = "1",
                shipping = "2",
                subtotal = ListCarts.Sum(m => m.Quantity * m.Product.Price).ToString()

            };
            // create amount object
            var amount = new Amount()
            {
                currency = "USD",
                total = (Convert.ToDouble(details.tax) + Convert.ToDouble(details.shipping) + Convert.ToDouble(details.subtotal)).ToString(),// tax+ fee shipping + subtotal
                details = details
            };
            // create transaction
            var transactionList = new List<Transaction>();
            transactionList.Add(new Transaction()
            {
                description = " testing description",
                invoice_number = Convert.ToString((new Random()).Next(100000)),
                amount = amount,
                item_list = listItems
            });
            payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls,
            };
            return payment.Create(apicontext);

        }
        // Create Executepayment method 
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            payment = new Payment() { id = paymentId };
            return payment.Execute(apiContext, paymentExecution);
        }
        // Create with paymentPaypal method
        public ActionResult PaymentWithPaypal()
        {
            // Getting context form the paypal bases on clientId & clientSecret for payment
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    //creating a payment
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/ShoppingCart/PaymentWithPaypal?";
                    var guid = Convert.ToString((new Random()).Next(100000));
                    var createdPayment = CreatePayment(apiContext, baseURI + "guid=" + guid);
                    // get links returned form paypal response to create call function
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = string.Empty;
                    while (links.MoveNext())
                    {
                        Links link = links.Current;
                        if (link.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            paypalRedirectUrl = link.href;
                        }
                    }
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // this one will be execute when we have reviced all the payment params form the previous call
                    var guid = Request.Params["guid"];
                    var executePayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    if (executePayment.state.ToLower() != " approved" )
                    {
                        return View("Success");
                    }
                }
            }
            catch (Exception ex)
            {
                PaypalLogger.Log("Error:" + ex.Message);

                Session.Remove(strCart);
                return View("Failure");
            }
            return View("Success");
        }
        






    }
}