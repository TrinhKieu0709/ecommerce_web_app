using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TechShop.Models
{
    public class Contact
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string address { get; set;}
        public string phonenumber { get; set; }
        public string mail { get; set; }
        public string content { get; set; }
        public bool status { get; set; }


    }
}