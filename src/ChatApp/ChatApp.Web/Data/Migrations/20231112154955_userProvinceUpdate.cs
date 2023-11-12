using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApp.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class userProvinceUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Provience",
                table: "Users",
                newName: "Province");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Province",
                table: "Users",
                newName: "Provience");
        }
    }
}
