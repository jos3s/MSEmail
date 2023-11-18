using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MSEmail.Infra.Migrations
{
    /// <inheritdoc />
    public partial class _0008 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClassName",
                table: "ExceptionLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClassName",
                table: "ExceptionLogs");
        }
    }
}
