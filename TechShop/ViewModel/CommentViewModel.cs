using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TechShop.ViewModel
{
    public class CommentViewModel
    {
        //product model
        public string Information { get; set; }
        public int Rating { get; set; }
        public int Number { get; set; }
        public string Image { get; set; }
        public int Price { get; set; }
        public string Product_Name { get; set; }
        public string Describe { get; set; }
        //Comment model
        public string Avatar { get; set; }
        [Required]
        public string Message { get; set; }
        public DateTime CommentDate { get; set; }
        //require
        public Nullable<int> Cus_Id { get; set; }
        public Nullable<int> Product_Id { get; set; }
        public Nullable<int> Admin_Id { get; set; }



    }
}