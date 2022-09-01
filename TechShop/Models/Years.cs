
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TechShop.Models
{
    public class Years
    {
        [Key]
        public int Year_ID { get; set; }
        [Required]
        public int Year_No { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}