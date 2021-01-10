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
                name: "Animals",
                schema: "rescute",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CaseNumber = table.Column<string>(nullable: true),
                    Type_Name = table.Column<string>(nullable: true),
                    RegistrationDate = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    BirthCertificateId = table.Column<string>(nullable: true),
                    IntroducedById = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Animals_Samaritans_IntroducedById",
                        column: x => x.IntroducedById,
                        principalSchema: "rescute",
                        principalTable: "Samaritans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AnimalAttachments",
                schema: "rescute",
                columns: table => new
                {
                    AnimalId = table.Column<string>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(nullable: true),
                    Type_Type = table.Column<string>(nullable: true),
                    Alias = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalAttachments", x => new { x.AnimalId, x.Id });
                    table.ForeignKey(
                        name: "FK_AnimalAttachments_Animals_AnimalId",
                        column: x => x.AnimalId,
                        principalSchema: "rescute",
                        principalTable: "Animals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnimalLogs",
                schema: "rescute",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    EventDate = table.Column<DateTime>(nullable: false),
                    SamaritanId = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    AnimalId = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    RepliesToId = table.Column<string>(nullable: true),
                    Total = table.Column<long>(nullable: true),
                    ContributionRequested = table.Column<bool>(nullable: true),
                    EventLocation_Latitude = table.Column<double>(nullable: true),
                    EventLocation_Longitude = table.Column<double>(nullable: true),
                    BillId = table.Column<string>(nullable: true),
                    Amount = table.Column<long>(nullable: true),
                    ToLocation_Latitude = table.Column<double>(nullable: true),
                    ToLocation_Longitude = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnimalLogs_AnimalLogs_RepliesToId",
                        column: x => x.RepliesToId,
                        principalSchema: "rescute",
                        principalTable: "AnimalLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AnimalLogs_Animals_AnimalId",
                        column: x => x.AnimalId,
                        principalSchema: "rescute",
                        principalTable: "Animals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AnimalLogs_Samaritans_SamaritanId",
                        column: x => x.SamaritanId,
                        principalSchema: "rescute",
                        principalTable: "Samaritans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AnimalLogs_AnimalLogs_BillId",
                        column: x => x.BillId,
                        principalSchema: "rescute",
                        principalTable: "AnimalLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AnimalLogAttachments",
                schema: "rescute",
                columns: table => new
                {
                    LogItemWithAttachmentsId = table.Column<string>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(nullable: true),
                    Type_Type = table.Column<string>(nullable: true),
                    Alias = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalLogAttachments", x => new { x.LogItemWithAttachmentsId, x.Id });
                    table.ForeignKey(
                        name: "FK_AnimalLogAttachments_AnimalLogs_LogItemWithAttachmentsId",
                        column: x => x.LogItemWithAttachmentsId,
                        principalSchema: "rescute",
                        principalTable: "AnimalLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnimalLogs_RepliesToId",
                schema: "rescute",
                table: "AnimalLogs",
                column: "RepliesToId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalLogs_AnimalId",
                schema: "rescute",
                table: "AnimalLogs",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalLogs_SamaritanId",
                schema: "rescute",
                table: "AnimalLogs",
                column: "SamaritanId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalLogs_BillId",
                schema: "rescute",
                table: "AnimalLogs",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "IX_Animals_IntroducedById",
                schema: "rescute",
                table: "Animals",
                column: "IntroducedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimalAttachments",
                schema: "rescute");

            migrationBuilder.DropTable(
                name: "AnimalLogAttachments",
                schema: "rescute");

            migrationBuilder.DropTable(
                name: "AnimalLogs",
                schema: "rescute");

            migrationBuilder.DropTable(
                name: "Animals",
                schema: "rescute");

            migrationBuilder.DropTable(
                name: "Samaritans",
                schema: "rescute");
        }
    }
}
