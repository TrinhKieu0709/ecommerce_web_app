using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace TechShop.Models
{
    public class Cart
    {
        [Key]
        public int Cart_Id { get; set;}
        public string  Cart_Name { get; set; }
        public string Payment_Type { get; set; }
        public Nullable<int> Phone { get; set; }
        public Nullable<int> Address { get; set; }
        public Nullable<int> Name { get; set; }
        public Nullable<int> Email { get; set; }
        public Nullable<int> Product_Name { get; set; }
        public Nullable<int> Price { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<int> Product_Id { get; set; }
        public Nullable <int> Cus_Id { get; set; }
     
        public Nullable<int> Cart_Status { get; set; }

        public virtual ICollection<Account_Admin> Account_Admins { get; set; }
 


    }
}