using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HwInf.DataAccess.Migrations
{
    public partial class Initial_Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accessories",
                columns: table => new
                {
                    AccessoryId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    Slug = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accessories", x => x.AccessoryId);
                });

            migrationBuilder.CreateTable(
                name: "DamageStatus",
                columns: table => new
                {
                    StatusId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: false),
                    Slug = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DamageStatus", x => x.StatusId);
                });

            migrationBuilder.CreateTable(
                name: "DeviceStatus",
                columns: table => new
                {
                    StatusId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceStatus", x => x.StatusId);
                });

            migrationBuilder.CreateTable(
                name: "DeviceTypes",
                columns: table => new
                {
                    TypeId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: false),
                    Slug = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceTypes", x => x.TypeId);
                });

            migrationBuilder.CreateTable(
                name: "FieldGroups",
                columns: table => new
                {
                    GroupId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    Slug = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsCountable = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldGroups", x => x.GroupId);
                });

            migrationBuilder.CreateTable(
                name: "OrderStatus",
                columns: table => new
                {
                    StatusId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: false),
                    Slug = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatus", x => x.StatusId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Key = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "DeviceTypeFieldGroup",
                columns: table => new
                {
                    DeviceTypeId = table.Column<int>(nullable: false),
                    FieldGroupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceTypeFieldGroup", x => new { x.DeviceTypeId, x.FieldGroupId });
                    table.ForeignKey(
                        name: "FK_DeviceTypeFieldGroup_DeviceTypes_DeviceTypeId",
                        column: x => x.DeviceTypeId,
                        principalTable: "DeviceTypes",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeviceTypeFieldGroup_FieldGroups_FieldGroupId",
                        column: x => x.FieldGroupId,
                        principalTable: "FieldGroups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Fields",
                columns: table => new
                {
                    FieldId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    Slug = table.Column<string>(nullable: true),
                    FieldGroupGroupId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fields", x => x.FieldId);
                    table.ForeignKey(
                        name: "FK_Fields_FieldGroups_FieldGroupGroupId",
                        column: x => x.FieldGroupGroupId,
                        principalTable: "FieldGroups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    PersId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Tel = table.Column<string>(nullable: true),
                    Uid = table.Column<string>(nullable: false),
                    RoleId = table.Column<int>(nullable: false),
                    Room = table.Column<string>(nullable: true),
                    Studiengang = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.PersId);
                    table.ForeignKey(
                        name: "FK_Persons_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    DeviceId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    Notes = table.Column<string>(nullable: true),
                    InvNum = table.Column<string>(nullable: true),
                    Brand = table.Column<string>(nullable: true),
                    PersonPersId = table.Column<int>(nullable: true),
                    Room = table.Column<string>(nullable: true),
                    StatusId = table.Column<int>(nullable: true),
                    TypeId = table.Column<int>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    DeviceGroupSlug = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.DeviceId);
                    table.ForeignKey(
                        name: "FK_Devices_Persons_PersonPersId",
                        column: x => x.PersonPersId,
                        principalTable: "Persons",
                        principalColumn: "PersId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Devices_DeviceStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "DeviceStatus",
                        principalColumn: "StatusId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Devices_DeviceTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "DeviceTypes",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    From = table.Column<DateTime>(nullable: false),
                    To = table.Column<DateTime>(nullable: false),
                    EntleiherPersId = table.Column<int>(nullable: true),
                    VerwalterPersId = table.Column<int>(nullable: true),
                    OrderGuid = table.Column<Guid>(nullable: false),
                    OrderReason = table.Column<string>(nullable: false),
                    OrderStatusStatusId = table.Column<int>(nullable: false),
                    ReturnDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Persons_EntleiherPersId",
                        column: x => x.EntleiherPersId,
                        principalTable: "Persons",
                        principalColumn: "PersId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_OrderStatus_OrderStatusStatusId",
                        column: x => x.OrderStatusStatusId,
                        principalTable: "OrderStatus",
                        principalColumn: "StatusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Persons_VerwalterPersId",
                        column: x => x.VerwalterPersId,
                        principalTable: "Persons",
                        principalColumn: "PersId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Damages",
                columns: table => new
                {
                    DamageId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    CausePersId = table.Column<int>(nullable: true),
                    ReporterPersId = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: false),
                    DeviceId = table.Column<int>(nullable: true),
                    DamageStatusStatusId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Damages", x => x.DamageId);
                    table.ForeignKey(
                        name: "FK_Damages_Persons_CausePersId",
                        column: x => x.CausePersId,
                        principalTable: "Persons",
                        principalColumn: "PersId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Damages_DamageStatus_DamageStatusStatusId",
                        column: x => x.DamageStatusStatusId,
                        principalTable: "DamageStatus",
                        principalColumn: "StatusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Damages_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Damages_Persons_ReporterPersId",
                        column: x => x.ReporterPersId,
                        principalTable: "Persons",
                        principalColumn: "PersId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DeviceMeta",
                columns: table => new
                {
                    MetaId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    FieldName = table.Column<string>(nullable: false),
                    FieldSlug = table.Column<string>(nullable: false),
                    FieldGroupName = table.Column<string>(nullable: false),
                    FieldGroupSlug = table.Column<string>(nullable: false),
                    MetaValue = table.Column<string>(nullable: false),
                    DeviceId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceMeta", x => x.MetaId);
                    table.ForeignKey(
                        name: "FK_DeviceMeta_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    ItemId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    DeviceId = table.Column<int>(nullable: true),
                    From = table.Column<DateTime>(nullable: false),
                    To = table.Column<DateTime>(nullable: false),
                    ReturnDate = table.Column<DateTime>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    EntleiherPersId = table.Column<int>(nullable: true),
                    VerwalterPersId = table.Column<int>(nullable: true),
                    IsDeclined = table.Column<bool>(nullable: false),
                    Accessories = table.Column<string>(nullable: true),
                    OrderId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.ItemId);
                    table.ForeignKey(
                        name: "FK_OrderItems_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItems_Persons_EntleiherPersId",
                        column: x => x.EntleiherPersId,
                        principalTable: "Persons",
                        principalColumn: "PersId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItems_Persons_VerwalterPersId",
                        column: x => x.VerwalterPersId,
                        principalTable: "Persons",
                        principalColumn: "PersId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "DamageStatus",
                columns: new[] { "StatusId", "Name", "Slug" },
                values: new object[,]
                {
                    { 1, "Gemeldet", "gemeldet" },
                    { 2, "Behoben", "behoben" },
                    { 3, "Dauerhaft", "dauerhaft" }
                });

            migrationBuilder.InsertData(
                table: "DeviceStatus",
                columns: new[] { "StatusId", "Description" },
                values: new object[,]
                {
                    { 1, "Verfügbar" },
                    { 2, "Ausgeliehen" },
                    { 3, "In Reparatur" },
                    { 4, "Archiviert" },
                    { 5, "Nicht verleihbar" },
                    { 6, "Präsentations-/Bachelorarbeitsgerät" }
                });

            migrationBuilder.InsertData(
                table: "OrderStatus",
                columns: new[] { "StatusId", "Name", "Slug" },
                values: new object[,]
                {
                    { 5, "Ausgeliehen", "ausgeliehen" },
                    { 4, "Abgeschlossen", "abgeschlossen" },
                    { 6, "Abgebrochen", "abgebrochen" },
                    { 2, "Akzeptiert", "akzeptiert" },
                    { 1, "Offen", "offen" },
                    { 3, "Abgelehnt", "abgelehnt" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "Name" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "User" },
                    { 3, "Verwalter" }
                });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Key", "Value" },
                values: new object[,]
                {
                    { "decline_mail_above", "oben" },
                    { "accept_mail_below", "unten" },
                    { "accept_mail_above", "oben" },
                    { "new_order_mail", "Neue Anfrage zu einem ihrer Geräte" },
                    { "ws_end", "31.01" },
                    { "ws_start", "25.10" },
                    { "ss_end", "30.06" },
                    { "ss_start", "15.02" },
                    { "decline_mail_below", "unten" },
                    { "reminder_mail", "bitte zurückbringen" },
                    { "days_before_reminder", "7" }
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "PersId", "Email", "LastName", "Name", "RoleId", "Room", "Studiengang", "Tel", "Uid" },
                values: new object[,]
                {
                    { 1, "jan.calanog@technikum-wien.at", "Calanog", "Jan", 1, null, null, null, "if15b042" },
                    { 2, "tobias.schlachter@technikum-wien.at", "Schlachter", "Tobias", 1, null, null, null, "if15b032" },
                    { 3, "valentin.sagl@technikum-wien.at", "Sagl", "Valentin", 1, null, null, null, "if15b030" },
                    { 4, "sebastian.slowak@technikum-wien.at", "Slowak", "Sebastian", 1, null, null, null, "if15b049" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Damages_CausePersId",
                table: "Damages",
                column: "CausePersId");

            migrationBuilder.CreateIndex(
                name: "IX_Damages_DamageStatusStatusId",
                table: "Damages",
                column: "DamageStatusStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Damages_DeviceId",
                table: "Damages",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Damages_ReporterPersId",
                table: "Damages",
                column: "ReporterPersId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceMeta_DeviceId",
                table: "DeviceMeta",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_PersonPersId",
                table: "Devices",
                column: "PersonPersId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_StatusId",
                table: "Devices",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_TypeId",
                table: "Devices",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceTypeFieldGroup_FieldGroupId",
                table: "DeviceTypeFieldGroup",
                column: "FieldGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Fields_FieldGroupGroupId",
                table: "Fields",
                column: "FieldGroupGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_DeviceId",
                table: "OrderItems",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_EntleiherPersId",
                table: "OrderItems",
                column: "EntleiherPersId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_VerwalterPersId",
                table: "OrderItems",
                column: "VerwalterPersId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_EntleiherPersId",
                table: "Orders",
                column: "EntleiherPersId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderStatusStatusId",
                table: "Orders",
                column: "OrderStatusStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_VerwalterPersId",
                table: "Orders",
                column: "VerwalterPersId");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_RoleId",
                table: "Persons",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_Uid",
                table: "Persons",
                column: "Uid",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accessories");

            migrationBuilder.DropTable(
                name: "Damages");

            migrationBuilder.DropTable(
                name: "DeviceMeta");

            migrationBuilder.DropTable(
                name: "DeviceTypeFieldGroup");

            migrationBuilder.DropTable(
                name: "Fields");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "DamageStatus");

            migrationBuilder.DropTable(
                name: "FieldGroups");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "DeviceStatus");

            migrationBuilder.DropTable(
                name: "DeviceTypes");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "OrderStatus");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
