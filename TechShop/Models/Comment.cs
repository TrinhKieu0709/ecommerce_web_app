using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace TechShop.Models
{
    public class Comment
    {
        public  int Id { get; set; }
        public string Contents { get; set; }
        public int Rating { get; set; }
        public Nullable<int> Product_Id { get; set; }
        public Nullable<int> Admin_Id { get; set; }
        public Nullable <int> Cus_Id { get; set; }

       

        public virtual Account_Admin Account_Admin  { get; set; }
        public virtual Account_Cus Account_Cus { get; set; }
       
        public virtual Product Product { get; set; }

        
    }
}