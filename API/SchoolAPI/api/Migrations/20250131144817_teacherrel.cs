using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class teacherrel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Student_StudentID",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Teacher_TeacherID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_StudentID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TeacherID",
                table: "AspNetUsers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "013859d1-3e96-4b29-a47b-1c119c92b28d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "967f9fdd-8bf0-4ce9-a4ad-93d87fbc25f7");

            migrationBuilder.DropColumn(
                name: "StudentID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TeacherID",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7b8dba50-86ea-462d-9436-33c8bbe79637", null, "User", "USER" },
                    { "a454b782-fef3-4d30-91cd-91fdcb092aea", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7b8dba50-86ea-462d-9436-33c8bbe79637");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a454b782-fef3-4d30-91cd-91fdcb092aea");

            migrationBuilder.AddColumn<int>(
                name: "StudentID",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeacherID",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "013859d1-3e96-4b29-a47b-1c119c92b28d", null, "User", "USER" },
                    { "967f9fdd-8bf0-4ce9-a4ad-93d87fbc25f7", null, "Admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_StudentID",
                table: "AspNetUsers",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TeacherID",
                table: "AspNetUsers",
                column: "TeacherID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Student_StudentID",
                table: "AspNetUsers",
                column: "StudentID",
                principalTable: "Student",
                principalColumn: "StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Teacher_TeacherID",
                table: "AspNetUsers",
                column: "TeacherID",
                principalTable: "Teacher",
                principalColumn: "TeacherID");
        }
    }
}
