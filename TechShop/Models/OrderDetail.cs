using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TechShop.Models
{
    public class OrderDetail
    {
        [Key]
        [Column(Order = 1 )]

        public int Order_Id { get; set; }
        [Key]
        [Column(Order = 2)]
        public int Product_Id { get; set; }
        public int Quantity { get; set; }
        public string Image { get; set; }

        public DateTime SubmitDate { get; set; }

        public Nullable<double> Price { get; set; }
        public Nullable<int> Name { get; set; }
   
        //public Nullable<int> Quantity { get; set; }
        public int Delivery_Unit { get; set; }
        public Nullable<int> Status_Id { get; set; }
        public Nullable<int> Delivery_Id { get; set; }
        public virtual Status Status { get; set; }
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
        public virtual Delivery Delivery { get; set;}


    }
}