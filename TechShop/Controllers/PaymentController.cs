using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TechShop.Controllers
{
    public class PaymentController : Controller
    {

        public ActionResult PaymentWithPaypal()
        {
            return View();
        }
    }
}