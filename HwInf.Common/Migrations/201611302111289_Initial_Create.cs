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
                        Name = c.String(nullable: false),
                        InvNum = c.String(),
                        Hersteller = c.String(),
                        Status = c.Int(nullable: false),
                        Type_TypeId = c.Int(),
                    })
                .PrimaryKey(t => t.DeviceId)
                .ForeignKey("public.DeviceType", t => t.Type_TypeId)
                .Index(t => t.Type_TypeId);
            
            CreateTable(
                "public.DeviceType",
                c => new
                    {
                        TypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.TypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("public.DeviceMeta", "DeviceType_TypeId", "public.DeviceType");
            DropForeignKey("public.DeviceMeta", "Device_DeviceId", "public.Device");
            DropForeignKey("public.Device", "Type_TypeId", "public.DeviceType");
            DropIndex("public.Device", new[] { "Type_TypeId" });
            DropIndex("public.DeviceMeta", new[] { "DeviceType_TypeId" });
            DropIndex("public.DeviceMeta", new[] { "Device_DeviceId" });
            DropTable("public.DeviceType");
            DropTable("public.Device");
            DropTable("public.DeviceMeta");
        }
    }
}
