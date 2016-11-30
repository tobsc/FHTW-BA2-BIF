namespace HwInf.Common.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitalCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.DeviceMeta",
                c => new
                    {
                        MetaId = c.Int(nullable: false, identity: true),
                        MetaKey = c.String(),
                        MetaValue = c.String(),
                        Device_DeviceId = c.Int(),
                        DeviceType_TypeId = c.Int(),
                    })
                .PrimaryKey(t => t.MetaId)
                .ForeignKey("public.Device", t => t.Device_DeviceId)
                .ForeignKey("public.DeviceType", t => t.DeviceType_TypeId)
                .Index(t => t.Device_DeviceId)
                .Index(t => t.DeviceType_TypeId);
            
            CreateTable(
                "public.Device",
                c => new
                    {
                        DeviceId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        InvNum = c.String(),
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
                        Name = c.String(),
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
