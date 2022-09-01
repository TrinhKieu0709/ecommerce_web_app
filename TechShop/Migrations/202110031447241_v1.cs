namespace TechShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Account_Admin",
                c => new
                    {
                        Admin_Id = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Password = c.String(),
                        Name = c.String(),
                        DOB = c.DateTime(nullable: false),
                        Email = c.String(),
                        Tel = c.Int(nullable: false),
                        RoleId = c.Int(),
                        Cart_Cart_Id = c.Int(),
                        Order_Order_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Admin_Id)
                .ForeignKey("dbo.Carts", t => t.Cart_Cart_Id)
                .ForeignKey("dbo.Orders", t => t.Order_Order_Id)
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .Index(t => t.RoleId)
                .Index(t => t.Cart_Cart_Id)
                .Index(t => t.Order_Order_Id);
            
            CreateTable(
                "dbo.Chat_Box",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        Cus_Id = c.Int(),
                        Adimin_Id = c.Int(),
                        Account_Admin_Admin_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Account_Admin", t => t.Account_Admin_Admin_Id)
                .ForeignKey("dbo.Account_Cus", t => t.Cus_Id)
                .Index(t => t.Cus_Id)
                .Index(t => t.Account_Admin_Admin_Id);
            
            CreateTable(
                "dbo.Account_Cus",
                c => new
                    {
                        Cus_Id = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Password = c.String(),
                        Name = c.String(),
                        DOB = c.DateTime(nullable: false),
                        Avatar = c.String(),
                        Email = c.String(),
                        Address = c.String(),
                        Tel = c.Int(nullable: false),
                        RoleId = c.Int(),
                        RoleName = c.Int(),
                        Cart_Cart_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Cus_Id)
                .ForeignKey("dbo.Carts", t => t.Cart_Cart_Id)
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .Index(t => t.RoleId)
                .Index(t => t.Cart_Cart_Id);
            
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        Cart_Id = c.Int(nullable: false, identity: true),
                        Cart_Name = c.String(),
                        Payment_Type = c.String(),
                        Phone = c.Int(),
                        Address = c.Int(),
                        Name = c.Int(),
                        Email = c.Int(),
                        Product_Name = c.Int(),
                        Price = c.Int(),
                        Quantity = c.Int(),
                        Product_Id = c.Int(),
                        Cus_Id = c.Int(),
                        Cart_Status = c.Int(),
                    })
                .PrimaryKey(t => t.Cart_Id);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Contents = c.String(),
                        Rating = c.Int(nullable: false),
                        Product_Id = c.Int(),
                        Admin_Id = c.Int(),
                        Cus_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Account_Admin", t => t.Admin_Id)
                .ForeignKey("dbo.Account_Cus", t => t.Cus_Id)
                .ForeignKey("dbo.Products", t => t.Product_Id)
                .Index(t => t.Product_Id)
                .Index(t => t.Admin_Id)
                .Index(t => t.Cus_Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Product_Id = c.Int(nullable: false, identity: true),
                        Product_Name = c.String(),
                        Describe = c.String(),
                        Price = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Image = c.String(),
                        Category_Id = c.Int(nullable: false),
                        Information = c.String(),
                        Account_Admin_Admin_Id = c.Int(),
                        Account_Cus_Cus_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Product_Id)
                .ForeignKey("dbo.Account_Admin", t => t.Account_Admin_Admin_Id)
                .ForeignKey("dbo.Account_Cus", t => t.Account_Cus_Cus_Id)
                .ForeignKey("dbo.Categories", t => t.Category_Id, cascadeDelete: true)
                .Index(t => t.Category_Id)
                .Index(t => t.Account_Admin_Admin_Id)
                .Index(t => t.Account_Cus_Cus_Id);
            
            CreateTable(
                "dbo.OrderDetails",
                c => new
                    {
                        Order_Id = c.Int(nullable: false),
                        Product_Id = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Image = c.String(),
                        SubmitDate = c.DateTime(nullable: false),
                        Price = c.Double(),
                        Name = c.Int(),
                        Delivery_Unit = c.Int(nullable: false),
                        Status_Id = c.Int(),
                        Delivery_Id = c.Int(),
                        Delivery_Delivery_Name = c.String(maxLength: 128),
                        Status_Status_Name = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Order_Id, t.Product_Id })
                .ForeignKey("dbo.Deliveries", t => t.Delivery_Delivery_Name)
                .ForeignKey("dbo.Orders", t => t.Order_Id, cascadeDelete: true)
                .ForeignKey("dbo.Status", t => t.Status_Status_Name)
                .ForeignKey("dbo.Products", t => t.Product_Id, cascadeDelete: true)
                .Index(t => t.Order_Id)
                .Index(t => t.Product_Id)
                .Index(t => t.Delivery_Delivery_Name)
                .Index(t => t.Status_Status_Name);
            
            CreateTable(
                "dbo.Deliveries",
                c => new
                    {
                        Delivery_Name = c.String(nullable: false, maxLength: 128),
                        Delivery_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Delivery_Name);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Order_Id = c.Int(nullable: false, identity: true),
                        Payment_Type = c.String(),
                        Phone = c.String(),
                        Address = c.String(),
                        Name = c.String(),
                        Email = c.String(),
                        Cart_Status = c.String(),
                        Voucher = c.String(),
                        Username = c.String(),
                        SubmitDate = c.DateTime(nullable: false),
                        Status_Id = c.Int(),
                        Year_ID = c.Int(),
                        Years_Year_ID = c.Int(),
                        Yearss_Year_ID = c.Int(),
                    })
                .PrimaryKey(t => t.Order_Id)
                .ForeignKey("dbo.Years", t => t.Years_Year_ID)
                .ForeignKey("dbo.Years", t => t.Yearss_Year_ID)
                .Index(t => t.Years_Year_ID)
                .Index(t => t.Yearss_Year_ID);
            
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        Status_Name = c.String(nullable: false, maxLength: 128),
                        Status_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Status_Name);
            
            CreateTable(
                "dbo.Years",
                c => new
                    {
                        Year_ID = c.Int(nullable: false, identity: true),
                        Year_No = c.Int(nullable: false),
                        Order_Order_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Year_ID)
                .ForeignKey("dbo.Orders", t => t.Order_Order_Id)
                .Index(t => t.Order_Order_Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Category_Id = c.Int(nullable: false, identity: true),
                        Category_Name = c.String(),
                    })
                .PrimaryKey(t => t.Category_Id);
            
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        address = c.String(),
                        phonenumber = c.String(),
                        mail = c.String(),
                        content = c.String(),
                        status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Coupons",
                c => new
                    {
                        Coupon_Id = c.Int(nullable: false, identity: true),
                        VoucherCode = c.String(),
                        discount = c.Double(nullable: false),
                        maxdiscount = c.Double(nullable: false),
                        Order_Order_Id = c.Int(),
                        OrderDetail_Order_Id = c.Int(),
                        OrderDetail_Product_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Coupon_Id)
                .ForeignKey("dbo.Orders", t => t.Order_Order_Id)
                .ForeignKey("dbo.OrderDetails", t => new { t.OrderDetail_Order_Id, t.OrderDetail_Product_Id })
                .Index(t => t.Order_Order_Id)
                .Index(t => new { t.OrderDetail_Order_Id, t.OrderDetail_Product_Id });
            
            CreateTable(
                "dbo.OrderProducts",
                c => new
                    {
                        Order_Order_Id = c.Int(nullable: false),
                        Product_Product_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Order_Order_Id, t.Product_Product_Id })
                .ForeignKey("dbo.Orders", t => t.Order_Order_Id, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.Product_Product_Id, cascadeDelete: true)
                .Index(t => t.Order_Order_Id)
                .Index(t => t.Product_Product_Id);
            
            CreateTable(
                "dbo.StatusOrders",
                c => new
                    {
                        Status_Status_Name = c.String(nullable: false, maxLength: 128),
                        Order_Order_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Status_Status_Name, t.Order_Order_Id })
                .ForeignKey("dbo.Status", t => t.Status_Status_Name, cascadeDelete: true)
                .ForeignKey("dbo.Orders", t => t.Order_Order_Id, cascadeDelete: true)
                .Index(t => t.Status_Status_Name)
                .Index(t => t.Order_Order_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Coupons", new[] { "OrderDetail_Order_Id", "OrderDetail_Product_Id" }, "dbo.OrderDetails");
            DropForeignKey("dbo.Coupons", "Order_Order_Id", "dbo.Orders");
            DropForeignKey("dbo.Products", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.Account_Cus", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Account_Admin", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.OrderDetails", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.Orders", "Yearss_Year_ID", "dbo.Years");
            DropForeignKey("dbo.Years", "Order_Order_Id", "dbo.Orders");
            DropForeignKey("dbo.Orders", "Years_Year_ID", "dbo.Years");
            DropForeignKey("dbo.StatusOrders", "Order_Order_Id", "dbo.Orders");
            DropForeignKey("dbo.StatusOrders", "Status_Status_Name", "dbo.Status");
            DropForeignKey("dbo.OrderDetails", "Status_Status_Name", "dbo.Status");
            DropForeignKey("dbo.OrderProducts", "Product_Product_Id", "dbo.Products");
            DropForeignKey("dbo.OrderProducts", "Order_Order_Id", "dbo.Orders");
            DropForeignKey("dbo.OrderDetails", "Order_Id", "dbo.Orders");
            DropForeignKey("dbo.Account_Admin", "Order_Order_Id", "dbo.Orders");
            DropForeignKey("dbo.OrderDetails", "Delivery_Delivery_Name", "dbo.Deliveries");
            DropForeignKey("dbo.Comments", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.Products", "Account_Cus_Cus_Id", "dbo.Account_Cus");
            DropForeignKey("dbo.Products", "Account_Admin_Admin_Id", "dbo.Account_Admin");
            DropForeignKey("dbo.Comments", "Cus_Id", "dbo.Account_Cus");
            DropForeignKey("dbo.Comments", "Admin_Id", "dbo.Account_Admin");
            DropForeignKey("dbo.Chat_Box", "Cus_Id", "dbo.Account_Cus");
            DropForeignKey("dbo.Account_Cus", "Cart_Cart_Id", "dbo.Carts");
            DropForeignKey("dbo.Account_Admin", "Cart_Cart_Id", "dbo.Carts");
            DropForeignKey("dbo.Chat_Box", "Account_Admin_Admin_Id", "dbo.Account_Admin");
            DropIndex("dbo.StatusOrders", new[] { "Order_Order_Id" });
            DropIndex("dbo.StatusOrders", new[] { "Status_Status_Name" });
            DropIndex("dbo.OrderProducts", new[] { "Product_Product_Id" });
            DropIndex("dbo.OrderProducts", new[] { "Order_Order_Id" });
            DropIndex("dbo.Coupons", new[] { "OrderDetail_Order_Id", "OrderDetail_Product_Id" });
            DropIndex("dbo.Coupons", new[] { "Order_Order_Id" });
            DropIndex("dbo.Years", new[] { "Order_Order_Id" });
            DropIndex("dbo.Orders", new[] { "Yearss_Year_ID" });
            DropIndex("dbo.Orders", new[] { "Years_Year_ID" });
            DropIndex("dbo.OrderDetails", new[] { "Status_Status_Name" });
            DropIndex("dbo.OrderDetails", new[] { "Delivery_Delivery_Name" });
            DropIndex("dbo.OrderDetails", new[] { "Product_Id" });
            DropIndex("dbo.OrderDetails", new[] { "Order_Id" });
            DropIndex("dbo.Products", new[] { "Account_Cus_Cus_Id" });
            DropIndex("dbo.Products", new[] { "Account_Admin_Admin_Id" });
            DropIndex("dbo.Products", new[] { "Category_Id" });
            DropIndex("dbo.Comments", new[] { "Cus_Id" });
            DropIndex("dbo.Comments", new[] { "Admin_Id" });
            DropIndex("dbo.Comments", new[] { "Product_Id" });
            DropIndex("dbo.Account_Cus", new[] { "Cart_Cart_Id" });
            DropIndex("dbo.Account_Cus", new[] { "RoleId" });
            DropIndex("dbo.Chat_Box", new[] { "Account_Admin_Admin_Id" });
            DropIndex("dbo.Chat_Box", new[] { "Cus_Id" });
            DropIndex("dbo.Account_Admin", new[] { "Order_Order_Id" });
            DropIndex("dbo.Account_Admin", new[] { "Cart_Cart_Id" });
            DropIndex("dbo.Account_Admin", new[] { "RoleId" });
            DropTable("dbo.StatusOrders");
            DropTable("dbo.OrderProducts");
            DropTable("dbo.Coupons");
            DropTable("dbo.Contacts");
            DropTable("dbo.Categories");
            DropTable("dbo.Roles");
            DropTable("dbo.Years");
            DropTable("dbo.Status");
            DropTable("dbo.Orders");
            DropTable("dbo.Deliveries");
            DropTable("dbo.OrderDetails");
            DropTable("dbo.Products");
            DropTable("dbo.Comments");
            DropTable("dbo.Carts");
            DropTable("dbo.Account_Cus");
            DropTable("dbo.Chat_Box");
            DropTable("dbo.Account_Admin");
        }
    }
}
