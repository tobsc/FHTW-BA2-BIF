namespace HwInf.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial_Create : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.Accessories",
                c => new
                    {
                        AccessoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Slug = c.String(),
                    })
                .PrimaryKey(t => t.AccessoryId);
            
            CreateTable(
                "public.Damages",
                c => new
                    {
                        DamageId = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Description = c.String(nullable: false),
                        Cause_PersId = c.Int(),
                        DamageStatus_StatusId = c.Int(nullable: false),
                        Device_DeviceId = c.Int(),
                        Reporter_PersId = c.Int(),
                    })
                .PrimaryKey(t => t.DamageId)
                .ForeignKey("public.Persons", t => t.Cause_PersId)
                .ForeignKey("public.DamageStatus", t => t.DamageStatus_StatusId, cascadeDelete: true)
                .ForeignKey("public.Devices", t => t.Device_DeviceId)
                .ForeignKey("public.Persons", t => t.Reporter_PersId)
                .Index(t => t.Cause_PersId)
                .Index(t => t.DamageStatus_StatusId)
                .Index(t => t.Device_DeviceId)
                .Index(t => t.Reporter_PersId);
            
            CreateTable(
                "public.Persons",
                c => new
                    {
                        PersId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Tel = c.String(),
                        Uid = c.String(nullable: false),
                        Room = c.String(),
                        Studiengang = c.String(),
                        Role_RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PersId)
                .ForeignKey("public.Roles", t => t.Role_RoleId, cascadeDelete: true)
                .Index(t => t.Uid, unique: true)
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
                "public.DamageStatus",
                c => new
                    {
                        StatusId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Slug = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.StatusId);
            
            CreateTable(
                "public.Devices",
                c => new
                    {
                        DeviceId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Notes = c.String(),
                        InvNum = c.String(),
                        Brand = c.String(),
                        Room = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        DeviceGroupSlug = c.String(),
                        Person_PersId = c.Int(),
                        Status_StatusId = c.Int(),
                        Type_TypeId = c.Int(),
                    })
                .PrimaryKey(t => t.DeviceId)
                .ForeignKey("public.Persons", t => t.Person_PersId)
                .ForeignKey("public.DeviceStatus", t => t.Status_StatusId)
                .ForeignKey("public.DeviceTypes", t => t.Type_TypeId)
                .Index(t => t.Person_PersId)
                .Index(t => t.Status_StatusId)
                .Index(t => t.Type_TypeId);
            
            CreateTable(
                "public.DeviceMeta",
                c => new
                    {
                        MetaId = c.Int(nullable: false, identity: true),
                        FieldName = c.String(nullable: false),
                        FieldSlug = c.String(nullable: false),
                        FieldGroupName = c.String(nullable: false),
                        FieldGroupSlug = c.String(nullable: false),
                        MetaValue = c.String(nullable: false),
                        Device_DeviceId = c.Int(),
                    })
                .PrimaryKey(t => t.MetaId)
                .ForeignKey("public.Devices", t => t.Device_DeviceId)
                .Index(t => t.Device_DeviceId);
            
            CreateTable(
                "public.DeviceStatus",
                c => new
                    {
                        StatusId = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.StatusId);
            
            CreateTable(
                "public.DeviceTypes",
                c => new
                    {
                        TypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Slug = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.TypeId);
            
            CreateTable(
                "public.FieldGroups",
                c => new
                    {
                        GroupId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Slug = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        IsCountable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.GroupId);
            
            CreateTable(
                "public.Fields",
                c => new
                    {
                        FieldId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Slug = c.String(),
                        FieldGroup_GroupId = c.Int(),
                    })
                .PrimaryKey(t => t.FieldId)
                .ForeignKey("public.FieldGroups", t => t.FieldGroup_GroupId)
                .Index(t => t.FieldGroup_GroupId);
            
            CreateTable(
                "public.OrderItems",
                c => new
                    {
                        ItemId = c.Int(nullable: false, identity: true),
                        From = c.DateTime(nullable: false),
                        To = c.DateTime(nullable: false),
                        ReturnDate = c.DateTime(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        IsDeclined = c.Boolean(nullable: false),
                        Accessories = c.String(),
                        Device_DeviceId = c.Int(),
                        Entleiher_PersId = c.Int(),
                        Verwalter_PersId = c.Int(),
                        Order_OrderId = c.Int(),
                    })
                .PrimaryKey(t => t.ItemId)
                .ForeignKey("public.Devices", t => t.Device_DeviceId)
                .ForeignKey("public.Persons", t => t.Entleiher_PersId)
                .ForeignKey("public.Persons", t => t.Verwalter_PersId)
                .ForeignKey("public.Orders", t => t.Order_OrderId)
                .Index(t => t.Device_DeviceId)
                .Index(t => t.Entleiher_PersId)
                .Index(t => t.Verwalter_PersId)
                .Index(t => t.Order_OrderId);
            
            CreateTable(
                "public.Orders",
                c => new
                    {
                        OrderId = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        From = c.DateTime(nullable: false),
                        To = c.DateTime(nullable: false),
                        OrderGuid = c.Guid(nullable: false),
                        OrderReason = c.String(nullable: false),
                        ReturnDate = c.DateTime(nullable: false),
                        Entleiher_PersId = c.Int(),
                        OrderStatus_StatusId = c.Int(nullable: false),
                        Verwalter_PersId = c.Int(),
                    })
                .PrimaryKey(t => t.OrderId)
                .ForeignKey("public.Persons", t => t.Entleiher_PersId)
                .ForeignKey("public.OrderStatus", t => t.OrderStatus_StatusId, cascadeDelete: true)
                .ForeignKey("public.Persons", t => t.Verwalter_PersId)
                .Index(t => t.Entleiher_PersId)
                .Index(t => t.OrderStatus_StatusId)
                .Index(t => t.Verwalter_PersId);
            
            CreateTable(
                "public.OrderStatus",
                c => new
                    {
                        StatusId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Slug = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.StatusId);
            
            CreateTable(
                "public.Settings",
                c => new
                    {
                        Key = c.String(nullable: false, maxLength: 128),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Key)
                .Index(t => t.Key, unique: true);
            
            CreateTable(
                "public.FieldGroupDeviceType",
                c => new
                    {
                        FieldGroup_GroupId = c.Int(nullable: false),
                        DeviceType_TypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.FieldGroup_GroupId, t.DeviceType_TypeId })
                .ForeignKey("public.FieldGroups", t => t.FieldGroup_GroupId, cascadeDelete: true)
                .ForeignKey("public.DeviceTypes", t => t.DeviceType_TypeId, cascadeDelete: true)
                .Index(t => t.FieldGroup_GroupId)
                .Index(t => t.DeviceType_TypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("public.Orders", "Verwalter_PersId", "public.Persons");
            DropForeignKey("public.Orders", "OrderStatus_StatusId", "public.OrderStatus");
            DropForeignKey("public.OrderItems", "Order_OrderId", "public.Orders");
            DropForeignKey("public.Orders", "Entleiher_PersId", "public.Persons");
            DropForeignKey("public.OrderItems", "Verwalter_PersId", "public.Persons");
            DropForeignKey("public.OrderItems", "Entleiher_PersId", "public.Persons");
            DropForeignKey("public.OrderItems", "Device_DeviceId", "public.Devices");
            DropForeignKey("public.Damages", "Reporter_PersId", "public.Persons");
            DropForeignKey("public.Damages", "Device_DeviceId", "public.Devices");
            DropForeignKey("public.Devices", "Type_TypeId", "public.DeviceTypes");
            DropForeignKey("public.Fields", "FieldGroup_GroupId", "public.FieldGroups");
            DropForeignKey("public.FieldGroupDeviceType", "DeviceType_TypeId", "public.DeviceTypes");
            DropForeignKey("public.FieldGroupDeviceType", "FieldGroup_GroupId", "public.FieldGroups");
            DropForeignKey("public.Devices", "Status_StatusId", "public.DeviceStatus");
            DropForeignKey("public.Devices", "Person_PersId", "public.Persons");
            DropForeignKey("public.DeviceMeta", "Device_DeviceId", "public.Devices");
            DropForeignKey("public.Damages", "DamageStatus_StatusId", "public.DamageStatus");
            DropForeignKey("public.Damages", "Cause_PersId", "public.Persons");
            DropForeignKey("public.Persons", "Role_RoleId", "public.Roles");
            DropIndex("public.FieldGroupDeviceType", new[] { "DeviceType_TypeId" });
            DropIndex("public.FieldGroupDeviceType", new[] { "FieldGroup_GroupId" });
            DropIndex("public.Settings", new[] { "Key" });
            DropIndex("public.Orders", new[] { "Verwalter_PersId" });
            DropIndex("public.Orders", new[] { "OrderStatus_StatusId" });
            DropIndex("public.Orders", new[] { "Entleiher_PersId" });
            DropIndex("public.OrderItems", new[] { "Order_OrderId" });
            DropIndex("public.OrderItems", new[] { "Verwalter_PersId" });
            DropIndex("public.OrderItems", new[] { "Entleiher_PersId" });
            DropIndex("public.OrderItems", new[] { "Device_DeviceId" });
            DropIndex("public.Fields", new[] { "FieldGroup_GroupId" });
            DropIndex("public.DeviceMeta", new[] { "Device_DeviceId" });
            DropIndex("public.Devices", new[] { "Type_TypeId" });
            DropIndex("public.Devices", new[] { "Status_StatusId" });
            DropIndex("public.Devices", new[] { "Person_PersId" });
            DropIndex("public.Persons", new[] { "Role_RoleId" });
            DropIndex("public.Persons", new[] { "Uid" });
            DropIndex("public.Damages", new[] { "Reporter_PersId" });
            DropIndex("public.Damages", new[] { "Device_DeviceId" });
            DropIndex("public.Damages", new[] { "DamageStatus_StatusId" });
            DropIndex("public.Damages", new[] { "Cause_PersId" });
            DropTable("public.FieldGroupDeviceType");
            DropTable("public.Settings");
            DropTable("public.OrderStatus");
            DropTable("public.Orders");
            DropTable("public.OrderItems");
            DropTable("public.Fields");
            DropTable("public.FieldGroups");
            DropTable("public.DeviceTypes");
            DropTable("public.DeviceStatus");
            DropTable("public.DeviceMeta");
            DropTable("public.Devices");
            DropTable("public.DamageStatus");
            DropTable("public.Roles");
            DropTable("public.Persons");
            DropTable("public.Damages");
            DropTable("public.Accessories");
        }
    }
}
