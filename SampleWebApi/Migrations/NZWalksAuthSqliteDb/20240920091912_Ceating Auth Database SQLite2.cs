using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SampleWebApi.Migrations.NZWalksAuthSqliteDb
{
    /// <inheritdoc />
    public partial class CeatingAuthDatabaseSQLite2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4e101c50-6b93-407c-b178-19b453ca90e7", "4e101c50-6b93-407c-b178-19b453ca90e7", "Reader", "READER" },
                    { "a3a2cb1c-d593-49fb-b4e0-33fdf9dca365", "a3a2cb1c-d593-49fb-b4e0-33fdf9dca365", "Writer", "WRITER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4e101c50-6b93-407c-b178-19b453ca90e7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a3a2cb1c-d593-49fb-b4e0-33fdf9dca365");
        }
    }
}
