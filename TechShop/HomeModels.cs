using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TechShop.DAL;
using TechShop.Models;

namespace TechShop.Models
{
    public class HomeModels
    {
        MyAppContext db = new MyAppContext();
        public IPagedList<Order> SearchListOrder { get; set; }
      
        public HomeModels SearchString(string searchString, int? page)
        {
            IPagedList<Order> CusOrders = db.Orders.Where(m => m.Name.Contains(searchString) || searchString == null).ToList().ToPagedList(page ?? 1, 10);
            return new HomeModels
            {
                SearchListOrder = CusOrders
            };
         
        }

        public IPagedList<Order> historyList { get; set; }
        public HomeModels HistoryOrder(string searchString, int? page, string username)
        {
            IPagedList<Order> CusOrders = db.Orders.Where(m => m.Name.Contains(searchString) || searchString == null && m.Username == username).ToList().ToPagedList(page ?? 1, 10);
            return new HomeModels
            {
                historyList = CusOrders
            };
        }
    }
}