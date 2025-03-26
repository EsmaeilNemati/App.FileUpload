using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addExtension2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AttachmentTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Extension",
                value: ".jpg,.jpeg,.png");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AttachmentTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Extension",
                value: ".Jpg,.png");
        }
    }
}
