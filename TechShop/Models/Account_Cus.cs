using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TechShop.Models
{
    public class Account_Cus
    {
        [Key]
        public int Cus_Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public DateTime DOB { get; set; }
        public string Avatar { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int Tel { get; set; }
        
        public Nullable<int> RoleId { get; set; }
        public Nullable<int> RoleName { get; set; }

        public virtual Role Role { get; set; }
        public  ICollection <Comment>Comments { get; set; }
        public ICollection <Chat_Box> Chat_Boxes { get; set; }
        public virtual Cart  Cart { get; set; }
        public ICollection<Product> Products { get; set; }



    }
}