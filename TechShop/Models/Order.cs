using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TechShop.Models
{
    public class Order
    {

        [Key]
        public int Order_Id { get; set; }
       
        public string Payment_Type { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Cart_Status { get; set; }
        public string Voucher { get; set; }
        public string Username { get; set; }
        public DateTime SubmitDate { get; set; }
      




        public Nullable<int> Status_Id { get; set; }
        public Nullable<int> Year_ID { get; set; }
        public ICollection<Account_Admin> account_Admins { get; set; }

        public ICollection<Years> Years { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Status> Statuses { get; set; }
        public virtual Years Yearss { get; set; }









    }
}