using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceCenterApp.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkTypeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Works");

            migrationBuilder.AddColumn<int>(
                name: "WorkTypeId",
                table: "Works",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "WorkType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Works_WorkTypeId",
                table: "Works",
                column: "WorkTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Works_WorkType_WorkTypeId",
                table: "Works",
                column: "WorkTypeId",
                principalTable: "WorkType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Works_WorkType_WorkTypeId",
                table: "Works");

            migrationBuilder.DropTable(
                name: "WorkType");

            migrationBuilder.DropIndex(
                name: "IX_Works_WorkTypeId",
                table: "Works");

            migrationBuilder.DropColumn(
                name: "WorkTypeId",
                table: "Works");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Works",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
