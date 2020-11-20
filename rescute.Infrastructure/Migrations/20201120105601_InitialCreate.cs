using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace rescute.Infrastructure.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "rescute");

            migrationBuilder.CreateTable(
                name: "Samaritans",
                schema: "rescute",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    FirstName_Value = table.Column<string>(nullable: true),
                    FirstName_MaxLength = table.Column<int>(nullable: true),
                    LastName_Value = table.Column<string>(nullable: true),
                    LastName_MaxLength = table.Column<int>(nullable: true),
                    Mobile_IsMobile = table.Column<bool>(nullable: true),
                    Mobile_Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Samaritans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                schema: "rescute",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    SamaritanId = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    CaseNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reports_Samaritans_SamaritanId",
                        column: x => x.SamaritanId,
                        principalSchema: "rescute",
                        principalTable: "Samaritans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Animals",
                schema: "rescute",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Type_Name = table.Column<string>(nullable: true),
                    ReportId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Animals_Reports_ReportId",
                        column: x => x.ReportId,
                        principalSchema: "rescute",
                        principalTable: "Reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReportLogs",
                schema: "rescute",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    EventDate = table.Column<DateTime>(nullable: false),
                    SamaritanId = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    ReportId = table.Column<string>(nullable: true),
                    RepliesToId = table.Column<string>(nullable: true),
                    BillId = table.Column<string>(nullable: true),
                    Amount = table.Column<long>(nullable: true),
                    Total = table.Column<long>(nullable: true),
                    ContributionRequested = table.Column<bool>(nullable: true),
                    EventLocation_Latitude = table.Column<double>(nullable: true),
                    EventLocation_Longitude = table.Column<double>(nullable: true),
                    ToLocation_Latitude = table.Column<double>(nullable: true),
                    ToLocation_Longitude = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportLogs_ReportLogs_RepliesToId",
                        column: x => x.RepliesToId,
                        principalSchema: "rescute",
                        principalTable: "ReportLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReportLogs_ReportLogs_BillId",
                        column: x => x.BillId,
                        principalSchema: "rescute",
                        principalTable: "ReportLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReportLogs_Reports_ReportId",
                        column: x => x.ReportId,
                        principalSchema: "rescute",
                        principalTable: "Reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReportLogs_Samaritans_SamaritanId",
                        column: x => x.SamaritanId,
                        principalSchema: "rescute",
                        principalTable: "Samaritans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                schema: "rescute",
                columns: table => new
                {
                    DocumentedLogItemId = table.Column<string>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(nullable: true),
                    Type_Type = table.Column<string>(nullable: true),
                    Alias = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => new { x.DocumentedLogItemId, x.Id });
                    table.ForeignKey(
                        name: "FK_Documents_ReportLogs_DocumentedLogItemId",
                        column: x => x.DocumentedLogItemId,
                        principalSchema: "rescute",
                        principalTable: "ReportLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Animals_ReportId",
                schema: "rescute",
                table: "Animals",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportLogs_RepliesToId",
                schema: "rescute",
                table: "ReportLogs",
                column: "RepliesToId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportLogs_BillId",
                schema: "rescute",
                table: "ReportLogs",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportLogs_ReportId",
                schema: "rescute",
                table: "ReportLogs",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportLogs_SamaritanId",
                schema: "rescute",
                table: "ReportLogs",
                column: "SamaritanId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_SamaritanId",
                schema: "rescute",
                table: "Reports",
                column: "SamaritanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Animals",
                schema: "rescute");

            migrationBuilder.DropTable(
                name: "Documents",
                schema: "rescute");

            migrationBuilder.DropTable(
                name: "ReportLogs",
                schema: "rescute");

            migrationBuilder.DropTable(
                name: "Reports",
                schema: "rescute");

            migrationBuilder.DropTable(
                name: "Samaritans",
                schema: "rescute");
        }
    }
}
