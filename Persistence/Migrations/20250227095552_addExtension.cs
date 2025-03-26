using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addExtension : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Extension",
                table: "AttachmentTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AttachmentTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Extension",
                value: null);

            migrationBuilder.UpdateData(
                table: "AttachmentTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Extension",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Extension",
                table: "AttachmentTypes");
        }
    }
}
