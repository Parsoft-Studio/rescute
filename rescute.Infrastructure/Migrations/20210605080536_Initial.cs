using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace rescute.Infrastructure.Migrations
{
    public partial class Initial : Migration
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
                    LastName_Value = table.Column<string>(nullable: true),
                    Mobile_IsMobile = table.Column<bool>(nullable: true),
                    Mobile_Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Samaritans", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
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
                    IdentityCertificateId = table.Column<string>(nullable: true),
                    IntroducedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animals", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
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
                    Extension = table.Column<string>(nullable: true),
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
                name: "TimelineItems",
                schema: "rescute",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    EventDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    AnimalId = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    Total = table.Column<decimal>(nullable: true),
                    IncludesLabResults = table.Column<bool>(nullable: true),
                    IncludesPrescription = table.Column<bool>(nullable: true),
                    IncludesVetFee = table.Column<bool>(nullable: true),
                    EventLocation_Latitude = table.Column<double>(nullable: true),
                    EventLocation_Longitude = table.Column<double>(nullable: true),
                    ToLocation_Latitude = table.Column<double>(nullable: true),
                    ToLocation_Longitude = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimelineItems", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_TimelineItems_Animals_AnimalId",
                        column: x => x.AnimalId,
                        principalSchema: "rescute",
                        principalTable: "Animals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TimelineItems_Samaritans_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "rescute",
                        principalTable: "Samaritans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                        name: "FK_Comments_TimelineItems_RepliesTo",
                        column: x => x.RepliesTo,
                        principalSchema: "rescute",
                        principalTable: "TimelineItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContributionRequests",
                schema: "rescute",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    RequestDate = table.Column<DateTime>(nullable: false),
                    RequestCompletionDate = table.Column<DateTime>(nullable: true),
                    BillId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContributionRequests", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_ContributionRequests_TimelineItems_BillId",
                        column: x => x.BillId,
                        principalSchema: "rescute",
                        principalTable: "TimelineItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TimelineItemAttachments",
                schema: "rescute",
                columns: table => new
                {
                    TimelineItemWithAttachmentsId = table.Column<string>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(nullable: true),
                    Extension = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimelineItemAttachments", x => new { x.TimelineItemWithAttachmentsId, x.Id });
                    table.ForeignKey(
                        name: "FK_TimelineItemAttachments_TimelineItems_TimelineItemWithAttachmentsId",
                        column: x => x.TimelineItemWithAttachmentsId,
                        principalSchema: "rescute",
                        principalTable: "TimelineItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contributions",
                schema: "rescute",
                columns: table => new
                {
                    ContribRequestId = table.Column<string>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    TransactionId = table.Column<string>(nullable: true),
                    ContributorId = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contributions", x => new { x.ContribRequestId, x.Id });
                    table.ForeignKey(
                        name: "FK_Contributions_ContributionRequests_ContribRequestId",
                        column: x => x.ContribRequestId,
                        principalSchema: "rescute",
                        principalTable: "ContributionRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contributions_Samaritans_ContributorId",
                        column: x => x.ContributorId,
                        principalSchema: "rescute",
                        principalTable: "Samaritans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Animals_IntroducedBy",
                schema: "rescute",
                table: "Animals",
                column: "IntroducedBy");

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
                name: "IX_ContributionRequests_BillId",
                schema: "rescute",
                table: "ContributionRequests",
                column: "BillId",
                unique: true,
                filter: "[BillId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Contributions_ContributorId",
                schema: "rescute",
                table: "Contributions",
                column: "ContributorId");

            migrationBuilder.CreateIndex(
                name: "IX_TimelineItems_AnimalId",
                schema: "rescute",
                table: "TimelineItems",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_TimelineItems_CreatedBy",
                schema: "rescute",
                table: "TimelineItems",
                column: "CreatedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimalAttachments",
                schema: "rescute");

            migrationBuilder.DropTable(
                name: "Comments",
                schema: "rescute");

            migrationBuilder.DropTable(
                name: "Contributions",
                schema: "rescute");

            migrationBuilder.DropTable(
                name: "TimelineItemAttachments",
                schema: "rescute");

            migrationBuilder.DropTable(
                name: "ContributionRequests",
                schema: "rescute");

            migrationBuilder.DropTable(
                name: "TimelineItems",
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
