using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addExtension1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AttachmentTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Extension",
                value: ".pdf");

            migrationBuilder.UpdateData(
                table: "AttachmentTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Extension",
                value: ".Jpg,.png");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
