using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Panda.SEOTracker.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "SEO");

            migrationBuilder.CreateTable(
                name: "TrackedUrls",
                schema: "SEO",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", maxLength: 100, nullable: false, defaultValueSql: "NEWID()"),
                    Url = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    TotalResultsToCheck = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackedUrls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SearchTerms",
                schema: "SEO",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", maxLength: 100, nullable: false, defaultValueSql: "NEWID()"),
                    Term = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    TrackedUrlId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchTerms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SearchTerms_TrackedUrls_TrackedUrlId",
                        column: x => x.TrackedUrlId,
                        principalSchema: "SEO",
                        principalTable: "TrackedUrls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SearchTermHistories",
                schema: "SEO",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", maxLength: 100, nullable: false, defaultValueSql: "NEWID()"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SearchEngineUsed = table.Column<int>(type: "int", nullable: false),
                    Positions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SearchTermId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchTermHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SearchTermHistories_SearchTerms_SearchTermId",
                        column: x => x.SearchTermId,
                        principalSchema: "SEO",
                        principalTable: "SearchTerms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SearchTermHistories_SearchTermId",
                schema: "SEO",
                table: "SearchTermHistories",
                column: "SearchTermId");

            migrationBuilder.CreateIndex(
                name: "IX_SearchTerms_TrackedUrlId",
                schema: "SEO",
                table: "SearchTerms",
                column: "TrackedUrlId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SearchTermHistories",
                schema: "SEO");

            migrationBuilder.DropTable(
                name: "SearchTerms",
                schema: "SEO");

            migrationBuilder.DropTable(
                name: "TrackedUrls",
                schema: "SEO");
        }
    }
}
