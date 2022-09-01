using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TechShop.Models;


namespace TechShop.DAL
{
    public class MyAppContext : DbContext
    {
      
        public DbSet<Account_Admin> Account_Admins { get; set; }
        public DbSet<Account_Cus> Account_Cuss { get; set; }
        public DbSet<Product> Products { get; set; }
       
        public DbSet<OrderDetail> orderDetails { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Chat_Box> Chat_Boxs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Delivery> deliveries { get; set; }
        
        public DbSet<Contact> contacts { get; set; }

        public DbSet<Years> years { get; set; }






    }
}