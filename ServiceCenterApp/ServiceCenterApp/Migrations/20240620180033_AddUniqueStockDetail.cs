using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceCenterApp.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueStockDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StockDetails_StockId",
                table: "StockDetails");

            migrationBuilder.CreateIndex(
                name: "IX_StockDetails_StockId_DetailId",
                table: "StockDetails",
                columns: new[] { "StockId", "DetailId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StockDetails_StockId_DetailId",
                table: "StockDetails");

            migrationBuilder.CreateIndex(
                name: "IX_StockDetails_StockId",
                table: "StockDetails",
                column: "StockId");
        }
    }
}
