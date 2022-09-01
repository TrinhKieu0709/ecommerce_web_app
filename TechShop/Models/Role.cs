using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TechShop.Models;
namespace TechShop.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Account_Admin> Account_Admins { get; set; }

             
    }
}