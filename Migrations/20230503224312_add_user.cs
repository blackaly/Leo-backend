using Leo.Model.Enums;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Leo.Migrations
{
    /// <inheritdoc />
    public partial class add_user : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[] { Guid.NewGuid().ToString(), nameof(Roles.Admin), nameof(Roles.Admin).ToUpper(), Guid.NewGuid().ToString() }
            );

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoles");
        }
    }
}
