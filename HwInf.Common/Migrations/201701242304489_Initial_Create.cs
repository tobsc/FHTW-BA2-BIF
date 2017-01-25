namespace HwInf.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial_Create : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.Components",
                c => new
                    {
                        CompId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        FieldType = c.String(),
                        DeviceType_TypeId = c.Int(),
                    })
                .PrimaryKey(t => t.CompId)
                .ForeignKey("public.DeviceTypes", t => t.DeviceType_TypeId)
                .Index(t => t.DeviceType_TypeId);
            
            CreateTable(
                "public.DeviceTypes",
                c => new
                    {
                        TypeId = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.TypeId);
            
            CreateTable(
                "public.DeviceHistory",
                c => new
                    {
                        HistId = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Device_DeviceId = c.Int(nullable: false),
                        Person_PersId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.HistId)
                .ForeignKey("public.Devices", t => t.Device_DeviceId, cascadeDelete: true)
                .ForeignKey("public.Persons", t => t.Person_PersId, cascadeDelete: true)
                .Index(t => t.Device_DeviceId)
                .Index(t => t.Person_PersId);
            
            CreateTable(
                "public.Devices",
                c => new
                    {
                        DeviceId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        InvNum = c.String(),
                        Brand = c.String(nullable: false),
                        Room = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        Person_PersId = c.Int(nullable: false),
                        Status_StatusId = c.Int(nullable: false),
                        Type_TypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DeviceId)
                .ForeignKey("public.Persons", t => t.Person_PersId, cascadeDelete: true)
                .ForeignKey("public.DeviceStatus", t => t.Status_StatusId, cascadeDelete: true)
                .ForeignKey("public.DeviceTypes", t => t.Type_TypeId, cascadeDelete: true)
                .Index(t => t.Person_PersId)
                .Index(t => t.Status_StatusId)
                .Index(t => t.Type_TypeId);
            
            CreateTable(
                "public.Persons",
                c => new
                    {
                        PersId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Tel = c.String(),
                        uid = c.String(nullable: false),
                        Room = c.String(),
                        Role_RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PersId)
                .ForeignKey("public.Roles", t => t.Role_RoleId, cascadeDelete: true)
                .Index(t => t.Role_RoleId);
            
            CreateTable(
                "public.Roles",
                c => new
                    {
                        RoleId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "public.DeviceStatus",
                c => new
                    {
                        StatusId = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.StatusId);
            
            CreateTable(
                "public.DeviceMeta",
                c => new
                    {
                        MetaId = c.Int(nullable: false, identity: true),
                        MetaValue = c.String(nullable: false),
                        Component_CompId = c.Int(nullable: false),
                        Device_DeviceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MetaId)
                .ForeignKey("public.Components", t => t.Component_CompId, cascadeDelete: true)
                .ForeignKey("public.Devices", t => t.Device_DeviceId, cascadeDelete: true)
                .Index(t => t.Component_CompId)
                .Index(t => t.Device_DeviceId);
            
            CreateTable(
                "public.OrderItems",
                c => new
                    {
                        ItemId = c.Int(nullable: false, identity: true),
                        Device_DeviceId = c.Int(nullable: false),
                        Order_OrderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ItemId)
                .ForeignKey("public.Devices", t => t.Device_DeviceId, cascadeDelete: true)
                .ForeignKey("public.Orders", t => t.Order_OrderId, cascadeDelete: true)
                .Index(t => t.Device_DeviceId)
                .Index(t => t.Order_OrderId);
            
            CreateTable(
                "public.Orders",
                c => new
                    {
                        OrderId = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        From = c.DateTime(nullable: false),
                        To = c.DateTime(nullable: false),
                        ReturnDate = c.DateTime(nullable: false),
                        Owner_PersId = c.Int(nullable: false),
                        Person_PersId = c.Int(nullable: false),
                        Status_StatusId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OrderId)
                .ForeignKey("public.Persons", t => t.Owner_PersId, cascadeDelete: true)
                .ForeignKey("public.Persons", t => t.Person_PersId, cascadeDelete: true)
                .ForeignKey("public.OrderStatus", t => t.Status_StatusId, cascadeDelete: true)
                .Index(t => t.Owner_PersId)
                .Index(t => t.Person_PersId)
                .Index(t => t.Status_StatusId);
            
            CreateTable(
                "public.OrderStatus",
                c => new
                    {
                        StatusId = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.StatusId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("public.OrderItems", "Order_OrderId", "public.Orders");
            DropForeignKey("public.Orders", "Status_StatusId", "public.OrderStatus");
            DropForeignKey("public.Orders", "Person_PersId", "public.Persons");
            DropForeignKey("public.Orders", "Owner_PersId", "public.Persons");
            DropForeignKey("public.OrderItems", "Device_DeviceId", "public.Devices");
            DropForeignKey("public.DeviceMeta", "Device_DeviceId", "public.Devices");
            DropForeignKey("public.DeviceMeta", "Component_CompId", "public.Components");
            DropForeignKey("public.DeviceHistory", "Person_PersId", "public.Persons");
            DropForeignKey("public.DeviceHistory", "Device_DeviceId", "public.Devices");
            DropForeignKey("public.Devices", "Type_TypeId", "public.DeviceTypes");
            DropForeignKey("public.Devices", "Status_StatusId", "public.DeviceStatus");
            DropForeignKey("public.Devices", "Person_PersId", "public.Persons");
            DropForeignKey("public.Persons", "Role_RoleId", "public.Roles");
            DropForeignKey("public.Components", "DeviceType_TypeId", "public.DeviceTypes");
            DropIndex("public.Orders", new[] { "Status_StatusId" });
            DropIndex("public.Orders", new[] { "Person_PersId" });
            DropIndex("public.Orders", new[] { "Owner_PersId" });
            DropIndex("public.OrderItems", new[] { "Order_OrderId" });
            DropIndex("public.OrderItems", new[] { "Device_DeviceId" });
            DropIndex("public.DeviceMeta", new[] { "Device_DeviceId" });
            DropIndex("public.DeviceMeta", new[] { "Component_CompId" });
            DropIndex("public.Persons", new[] { "Role_RoleId" });
            DropIndex("public.Devices", new[] { "Type_TypeId" });
            DropIndex("public.Devices", new[] { "Status_StatusId" });
            DropIndex("public.Devices", new[] { "Person_PersId" });
            DropIndex("public.DeviceHistory", new[] { "Person_PersId" });
            DropIndex("public.DeviceHistory", new[] { "Device_DeviceId" });
            DropIndex("public.Components", new[] { "DeviceType_TypeId" });
            DropTable("public.OrderStatus");
            DropTable("public.Orders");
            DropTable("public.OrderItems");
            DropTable("public.DeviceMeta");
            DropTable("public.DeviceStatus");
            DropTable("public.Roles");
            DropTable("public.Persons");
            DropTable("public.Devices");
            DropTable("public.DeviceHistory");
            DropTable("public.DeviceTypes");
            DropTable("public.Components");
        }
    }
}
