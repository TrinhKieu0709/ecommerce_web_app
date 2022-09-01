using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechShop.Models
{
    public class Chat_Box

    {
        public int Id { get; set; }
        public string Message { get; set; }
        public Nullable<int> Cus_Id { get; set; }
        public Nullable<int> Adimin_Id { get; set; }

        public virtual Account_Cus Account_Cus { get; set; }
        public virtual Account_Admin  Account_Admin { get; set; }
    }
}