using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TechShop.Models
{
    public class Coupon
    {
        [Key]
        public int Coupon_Id { get; set; }
        public string VoucherCode { get; set; }
        public double discount { get; set; }
        public double maxdiscount { get; set; } 
        public virtual Order Order { get; set; } 
        public virtual OrderDetail OrderDetail { get; set; }

    }
}