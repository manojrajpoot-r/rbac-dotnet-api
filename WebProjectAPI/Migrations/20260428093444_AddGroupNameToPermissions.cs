using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebProjectAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddGroupNameToPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
               name: "GroupName",
               table: "Permissions",
               type: "nvarchar(max)",
               nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
           name: "GroupName",
           table: "Permissions");
        }
    }
}
