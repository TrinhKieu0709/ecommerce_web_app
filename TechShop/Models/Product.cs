using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;



namespace TechShop.Models
{
    public class Product
    {
        [Key]
        
        public int Product_Id { get; set; }

        [Display(Name = "Transt_ProductName", ResourceType =typeof(StaticResource.Resources))]
        public string Product_Name { get; set; }

        [Display(Name = "Transt_Describe", ResourceType = typeof(StaticResource.Resources))]
        public string Describe { get; set; }

        [Display(Name = "Transt_Price", ResourceType = typeof(StaticResource.Resources))]
        public int Price { get; set; }
        [Display(Name = "Transt_Quantity", ResourceType = typeof(StaticResource.Resources))]
        public int Quantity { get; set; }

        [Display(Name = "Transt_Image", ResourceType = typeof(StaticResource.Resources))]
        public string Image { get; set; }


        [Display(Name = "Transt_ID", ResourceType = typeof(StaticResource.Resources))]
        public int Category_Id { get; set; }

        [Display(Name = "Transt_Information", ResourceType = typeof(StaticResource.Resources))]
        public string Information { get; set; }



        public virtual Account_Admin Account_Admin { get; set; }
    
       
        public virtual ICollection<Order> Orders { get; set; }
      
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
       
        public virtual Account_Cus Account_Cus { get; set; }
        public ICollection<Comment> Comments { get; set; }

    }
}