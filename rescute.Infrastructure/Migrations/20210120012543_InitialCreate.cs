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
                    RegistrationDate = table.Column<DateTime>(nullable: false),
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
                    Type_Name = table.Column<string>(nullable: true),
                    RegistrationDate = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    BirthCertificateId = table.Column<string>(nullable: true),
                    IntroducedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Animals_Samaritans_IntroducedBy",
                        column: x => x.IntroducedBy,
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
                name: "TimelineEvents",
                schema: "rescute",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    EventDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    AnimalId = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    Total = table.Column<long>(nullable: true),
                    ContributionRequested = table.Column<bool>(nullable: true),
                    EventLocation_Latitude = table.Column<double>(nullable: true),
                    EventLocation_Longitude = table.Column<double>(nullable: true),
                    ToLocation_Latitude = table.Column<double>(nullable: true),
                    ToLocation_Longitude = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimelineEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimelineEvents_Animals_AnimalId",
                        column: x => x.AnimalId,
                        principalSchema: "rescute",
                        principalTable: "Animals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TimelineEvents_Samaritans_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "rescute",
                        principalTable: "Samaritans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BillContributions",
                schema: "rescute",
                columns: table => new
                {
                    BillAttachedId = table.Column<string>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    Amount = table.Column<long>(nullable: false),
                    TransactionId = table.Column<string>(nullable: true),
                    ContributorId = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillContributions", x => new { x.BillAttachedId, x.Id });
                    table.ForeignKey(
                        name: "FK_BillContributions_TimelineEvents_BillAttachedId",
                        column: x => x.BillAttachedId,
                        principalSchema: "rescute",
                        principalTable: "TimelineEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BillContributions_Samaritans_ContributorId",
                        column: x => x.ContributorId,
                        principalSchema: "rescute",
                        principalTable: "Samaritans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                schema: "rescute",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    CommentText = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    RepliesTo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Samaritans_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "rescute",
                        principalTable: "Samaritans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_TimelineEvents_RepliesTo",
                        column: x => x.RepliesTo,
                        principalSchema: "rescute",
                        principalTable: "TimelineEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TimelineEventAttachments",
                schema: "rescute",
                columns: table => new
                {
                    TimelineEventWithAttachmentsId = table.Column<string>(nullable: false),
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
                    table.PrimaryKey("PK_TimelineEventAttachments", x => new { x.TimelineEventWithAttachmentsId, x.Id });
                    table.ForeignKey(
                        name: "FK_TimelineEventAttachments_TimelineEvents_TimelineEventWithAttachmentsId",
                        column: x => x.TimelineEventWithAttachmentsId,
                        principalSchema: "rescute",
                        principalTable: "TimelineEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Animals_IntroducedBy",
                schema: "rescute",
                table: "Animals",
                column: "IntroducedBy");

            migrationBuilder.CreateIndex(
                name: "IX_BillContributions_ContributorId",
                schema: "rescute",
                table: "BillContributions",
                column: "ContributorId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CreatedBy",
                schema: "rescute",
                table: "Comments",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_RepliesTo",
                schema: "rescute",
                table: "Comments",
                column: "RepliesTo");

            migrationBuilder.CreateIndex(
                name: "IX_TimelineEvents_AnimalId",
                schema: "rescute",
                table: "TimelineEvents",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_TimelineEvents_CreatedBy",
                schema: "rescute",
                table: "TimelineEvents",
                column: "CreatedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimalAttachments",
                schema: "rescute");

            migrationBuilder.DropTable(
                name: "BillContributions",
                schema: "rescute");

            migrationBuilder.DropTable(
                name: "Comments",
                schema: "rescute");

            migrationBuilder.DropTable(
                name: "TimelineEventAttachments",
                schema: "rescute");

            migrationBuilder.DropTable(
                name: "TimelineEvents",
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
