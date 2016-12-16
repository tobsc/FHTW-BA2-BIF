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
                        CreateDate = c.DateTime(nullable: false),
                        Person_PersId = c.Int(nullable: false),
                        Room_RoomId = c.Int(),
                        Status_StatusId = c.Int(nullable: false),
                        Type_TypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DeviceId)
                .ForeignKey("public.Persons", t => t.Person_PersId, cascadeDelete: true)
                .ForeignKey("public.Rooms", t => t.Room_RoomId)
                .ForeignKey("public.Status", t => t.Status_StatusId, cascadeDelete: true)
                .ForeignKey("public.DeviceTypes", t => t.Type_TypeId, cascadeDelete: true)
                .Index(t => t.Person_PersId)
                .Index(t => t.Room_RoomId)
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
                        Role_RoleId = c.Int(nullable: false),
                        Room_RoomId = c.Int(),
                    })
                .PrimaryKey(t => t.PersId)
                .ForeignKey("public.Roles", t => t.Role_RoleId, cascadeDelete: true)
                .ForeignKey("public.Rooms", t => t.Room_RoomId)
                .Index(t => t.Role_RoleId)
                .Index(t => t.Room_RoomId);
            
            CreateTable(
                "public.Roles",
                c => new
                    {
                        RoleId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "public.Rooms",
                c => new
                    {
                        RoomId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.RoomId);
            
            CreateTable(
                "public.Status",
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
                        MetaKey = c.String(nullable: false),
                        MetaValue = c.String(nullable: false),
                        Device_DeviceId = c.Int(nullable: false),
                        DeviceType_TypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MetaId)
                .ForeignKey("public.Devices", t => t.Device_DeviceId, cascadeDelete: true)
                .ForeignKey("public.DeviceTypes", t => t.DeviceType_TypeId, cascadeDelete: true)
                .Index(t => t.Device_DeviceId)
                .Index(t => t.DeviceType_TypeId);
            
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
                        Owner_PersId = c.Int(nullable: false),
                        Person_PersId = c.Int(nullable: false),
                        Status_StatusId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OrderId)
                .ForeignKey("public.Persons", t => t.Owner_PersId, cascadeDelete: true)
                .ForeignKey("public.Persons", t => t.Person_PersId, cascadeDelete: true)
                .ForeignKey("public.Status", t => t.Status_StatusId, cascadeDelete: true)
                .Index(t => t.Owner_PersId)
                .Index(t => t.Person_PersId)
                .Index(t => t.Status_StatusId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("public.OrderItems", "Order_OrderId", "public.Orders");
            DropForeignKey("public.Orders", "Status_StatusId", "public.Status");
            DropForeignKey("public.Orders", "Person_PersId", "public.Persons");
            DropForeignKey("public.Orders", "Owner_PersId", "public.Persons");
            DropForeignKey("public.OrderItems", "Device_DeviceId", "public.Devices");
            DropForeignKey("public.DeviceMeta", "DeviceType_TypeId", "public.DeviceTypes");
            DropForeignKey("public.DeviceMeta", "Device_DeviceId", "public.Devices");
            DropForeignKey("public.DeviceHistory", "Person_PersId", "public.Persons");
            DropForeignKey("public.DeviceHistory", "Device_DeviceId", "public.Devices");
            DropForeignKey("public.Devices", "Type_TypeId", "public.DeviceTypes");
            DropForeignKey("public.Devices", "Status_StatusId", "public.Status");
            DropForeignKey("public.Devices", "Room_RoomId", "public.Rooms");
            DropForeignKey("public.Devices", "Person_PersId", "public.Persons");
            DropForeignKey("public.Persons", "Room_RoomId", "public.Rooms");
            DropForeignKey("public.Persons", "Role_RoleId", "public.Roles");
            DropForeignKey("public.Components", "DeviceType_TypeId", "public.DeviceTypes");
            DropIndex("public.Orders", new[] { "Status_StatusId" });
            DropIndex("public.Orders", new[] { "Person_PersId" });
            DropIndex("public.Orders", new[] { "Owner_PersId" });
            DropIndex("public.OrderItems", new[] { "Order_OrderId" });
            DropIndex("public.OrderItems", new[] { "Device_DeviceId" });
            DropIndex("public.DeviceMeta", new[] { "DeviceType_TypeId" });
            DropIndex("public.DeviceMeta", new[] { "Device_DeviceId" });
            DropIndex("public.Persons", new[] { "Room_RoomId" });
            DropIndex("public.Persons", new[] { "Role_RoleId" });
            DropIndex("public.Devices", new[] { "Type_TypeId" });
            DropIndex("public.Devices", new[] { "Status_StatusId" });
            DropIndex("public.Devices", new[] { "Room_RoomId" });
            DropIndex("public.Devices", new[] { "Person_PersId" });
            DropIndex("public.DeviceHistory", new[] { "Person_PersId" });
            DropIndex("public.DeviceHistory", new[] { "Device_DeviceId" });
            DropIndex("public.Components", new[] { "DeviceType_TypeId" });
            DropTable("public.Orders");
            DropTable("public.OrderItems");
            DropTable("public.DeviceMeta");
            DropTable("public.Status");
            DropTable("public.Rooms");
            DropTable("public.Roles");
            DropTable("public.Persons");
            DropTable("public.Devices");
            DropTable("public.DeviceHistory");
            DropTable("public.DeviceTypes");
            DropTable("public.Components");
        }
    }
}
