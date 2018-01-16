namespace HwInf.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Db_Changes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("public.Components", "DeviceType_TypeId", "public.DeviceTypes");
            DropForeignKey("public.DeviceHistory", "Device_DeviceId", "public.Devices");
            DropForeignKey("public.DeviceHistory", "Person_PersId", "public.Persons");
            DropForeignKey("public.DeviceMeta", "Component_CompId", "public.Components");
            DropForeignKey("public.Devices", "Person_PersId", "public.Persons");
            DropForeignKey("public.Devices", "Status_StatusId", "public.DeviceStatus");
            DropForeignKey("public.Devices", "Type_TypeId", "public.DeviceTypes");
            DropForeignKey("public.OrderItems", "Device_DeviceId", "public.Devices");
            DropForeignKey("public.DeviceMeta", "Device_DeviceId", "public.Devices");
            DropForeignKey("public.OrderItems", "Order_OrderId", "public.Orders");
            DropForeignKey("public.Orders", "Owner_PersId", "public.Persons");
            DropForeignKey("public.Orders", "Person_PersId", "public.Persons");
            DropIndex("public.Components", new[] { "DeviceType_TypeId" });
            DropIndex("public.DeviceHistory", new[] { "Device_DeviceId" });
            DropIndex("public.DeviceHistory", new[] { "Person_PersId" });
            DropIndex("public.Devices", new[] { "Person_PersId" });
            DropIndex("public.Devices", new[] { "Status_StatusId" });
            DropIndex("public.Devices", new[] { "Type_TypeId" });
            DropIndex("public.DeviceMeta", new[] { "Component_CompId" });
            DropIndex("public.DeviceMeta", new[] { "Device_DeviceId" });
            DropIndex("public.OrderItems", new[] { "Device_DeviceId" });
            DropIndex("public.OrderItems", new[] { "Order_OrderId" });
            DropIndex("public.Orders", new[] { "Owner_PersId" });
            DropIndex("public.Orders", new[] { "Person_PersId" });
            RenameColumn(table: "public.Orders", name: "Owner_PersId", newName: "Entleiher_PersId");
            RenameColumn(table: "public.Orders", name: "Person_PersId", newName: "Verwalter_PersId");
            RenameColumn(table: "public.Orders", name: "Status_StatusId", newName: "OrderStatus_StatusId");
            RenameIndex(table: "public.Orders", name: "IX_Status_StatusId", newName: "IX_OrderStatus_StatusId");
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
                "public.DamageStatus",
                c => new
                    {
                        StatusId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Slug = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.StatusId);
            
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
            
            AddColumn("public.DeviceTypes", "Name", c => c.String(nullable: false));
            AddColumn("public.DeviceTypes", "Slug", c => c.String());
            AddColumn("public.DeviceTypes", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("public.Devices", "Notes", c => c.String());
            AddColumn("public.Devices", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("public.Devices", "DeviceGroupSlug", c => c.String());
            AddColumn("public.Persons", "Studiengang", c => c.String());
            AddColumn("public.DeviceMeta", "FieldName", c => c.String(nullable: false));
            AddColumn("public.DeviceMeta", "FieldSlug", c => c.String(nullable: false));
            AddColumn("public.DeviceMeta", "FieldGroupName", c => c.String(nullable: false));
            AddColumn("public.DeviceMeta", "FieldGroupSlug", c => c.String(nullable: false));
            AddColumn("public.OrderItems", "From", c => c.DateTime(nullable: false));
            AddColumn("public.OrderItems", "To", c => c.DateTime(nullable: false));
            AddColumn("public.OrderItems", "ReturnDate", c => c.DateTime(nullable: false));
            AddColumn("public.OrderItems", "CreateDate", c => c.DateTime(nullable: false));
            AddColumn("public.OrderItems", "IsDeclined", c => c.Boolean(nullable: false));
            AddColumn("public.OrderItems", "Accessories", c => c.String());
            AddColumn("public.OrderItems", "Entleiher_PersId", c => c.Int());
            AddColumn("public.OrderItems", "Verwalter_PersId", c => c.Int());
            AddColumn("public.Orders", "OrderGuid", c => c.Guid(nullable: false));
            AddColumn("public.Orders", "OrderReason", c => c.String(nullable: false));
            AddColumn("public.OrderStatus", "Name", c => c.String(nullable: false));
            AddColumn("public.OrderStatus", "Slug", c => c.String(nullable: false));
            AlterColumn("public.Devices", "Name", c => c.String());
            AlterColumn("public.Devices", "Brand", c => c.String());
            AlterColumn("public.Devices", "Person_PersId", c => c.Int());
            AlterColumn("public.Devices", "Status_StatusId", c => c.Int());
            AlterColumn("public.Devices", "Type_TypeId", c => c.Int());
            AlterColumn("public.DeviceMeta", "Device_DeviceId", c => c.Int());
            AlterColumn("public.OrderItems", "Device_DeviceId", c => c.Int());
            AlterColumn("public.OrderItems", "Order_OrderId", c => c.Int());
            AlterColumn("public.Orders", "Entleiher_PersId", c => c.Int());
            AlterColumn("public.Orders", "Verwalter_PersId", c => c.Int());
            CreateIndex("public.Persons", "Uid", unique: true);
            CreateIndex("public.Devices", "Person_PersId");
            CreateIndex("public.Devices", "Status_StatusId");
            CreateIndex("public.Devices", "Type_TypeId");
            CreateIndex("public.DeviceMeta", "Device_DeviceId");
            CreateIndex("public.OrderItems", "Device_DeviceId");
            CreateIndex("public.OrderItems", "Entleiher_PersId");
            CreateIndex("public.OrderItems", "Verwalter_PersId");
            CreateIndex("public.OrderItems", "Order_OrderId");
            CreateIndex("public.Orders", "Entleiher_PersId");
            CreateIndex("public.Orders", "Verwalter_PersId");
            AddForeignKey("public.OrderItems", "Entleiher_PersId", "public.Persons", "PersId");
            AddForeignKey("public.OrderItems", "Verwalter_PersId", "public.Persons", "PersId");
            AddForeignKey("public.Devices", "Person_PersId", "public.Persons", "PersId");
            AddForeignKey("public.Devices", "Status_StatusId", "public.DeviceStatus", "StatusId");
            AddForeignKey("public.Devices", "Type_TypeId", "public.DeviceTypes", "TypeId");
            AddForeignKey("public.OrderItems", "Device_DeviceId", "public.Devices", "DeviceId");
            AddForeignKey("public.DeviceMeta", "Device_DeviceId", "public.Devices", "DeviceId");
            AddForeignKey("public.OrderItems", "Order_OrderId", "public.Orders", "OrderId");
            AddForeignKey("public.Orders", "Entleiher_PersId", "public.Persons", "PersId");
            AddForeignKey("public.Orders", "Verwalter_PersId", "public.Persons", "PersId");
            DropColumn("public.DeviceTypes", "Description");
            DropColumn("public.DeviceMeta", "Component_CompId");
            DropColumn("public.OrderStatus", "Description");
            DropTable("public.Components");
            DropTable("public.DeviceHistory");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.HistId);
            
            CreateTable(
                "public.Components",
                c => new
                    {
                        CompId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        FieldType = c.String(),
                        DeviceType_TypeId = c.Int(),
                    })
                .PrimaryKey(t => t.CompId);
            
            AddColumn("public.OrderStatus", "Description", c => c.String(nullable: false));
            AddColumn("public.DeviceMeta", "Component_CompId", c => c.Int(nullable: false));
            AddColumn("public.DeviceTypes", "Description", c => c.String(nullable: false));
            DropForeignKey("public.Orders", "Verwalter_PersId", "public.Persons");
            DropForeignKey("public.Orders", "Entleiher_PersId", "public.Persons");
            DropForeignKey("public.OrderItems", "Order_OrderId", "public.Orders");
            DropForeignKey("public.DeviceMeta", "Device_DeviceId", "public.Devices");
            DropForeignKey("public.OrderItems", "Device_DeviceId", "public.Devices");
            DropForeignKey("public.Devices", "Type_TypeId", "public.DeviceTypes");
            DropForeignKey("public.Devices", "Status_StatusId", "public.DeviceStatus");
            DropForeignKey("public.Devices", "Person_PersId", "public.Persons");
            DropForeignKey("public.OrderItems", "Verwalter_PersId", "public.Persons");
            DropForeignKey("public.OrderItems", "Entleiher_PersId", "public.Persons");
            DropForeignKey("public.Damages", "Reporter_PersId", "public.Persons");
            DropForeignKey("public.Damages", "Device_DeviceId", "public.Devices");
            DropForeignKey("public.Fields", "FieldGroup_GroupId", "public.FieldGroups");
            DropForeignKey("public.FieldGroupDeviceType", "DeviceType_TypeId", "public.DeviceTypes");
            DropForeignKey("public.FieldGroupDeviceType", "FieldGroup_GroupId", "public.FieldGroups");
            DropForeignKey("public.Damages", "DamageStatus_StatusId", "public.DamageStatus");
            DropForeignKey("public.Damages", "Cause_PersId", "public.Persons");
            DropIndex("public.FieldGroupDeviceType", new[] { "DeviceType_TypeId" });
            DropIndex("public.FieldGroupDeviceType", new[] { "FieldGroup_GroupId" });
            DropIndex("public.Settings", new[] { "Key" });
            DropIndex("public.Orders", new[] { "Verwalter_PersId" });
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
            DropIndex("public.Persons", new[] { "Uid" });
            DropIndex("public.Damages", new[] { "Reporter_PersId" });
            DropIndex("public.Damages", new[] { "Device_DeviceId" });
            DropIndex("public.Damages", new[] { "DamageStatus_StatusId" });
            DropIndex("public.Damages", new[] { "Cause_PersId" });
            AlterColumn("public.Orders", "Verwalter_PersId", c => c.Int(nullable: false));
            AlterColumn("public.Orders", "Entleiher_PersId", c => c.Int(nullable: false));
            AlterColumn("public.OrderItems", "Order_OrderId", c => c.Int(nullable: false));
            AlterColumn("public.OrderItems", "Device_DeviceId", c => c.Int(nullable: false));
            AlterColumn("public.DeviceMeta", "Device_DeviceId", c => c.Int(nullable: false));
            AlterColumn("public.Devices", "Type_TypeId", c => c.Int(nullable: false));
            AlterColumn("public.Devices", "Status_StatusId", c => c.Int(nullable: false));
            AlterColumn("public.Devices", "Person_PersId", c => c.Int(nullable: false));
            AlterColumn("public.Devices", "Brand", c => c.String(nullable: false));
            AlterColumn("public.Devices", "Name", c => c.String(nullable: false));
            DropColumn("public.OrderStatus", "Slug");
            DropColumn("public.OrderStatus", "Name");
            DropColumn("public.Orders", "OrderReason");
            DropColumn("public.Orders", "OrderGuid");
            DropColumn("public.OrderItems", "Verwalter_PersId");
            DropColumn("public.OrderItems", "Entleiher_PersId");
            DropColumn("public.OrderItems", "Accessories");
            DropColumn("public.OrderItems", "IsDeclined");
            DropColumn("public.OrderItems", "CreateDate");
            DropColumn("public.OrderItems", "ReturnDate");
            DropColumn("public.OrderItems", "To");
            DropColumn("public.OrderItems", "From");
            DropColumn("public.DeviceMeta", "FieldGroupSlug");
            DropColumn("public.DeviceMeta", "FieldGroupName");
            DropColumn("public.DeviceMeta", "FieldSlug");
            DropColumn("public.DeviceMeta", "FieldName");
            DropColumn("public.Persons", "Studiengang");
            DropColumn("public.Devices", "DeviceGroupSlug");
            DropColumn("public.Devices", "IsActive");
            DropColumn("public.Devices", "Notes");
            DropColumn("public.DeviceTypes", "IsActive");
            DropColumn("public.DeviceTypes", "Slug");
            DropColumn("public.DeviceTypes", "Name");
            DropTable("public.FieldGroupDeviceType");
            DropTable("public.Settings");
            DropTable("public.Fields");
            DropTable("public.FieldGroups");
            DropTable("public.DamageStatus");
            DropTable("public.Damages");
            DropTable("public.Accessories");
            RenameIndex(table: "public.Orders", name: "IX_OrderStatus_StatusId", newName: "IX_Status_StatusId");
            RenameColumn(table: "public.Orders", name: "OrderStatus_StatusId", newName: "Status_StatusId");
            RenameColumn(table: "public.Orders", name: "Verwalter_PersId", newName: "Person_PersId");
            RenameColumn(table: "public.Orders", name: "Entleiher_PersId", newName: "Owner_PersId");
            CreateIndex("public.Orders", "Person_PersId");
            CreateIndex("public.Orders", "Owner_PersId");
            CreateIndex("public.OrderItems", "Order_OrderId");
            CreateIndex("public.OrderItems", "Device_DeviceId");
            CreateIndex("public.DeviceMeta", "Device_DeviceId");
            CreateIndex("public.DeviceMeta", "Component_CompId");
            CreateIndex("public.Devices", "Type_TypeId");
            CreateIndex("public.Devices", "Status_StatusId");
            CreateIndex("public.Devices", "Person_PersId");
            CreateIndex("public.DeviceHistory", "Person_PersId");
            CreateIndex("public.DeviceHistory", "Device_DeviceId");
            CreateIndex("public.Components", "DeviceType_TypeId");
            AddForeignKey("public.Orders", "Person_PersId", "public.Persons", "PersId", cascadeDelete: true);
            AddForeignKey("public.Orders", "Owner_PersId", "public.Persons", "PersId", cascadeDelete: true);
            AddForeignKey("public.OrderItems", "Order_OrderId", "public.Orders", "OrderId", cascadeDelete: true);
            AddForeignKey("public.DeviceMeta", "Device_DeviceId", "public.Devices", "DeviceId", cascadeDelete: true);
            AddForeignKey("public.OrderItems", "Device_DeviceId", "public.Devices", "DeviceId", cascadeDelete: true);
            AddForeignKey("public.Devices", "Type_TypeId", "public.DeviceTypes", "TypeId", cascadeDelete: true);
            AddForeignKey("public.Devices", "Status_StatusId", "public.DeviceStatus", "StatusId", cascadeDelete: true);
            AddForeignKey("public.Devices", "Person_PersId", "public.Persons", "PersId", cascadeDelete: true);
            AddForeignKey("public.DeviceMeta", "Component_CompId", "public.Components", "CompId", cascadeDelete: true);
            AddForeignKey("public.DeviceHistory", "Person_PersId", "public.Persons", "PersId", cascadeDelete: true);
            AddForeignKey("public.DeviceHistory", "Device_DeviceId", "public.Devices", "DeviceId", cascadeDelete: true);
            AddForeignKey("public.Components", "DeviceType_TypeId", "public.DeviceTypes", "TypeId");
        }
    }
}
