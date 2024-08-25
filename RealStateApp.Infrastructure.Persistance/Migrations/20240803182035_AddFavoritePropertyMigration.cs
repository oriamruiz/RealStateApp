using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealStateApp.Infrastructure.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class AddFavoritePropertyMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyImprovements_Improvements_ImprovementId",
                table: "PropertyImprovements");

            migrationBuilder.CreateTable(
                name: "FavoriteProperties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    PropertyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavoriteProperties_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteProperties_ClientId_PropertyId",
                table: "FavoriteProperties",
                columns: new[] { "ClientId", "PropertyId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteProperties_PropertyId",
                table: "FavoriteProperties",
                column: "PropertyId");

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyImprovements_Improvements_ImprovementId",
                table: "PropertyImprovements",
                column: "ImprovementId",
                principalTable: "Improvements",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyImprovements_Improvements_ImprovementId",
                table: "PropertyImprovements");

            migrationBuilder.DropTable(
                name: "FavoriteProperties");

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyImprovements_Improvements_ImprovementId",
                table: "PropertyImprovements",
                column: "ImprovementId",
                principalTable: "Improvements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
