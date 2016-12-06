namespace HwInf.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial_Create : DbMigration
    {
        public override void Up()
        {
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
                .ForeignKey("public.Device", t => t.Device_DeviceId, cascadeDelete: true)
                .ForeignKey("public.DeviceType", t => t.DeviceType_TypeId, cascadeDelete: true)
                .Index(t => t.Device_DeviceId)
                .Index(t => t.DeviceType_TypeId);
            
            CreateTable(
                "public.Device",
                c => new
                    {
                        DeviceId = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                        InvNum = c.String(),
                        Brand = c.String(),
                        Status_StatusId = c.Int(nullable: false),
                        Type_TypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DeviceId)
                .ForeignKey("public.DeviceStatus", t => t.Status_StatusId, cascadeDelete: true)
                .ForeignKey("public.DeviceType", t => t.Type_TypeId, cascadeDelete: true)
                .Index(t => t.Status_StatusId)
                .Index(t => t.Type_TypeId);
            
            CreateTable(
                "public.DeviceStatus",
                c => new
                    {
                        StatusId = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.StatusId);
            
            CreateTable(
                "public.DeviceType",
                c => new
                    {
                        TypeId = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.TypeId);
            
            CreateTable(
                "public.Persons",
                c => new
                    {
                        PersId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Phone = c.String(),
                        uid = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.PersId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("public.DeviceMeta", "DeviceType_TypeId", "public.DeviceType");
            DropForeignKey("public.DeviceMeta", "Device_DeviceId", "public.Device");
            DropForeignKey("public.Device", "Type_TypeId", "public.DeviceType");
            DropForeignKey("public.Device", "Status_StatusId", "public.DeviceStatus");
            DropIndex("public.Device", new[] { "Type_TypeId" });
            DropIndex("public.Device", new[] { "Status_StatusId" });
            DropIndex("public.DeviceMeta", new[] { "DeviceType_TypeId" });
            DropIndex("public.DeviceMeta", new[] { "Device_DeviceId" });
            DropTable("public.Persons");
            DropTable("public.DeviceType");
            DropTable("public.DeviceStatus");
            DropTable("public.Device");
            DropTable("public.DeviceMeta");
        }
    }
}
