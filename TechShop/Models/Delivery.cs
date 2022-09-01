using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TechShop.Models
{
    public class Delivery
    {
        [Key]
        public string Delivery_Name { get; set; }
        public int Delivery_ID { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}