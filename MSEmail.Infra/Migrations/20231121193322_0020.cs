using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MSEmail.Infra.Migrations
{
    /// <inheritdoc />
    public partial class _0020 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ServiceType",
                table: "SystemLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ServiceType",
                table: "ExceptionLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServiceType",
                table: "SystemLogs");
            
            migrationBuilder.DropColumn(
                name: "ServiceType",
                table: "ExceptionLogs");
        }
    }
}
