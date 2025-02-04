using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class emptyappuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4a803555-f46d-442f-8741-3110cb27059b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e7d652f9-4ab6-43cc-96dc-e525eab3f1ed");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "013859d1-3e96-4b29-a47b-1c119c92b28d", null, "User", "USER" },
                    { "967f9fdd-8bf0-4ce9-a4ad-93d87fbc25f7", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "013859d1-3e96-4b29-a47b-1c119c92b28d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "967f9fdd-8bf0-4ce9-a4ad-93d87fbc25f7");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4a803555-f46d-442f-8741-3110cb27059b", null, "User", "USER" },
                    { "e7d652f9-4ab6-43cc-96dc-e525eab3f1ed", null, "Admin", "ADMIN" }
                });
        }
    }
}
