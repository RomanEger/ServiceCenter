using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceCenterApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateClients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserWorks_Clients_ClientId",
                table: "UserWorks");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkDetail_Details_DetailId",
                table: "WorkDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkDetail_Works_WorkId",
                table: "WorkDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_Works_WorkType_WorkTypeId",
                table: "Works");

            migrationBuilder.DropIndex(
                name: "IX_UserWorks_ClientId",
                table: "UserWorks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkType",
                table: "WorkType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkDetail",
                table: "WorkDetail");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "UserWorks");

            migrationBuilder.RenameTable(
                name: "WorkType",
                newName: "WorkTypes");

            migrationBuilder.RenameTable(
                name: "WorkDetail",
                newName: "WorkDetails");

            migrationBuilder.RenameIndex(
                name: "IX_WorkDetail_WorkId",
                table: "WorkDetails",
                newName: "IX_WorkDetails_WorkId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkDetail_DetailId",
                table: "WorkDetails",
                newName: "IX_WorkDetails_DetailId");

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "Works",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkTypes",
                table: "WorkTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkDetails",
                table: "WorkDetails",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Works_ClientId",
                table: "Works",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkDetails_Details_DetailId",
                table: "WorkDetails",
                column: "DetailId",
                principalTable: "Details",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkDetails_Works_WorkId",
                table: "WorkDetails",
                column: "WorkId",
                principalTable: "Works",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Works_Clients_ClientId",
                table: "Works",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Works_WorkTypes_WorkTypeId",
                table: "Works",
                column: "WorkTypeId",
                principalTable: "WorkTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkDetails_Details_DetailId",
                table: "WorkDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkDetails_Works_WorkId",
                table: "WorkDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Works_Clients_ClientId",
                table: "Works");

            migrationBuilder.DropForeignKey(
                name: "FK_Works_WorkTypes_WorkTypeId",
                table: "Works");

            migrationBuilder.DropIndex(
                name: "IX_Works_ClientId",
                table: "Works");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkTypes",
                table: "WorkTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkDetails",
                table: "WorkDetails");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Works");

            migrationBuilder.RenameTable(
                name: "WorkTypes",
                newName: "WorkType");

            migrationBuilder.RenameTable(
                name: "WorkDetails",
                newName: "WorkDetail");

            migrationBuilder.RenameIndex(
                name: "IX_WorkDetails_WorkId",
                table: "WorkDetail",
                newName: "IX_WorkDetail_WorkId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkDetails_DetailId",
                table: "WorkDetail",
                newName: "IX_WorkDetail_DetailId");

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "UserWorks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkType",
                table: "WorkType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkDetail",
                table: "WorkDetail",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserWorks_ClientId",
                table: "UserWorks",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserWorks_Clients_ClientId",
                table: "UserWorks",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkDetail_Details_DetailId",
                table: "WorkDetail",
                column: "DetailId",
                principalTable: "Details",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkDetail_Works_WorkId",
                table: "WorkDetail",
                column: "WorkId",
                principalTable: "Works",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Works_WorkType_WorkTypeId",
                table: "Works",
                column: "WorkTypeId",
                principalTable: "WorkType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
