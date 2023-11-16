using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MsEmail.API.Context.Migrations
{
    /// <inheritdoc />
    public partial class _0007 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CreationUserId",
                table: "Users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "UpdateUserId",
                table: "Users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "CreationUserId",
                table: "SystemLogs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "UpdateUserId",
                table: "SystemLogs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "CreationUserId",
                table: "ExceptionLogs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "UpdateUserId",
                table: "ExceptionLogs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "CreationUserId",
                table: "Emails",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "UpdateUserId",
                table: "Emails",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationUserId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UpdateUserId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreationUserId",
                table: "SystemLogs");

            migrationBuilder.DropColumn(
                name: "UpdateUserId",
                table: "SystemLogs");

            migrationBuilder.DropColumn(
                name: "CreationUserId",
                table: "ExceptionLogs");

            migrationBuilder.DropColumn(
                name: "UpdateUserId",
                table: "ExceptionLogs");

            migrationBuilder.DropColumn(
                name: "CreationUserId",
                table: "Emails");

            migrationBuilder.DropColumn(
                name: "UpdateUserId",
                table: "Emails");
        }
    }
}
