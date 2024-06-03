using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceCenterApp.Migrations
{
    /// <inheritdoc />
    public partial class ToNormalForm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkId = table.Column<int>(type: "int", nullable: false),
                    DetailId = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkDetail_Details_DetailId",
                        column: x => x.DetailId,
                        principalTable: "Details",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkDetail_Works_WorkId",
                        column: x => x.WorkId,
                        principalTable: "Works",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkDetail_DetailId",
                table: "WorkDetail",
                column: "DetailId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkDetail_WorkId",
                table: "WorkDetail",
                column: "WorkId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkDetail");
        }
    }
}
